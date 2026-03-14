using System.Collections;
using UnityEngine;

namespace OrbCreationExtensions
{
	public static class CollectionExtensions
	{
		public static object GetValue(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				return hash[key];
			}
			return null;
		}

		public static Hashtable GetHashtable(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key) && hash[key].GetType() == typeof(Hashtable))
			{
				return (Hashtable)hash[key];
			}
			return null;
		}

		public static ArrayList GetArrayList(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key) && hash[key].GetType() == typeof(ArrayList))
			{
				return (ArrayList)hash[key];
			}
			return null;
		}

		public static ArrayList GetArrayList(this Hashtable hash, object key, bool wrap)
		{
			if (hash.ContainsKey(key))
			{
				if (hash[key].GetType() == typeof(ArrayList))
				{
					return (ArrayList)hash[key];
				}
				if (wrap)
				{
					return new ArrayList { hash[key] };
				}
			}
			return null;
		}

		public static string GetString(this Hashtable hash, object key)
		{
			return hash.GetString(key, null);
		}

		public static string GetString(this Hashtable hash, object key, string defaultValue)
		{
			object obj = null;
			if (hash.ContainsKey(key))
			{
				obj = hash[key];
				if (obj == null)
				{
					return defaultValue;
				}
				if (obj.GetType() == typeof(string))
				{
					return (string)obj;
				}
				if (obj.GetType() == typeof(bool))
				{
					return ((bool)obj).MakeString();
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return ((int)obj).MakeString();
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return ((float)obj).MakeString();
				}
			}
			return defaultValue;
		}

		public static int GetInt(this Hashtable hash, object key)
		{
			return hash.GetInt(key, 0);
		}

		public static int GetInt(this Hashtable hash, object key, int defaultValue)
		{
			object obj = null;
			if (hash.ContainsKey(key))
			{
				obj = hash[key];
				if (obj == null)
				{
					return defaultValue;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeInt();
				}
				if (obj.GetType() == typeof(bool) && (bool)obj)
				{
					return 1;
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return (int)obj;
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return Mathf.FloorToInt((float)obj);
				}
			}
			return defaultValue;
		}

		public static float GetFloat(this Hashtable hash, object key)
		{
			return hash.GetFloat(key, 0f);
		}

		public static float GetFloat(this Hashtable hash, object key, float defaultValue)
		{
			object obj = null;
			if (hash.ContainsKey(key))
			{
				obj = hash[key];
				if (obj == null)
				{
					return defaultValue;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeFloat();
				}
				if (obj.GetType() == typeof(bool) && (bool)obj)
				{
					return 1f;
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return (float)obj;
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return (float)obj;
				}
			}
			return defaultValue;
		}

		public static bool GetBool(this Hashtable hash, object key)
		{
			object obj = null;
			if (hash.ContainsKey(key))
			{
				obj = hash[key];
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeBool();
				}
				if (obj.GetType() == typeof(bool) && (bool)obj)
				{
					return (bool)obj;
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return (int)obj > 0;
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return (float)obj > 0f;
				}
			}
			return false;
		}

		public static Color GetColor(this Hashtable hash, object key)
		{
			object obj = null;
			if (hash.ContainsKey(key))
			{
				obj = hash[key];
				if (obj == null)
				{
					return new Color(0f, 0f, 0f, 0f);
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeColor();
				}
				if (obj.GetType() == typeof(Color))
				{
					return (Color)obj;
				}
			}
			return new Color(0f, 0f, 0f, 0f);
		}

		public static Vector3 GetVector3(this Hashtable hash, object key)
		{
			object obj = null;
			if (hash.ContainsKey(key))
			{
				obj = hash[key];
				if (obj == null)
				{
					return Vector3.zero;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeVector3();
				}
				if (obj.GetType() == typeof(Vector3))
				{
					return (Vector3)obj;
				}
			}
			return Vector3.zero;
		}

		public static Texture2D GetTexture2D(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return null;
				}
				if (obj.GetType() == typeof(Texture2D))
				{
					return (Texture2D)obj;
				}
			}
			return null;
		}

		public static byte[] GetBytes(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return null;
				}
				if (obj.GetType() == typeof(byte[]))
				{
					return (byte[])obj;
				}
			}
			return null;
		}

		public static void AddHashtable(this Hashtable hash, Hashtable addHash, bool overwriteExistingKeys)
		{
			foreach (string key in addHash.Keys)
			{
				if (overwriteExistingKeys || !hash.ContainsKey(key))
				{
					hash[key] = addHash[key];
				}
			}
		}

		public static string XmlString(this Hashtable hash)
		{
			string str = "";
			hash.XmlString(ref str, 0);
			return str;
		}

		public static string XmlString(this ArrayList arr)
		{
			string str = "";
			arr.XmlString(ref str, 0);
			return str;
		}

		private static void XmlString(this Hashtable hash, ref string str, int level)
		{
			bool flag = false;
			string text = null;
			if (hash.ContainsKey(".tag."))
			{
				text = (string)hash[".tag."];
				MoveToNewLineIfNeeded(ref str, level);
				str = str + "<" + text;
				if (hash.Count < 6 && hash.ContainsKey(text))
				{
					flag = true;
					foreach (string key in hash.Keys)
					{
						if (key != text && (hash[key].GetType() == typeof(Hashtable) || hash[key].GetType() == typeof(ArrayList)))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					foreach (string key2 in hash.Keys)
					{
						if (key2 != text && key2 != ".tag.")
						{
							str = str + " " + key2 + "=\"" + hash[key2]?.ToString() + "\"";
						}
					}
				}
				str += ">\n";
				level++;
			}
			if (flag)
			{
				MoveToNewLineIfNeeded(ref str, level);
				str = str + hash[text]?.ToString() + "\n";
			}
			else
			{
				foreach (string key3 in hash.Keys)
				{
					if (key3 != ".tag.")
					{
						object obj = hash[key3];
						MoveToNewLineIfNeeded(ref str, level);
						str = str + "<" + key3 + ">";
						level++;
						if (obj == null)
						{
							str += "NULL";
						}
						else if (obj.GetType() == typeof(Hashtable))
						{
							((Hashtable)obj).XmlString(ref str, level);
						}
						else if (obj.GetType() == typeof(ArrayList))
						{
							((ArrayList)obj).XmlString(ref str, level);
						}
						else if (obj.GetType() == typeof(string))
						{
							str += ((string)obj).XmlEncode();
						}
						else
						{
							str += obj;
						}
						level--;
						MoveToNewLineIfNeeded(ref str, level);
						str = str + "</" + key3 + ">\n";
					}
				}
			}
			if (hash.ContainsKey(".tag."))
			{
				level--;
				MoveToNewLineIfNeeded(ref str, level);
				str = str + "</" + text + ">\n";
			}
		}

		private static void XmlString(this ArrayList arr, ref string str, int level)
		{
			MoveToNewLineIfNeeded(ref str, level);
			for (int i = 0; i < arr.Count; i++)
			{
				object obj = arr[i];
				MoveToNewLineIfNeeded(ref str, level);
				if (obj == null)
				{
					str += "NULL";
					str += "\n";
				}
				else if (obj.GetType() == typeof(Hashtable))
				{
					((Hashtable)obj).XmlString(ref str, level);
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					((ArrayList)obj).XmlString(ref str, level);
				}
				else if (obj.GetType() == typeof(string))
				{
					str += ((string)obj).XmlEncode();
					str += "\n";
				}
				else
				{
					str += obj;
					str += "\n";
				}
			}
		}

		private static void MoveToNewLineIfNeeded(ref string str, int level)
		{
			if (str.Length > 0 && str.Substring(str.Length - 1) == ">")
			{
				str += "\n";
				for (int i = 0; i < level; i++)
				{
					str += "\t";
				}
			}
			else if (str.Length > 0 && str.Substring(str.Length - 1) == "\n")
			{
				for (int j = 0; j < level; j++)
				{
					str += "\t";
				}
			}
		}

		public static string JsonString(this Hashtable hash)
		{
			string str = "";
			hash.JsonString(ref str, 0, true);
			return str;
		}

		public static string JsonString(this ArrayList arr)
		{
			string str = "";
			arr.JsonString(ref str, 0, true);
			return str;
		}

		public static string JsonString(this Hashtable hash, bool encode)
		{
			string str = "";
			hash.JsonString(ref str, 0, encode);
			return str;
		}

		public static string JsonString(this ArrayList arr, bool encode)
		{
			string str = "";
			arr.JsonString(ref str, 0, encode);
			return str;
		}

		private static void JsonString(this Hashtable hash, ref string str, int level, bool encode)
		{
			str += "{\n";
			level++;
			int num = 0;
			foreach (string key in hash.Keys)
			{
				object obj = hash[key];
				for (int i = 0; i < level; i++)
				{
					str += "\t";
				}
				if (encode)
				{
					str = str + "\"" + key + "\": ";
				}
				else
				{
					str = str + key + ": ";
				}
				if (obj == null)
				{
					str += "NULL";
				}
				else if (obj.GetType() == typeof(Hashtable))
				{
					((Hashtable)obj).JsonString(ref str, level, encode);
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					((ArrayList)obj).JsonString(ref str, level, encode);
				}
				else if (obj.GetType() == typeof(string))
				{
					if (encode)
					{
						str = str + "\"" + ((string)obj).JsonEncode() + "\"";
					}
					else
					{
						str += (string)obj;
					}
				}
				else
				{
					str = str + "\"" + obj?.ToString() + "\"";
				}
				num++;
				if (num < hash.Count)
				{
					str += ",";
				}
				str += "\n";
			}
			level--;
			for (int j = 0; j < level; j++)
			{
				str += "\t";
			}
			str += "}";
		}

		private static void JsonString(this ArrayList arr, ref string str, int level, bool encode)
		{
			str += "[\n";
			level++;
			for (int i = 0; i < arr.Count; i++)
			{
				object obj = arr[i];
				for (int j = 0; j < level; j++)
				{
					str += "\t";
				}
				if (obj == null)
				{
					str += "NULL";
				}
				else if (obj.GetType() == typeof(Hashtable))
				{
					((Hashtable)obj).JsonString(ref str, level, encode);
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					((ArrayList)obj).JsonString(ref str, level, encode);
				}
				else if (obj.GetType() == typeof(string))
				{
					if (encode)
					{
						str = str + "\"" + ((string)obj).JsonEncode() + "\"";
					}
					else
					{
						str += (string)obj;
					}
				}
				else
				{
					str = str + "\"" + obj?.ToString() + "\"";
				}
				if (i < arr.Count - 1)
				{
					str += ",";
				}
				str += "\n";
			}
			level--;
			for (int k = 0; k < level; k++)
			{
				str += "\t";
			}
			str += "]";
		}

		public static object GetNodeAtPath(this ArrayList inNodeList, string[] path)
		{
			return GetNodeAtPath(inNodeList, path, 0);
		}

		public static object GetNodeAtPath(this Hashtable inNodeHash, string[] path)
		{
			return GetNodeAtPath(inNodeHash, path, 0);
		}

		private static object GetNodeAtPath(ArrayList inNodeList, string[] path, int level)
		{
			if (inNodeList == null)
			{
				return null;
			}
			string text = path[level];
			for (int i = 0; i < inNodeList.Count; i++)
			{
				object obj = inNodeList[i];
				if (obj == null)
				{
					continue;
				}
				if (obj.GetType() == typeof(Hashtable))
				{
					if (!((Hashtable)obj).ContainsKey(".tag."))
					{
						return GetNodeAtPath((Hashtable)obj, path, level);
					}
					if ((string)((Hashtable)obj)[".tag."] == text)
					{
						if (level == path.Length - 1)
						{
							return obj;
						}
						return GetNodeAtPath((Hashtable)obj, path, level + 1);
					}
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					return GetNodeAtPath((ArrayList)obj, path, level);
				}
			}
			return null;
		}

		private static object GetNodeAtPath(Hashtable inNodeHash, string[] path, int level)
		{
			if (inNodeHash == null)
			{
				return null;
			}
			string key = path[level];
			if (inNodeHash.ContainsKey(key))
			{
				object obj = inNodeHash[key];
				if (level == path.Length - 1)
				{
					return inNodeHash[key];
				}
				if (obj != null)
				{
					if (obj.GetType() == typeof(Hashtable))
					{
						return GetNodeAtPath((Hashtable)obj, path, level + 1);
					}
					if (obj.GetType() == typeof(ArrayList))
					{
						return GetNodeAtPath((ArrayList)obj, path, level + 1);
					}
				}
			}
			return null;
		}

		public static Hashtable GetNodeWithProperty(this ArrayList inNodeList, string aKey, string aValue)
		{
			if (inNodeList == null)
			{
				return null;
			}
			for (int i = 0; i < inNodeList.Count; i++)
			{
				object obj = inNodeList[i];
				if (obj != null)
				{
					Hashtable hashtable = null;
					if (obj.GetType() == typeof(Hashtable))
					{
						hashtable = ((Hashtable)obj).GetNodeWithProperty(aKey, aValue);
					}
					else if (obj.GetType() == typeof(ArrayList))
					{
						hashtable = ((ArrayList)obj).GetNodeWithProperty(aKey, aValue);
					}
					if (hashtable != null)
					{
						return hashtable;
					}
				}
			}
			return null;
		}

		public static Hashtable GetNodeWithProperty(this Hashtable inNodeHash, string aKey, string aValue)
		{
			if (inNodeHash == null)
			{
				return null;
			}
			foreach (string key in inNodeHash.Keys)
			{
				object obj = inNodeHash[key];
				if (obj == null)
				{
					continue;
				}
				Hashtable hashtable = null;
				if (key == aKey)
				{
					if ((obj?.ToString() ?? "") == aValue)
					{
						hashtable = inNodeHash;
					}
				}
				else if (obj.GetType() == typeof(Hashtable))
				{
					hashtable = ((Hashtable)obj).GetNodeWithProperty(aKey, aValue);
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					hashtable = ((ArrayList)obj).GetNodeWithProperty(aKey, aValue);
				}
				if (hashtable != null)
				{
					return hashtable;
				}
			}
			return null;
		}

		public static Hashtable GetHashtable(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(Hashtable))
			{
				return (Hashtable)arr[index];
			}
			return null;
		}

		public static ArrayList GetArrayList(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(ArrayList))
			{
				return (ArrayList)arr[index];
			}
			return null;
		}

		public static ArrayList GetArrayList(this ArrayList arr, int index, bool wrap)
		{
			if (arr.Count > index && index >= 0)
			{
				if (arr[index].GetType() == typeof(ArrayList))
				{
					return (ArrayList)arr[index];
				}
				if (wrap)
				{
					return new ArrayList { arr[index] };
				}
			}
			return null;
		}

		public static string GetString(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(string))
			{
				return (string)arr[index];
			}
			return null;
		}

		public static float GetFloat(this ArrayList arr, int index, float defaultValue = 0f)
		{
			if (arr.Count > index && index >= 0)
			{
				if (arr[index].GetType() == typeof(float))
				{
					return (float)arr[index];
				}
				if (arr[index].GetType() == typeof(int))
				{
					return (float)arr[index];
				}
				if (arr[index].GetType() == typeof(string))
				{
					return ((string)arr[index]).MakeFloat();
				}
			}
			return defaultValue;
		}
	}
}
