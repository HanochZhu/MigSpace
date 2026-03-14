using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class JigUtilities
{
	[Serializable]
	public class ArrayWrapper<T>
	{
		public T[] Items;
	}

	public const string MatchEmailRegexPattern = "^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";

	public const string MatchJigHashPattern = "\\/[a-zA-Z0-9]{8}\\/";

	public static void UnloadUnusedAssets()
	{
		{
			Resources.UnloadUnusedAssets();
		}
	}

	public static bool TryParseGuid(string guidString, out Guid guid)
	{
		if (guidString == null)
		{
			guid = default(Guid);
			return false;
		}
		try
		{
			guid = new Guid(guidString);
			return true;
		}
		catch (FormatException)
		{
			guid = default(Guid);
			return false;
		}
	}

	public static T TryFromJson<T>(string json, bool tryRemoveInvalidChars = true)
	{
		try
		{
			return JsonUtility.FromJson<T>(json);
		}
		catch
		{
			if (!tryRemoveInvalidChars)
			{
				Debug.LogError("JSON failed to deserialize. (tryRemoveInvalidChars is FALSE)");
				return default(T);
			}
			try
			{
				Debug.Log("JSON failed to deserialize. Trying to remove invalid control characters.");
				return JsonUtility.FromJson<T>(json.RemoveControlCharactersForJson());
			}
			catch
			{
				Debug.LogError("JSON failed to deserialize, even after removing control characters.(tryRemoveInvalidChars is TRUE)");
				return default(T);
			}
		}
	}

	public static int CompareVersion(string ver0, string ver1)
	{
		List<string> list = ver0.Split('.', StringSplitOptions.None).ToList();
		List<string> list2 = ver1.Split('.', StringSplitOptions.None).ToList();
		for (int i = 0; i < list.Count - list2.Count; i++)
		{
			list2.Add("0");
		}
		for (int j = 0; j < list2.Count - list.Count; j++)
		{
			list.Add("0");
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (int.Parse(list[k]) > int.Parse(list2[k]))
			{
				return 1;
			}
			if (int.Parse(list2[k]) > int.Parse(list[k]))
			{
				return -1;
			}
		}
		return 0;
	}

	public static float MaxPlaneDimension(Vector3 extent)
	{
		if (!(extent.x > extent.z))
		{
			return extent.z;
		}
		return extent.x;
	}

	public static Vector3 UniformVector(float value)
	{
		return new Vector3(value, value, value);
	}

	public static List<T> FindAllObjectsOfType<T>(bool onlySceneObjects)
	{
		List<T> list = new List<T>();
		GameObject[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		if (array != null && array.Length != 0)
		{
			foreach (GameObject item in array.Where((GameObject x) => x.GetComponent<T>() != null))
			{
				if (!onlySceneObjects || (item.hideFlags != HideFlags.NotEditable && item.hideFlags != HideFlags.HideAndDontSave))
				{
					list.Add(item.GetComponent<T>());
				}
			}
			return list;
		}
		return list;
	}

	public static List<T> FindObjectsOfTypeAll<T>(bool includeInactive)
	{
		List<T> list = new List<T>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			GameObject[] rootGameObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				list.AddRange(gameObject.GetComponentsInChildren<T>(includeInactive));
			}
		}
		return list;
	}

	public static bool ColorsAreEqual(Color color1, Color color2, float tolerance)
	{
		if (Mathf.Abs(color1.r - color2.r) < tolerance && Mathf.Abs(color1.g - color2.g) < tolerance && Mathf.Abs(color1.b - color2.b) < tolerance)
		{
			return Mathf.Abs(color1.a - color2.a) < tolerance;
		}
		return false;
	}

	public static Color? HexToColor(string hexValue)
	{
		hexValue = hexValue.Replace("#", "");
		if (hexValue.Length < 6 || hexValue.Length > 8)
		{
			return null;
		}
		byte result;
		if (!byte.TryParse(hexValue.Substring(0, 2), NumberStyles.HexNumber, null, out result))
		{
			return null;
		}
		byte result2;
		if (!byte.TryParse(hexValue.Substring(2, 2), NumberStyles.HexNumber, null, out result2))
		{
			return null;
		}
		byte result3;
		if (!byte.TryParse(hexValue.Substring(4, 2), NumberStyles.HexNumber, null, out result3))
		{
			return null;
		}
		byte result4 = byte.MaxValue;
		if (hexValue.Length == 8 && !byte.TryParse(hexValue.Substring(6, 2), NumberStyles.HexNumber, null, out result4))
		{
			return null;
		}
		return new Color32(result, result2, result3, result4);
	}

	public static Dictionary<string, string> ParseQueryString(string queryString)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		queryString = queryString.Replace("?", "&");
		string[] array = queryString.Split('&', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('=', StringSplitOptions.None);
			if (array2.Length > 1 && !dictionary.ContainsKey(array2[0]))
			{
				dictionary.Add(array2[0], array2[1]);
			}
		}
		return dictionary;
	}

	public static string DictionaryToJsonString(Dictionary<string, string> dictionary)
	{
		string text = "{";
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			text = text + "\"" + item.Key + "\": \"" + item.Value + "\",";
		}
		return text.TrimEnd(',', ' ') + "}";
	}

	public static Bounds GetJigTotalBounds()
	{
		return GetObjectTotalBounds(JigSingleton<GameManager>.Instance.RootParent);
	}

	public static Bounds GetObjectTotalBounds(GameObject gameobject)
	{
		if (gameobject != null)
		{
			Quaternion rotation = gameobject.transform.rotation;
			List<MeshRenderer> list = (from r in gameobject.GetComponentsInChildren<MeshRenderer>(true).ToList()
				where r.gameObject.CompareTag("element")
				select r).ToList();
			if (list != null && list.Count > 0)
			{
				Bounds result = new Bounds(gameobject.transform.position, Vector3.zero);
				{
					foreach (MeshRenderer item in list)
					{
						if (item.gameObject.activeInHierarchy && item.enabled)
						{
							result.Encapsulate(item.bounds);
						}
					}
					return result;
				}
			}
		}
		Debug.LogError("No renderer - Can't calculate total bounds.");
		return default(Bounds);
	}

	public static Bounds GetTotalBounds(List<MeshRenderer> meshRenders)
	{
		Bounds result = default(Bounds);
		if (meshRenders == null)
		{
			return result;
		}
		int count = meshRenders.Count;
		for (int i = 0; i < count; i++)
		{
			Renderer renderer = meshRenders[i];
			if (renderer.enabled && renderer.gameObject.activeInHierarchy)
			{
				result.Encapsulate(renderer.bounds);
			}
		}
		return result;
	}

	public static string ToLowerCaseExtension(this string originalPath)
	{
		string extension = Path.GetExtension(originalPath);
		return Path.ChangeExtension(originalPath, extension.ToLower());
	}

	public static IEnumerator CallAfterDelay(float delay, Action call)
	{
		yield return new WaitForSeconds(delay);
		call?.Invoke();
	}

	public static bool DeviceHasNotch()
	{
		return Screen.safeArea.width < (float)Screen.width;
	}

	public static string CalculateMD5(byte[] bytes)
	{
		using (MD5 mD = MD5.Create())
		{
			return BitConverter.ToString(mD.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
		}
	}

	public static void ScrollToNormalizedValue(this ScrollRect sr, float value = 0f)
	{
		sr.verticalNormalizedPosition = Mathf.Clamp(value, 0f, 1f);
	}

	public static Texture2D ToTexture2DFromBase64(this string base64)
	{
		if (base64.IsNullOrEmpty())
		{
			return null;
		}
		byte[] data = Convert.FromBase64String(base64);
		Texture2D texture2D = new Texture2D(0, 0, TextureFormat.ARGB32, false);
		texture2D.LoadImage(data);
		return texture2D;
	}

	public static T Clone<T>(T obj)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			((IFormatter)binaryFormatter).Serialize((Stream)memoryStream, (object)obj);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return (T)((IFormatter)binaryFormatter).Deserialize((Stream)memoryStream);
		}
	}
}
