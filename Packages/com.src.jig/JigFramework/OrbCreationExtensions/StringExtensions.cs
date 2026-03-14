using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace OrbCreationExtensions
{
	public static class StringExtensions
	{
		public static string MakeString(this string[] aPath)
		{
			string text = "";
			if (aPath != null)
			{
				if (aPath.Length != 0)
				{
					text = aPath[0];
				}
				for (int i = 1; i < aPath.Length; i++)
				{
					if (aPath.Length == 0)
					{
						return text;
					}
					text = text + "/" + aPath[i];
				}
			}
			return text;
		}

		public static string[] TruncatePath(this string[] aPath, int newlen)
		{
			int num = 0;
			string[] array = new string[newlen];
			for (num = 0; num < aPath.Length && num < newlen; num++)
			{
				array[num] = aPath[num];
			}
			return array;
		}

		public static string MakeString(this string aStr)
		{
			if (aStr == null)
			{
				return "";
			}
			return aStr;
		}

		public static int MakeInt(this string str)
		{
			int result = 0;
			if (str != null && int.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0;
		}

		public static float MakeFloat(this string aStr)
		{
			float result = 0f;
			if (aStr != null && float.TryParse(aStr, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0f;
		}

		public static double MakeDouble(this string aStr)
		{
			double result = 0.0;
			if (aStr != null && double.TryParse(aStr, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}

		public static bool MakeBool(this string aStr)
		{
			int result = 0;
			if (aStr == "True")
			{
				return true;
			}
			if (aStr != null && int.TryParse(aStr, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result > 0;
			}
			return false;
		}

		public static Color MakeColor(this string aStr)
		{
			Color result = new Color(0f, 0f, 0f, 0f);
			if (aStr != null && aStr.Length > 0)
			{
				try
				{
					if (!(aStr.Substring(0, 1) == "#"))
					{
						if (aStr.IndexOf(",", 0) < 0)
						{
							if (aStr.IndexOf(" ", 0) < 0)
							{
								return result;
							}
							int num = 0;
							int num2 = 0;
							int num3 = 0;
							float num4 = 1f;
							if (aStr.IndexOf(".", 0) < 0)
							{
								num4 = 255f;
							}
							num2 = aStr.IndexOf(" ", num);
							while (num2 > num && num3 < 4)
							{
								result[num3++] = Mathf.Clamp01(aStr.Substring(num, num2 - num).MakeFloat() / num4);
								num = num2 + 1;
								if (num < aStr.Length)
								{
									num2 = aStr.IndexOf(" ", num);
								}
								if (num2 < 0)
								{
									num2 = aStr.Length;
								}
							}
							if (num3 >= 4)
							{
								return result;
							}
							result.a = 1f;
							return result;
						}
						int num5 = 0;
						int num6 = 0;
						int num7 = 0;
						float num8 = 1f;
						if (aStr.IndexOf(".", 0) < 0)
						{
							num8 = 255f;
						}
						num6 = aStr.IndexOf(",", num5);
						while (num6 > num5 && num7 < 4)
						{
							result[num7++] = Mathf.Clamp01(aStr.Substring(num5, num6 - num5).MakeFloat() / num8);
							num5 = num6 + 1;
							if (num5 < aStr.Length)
							{
								num6 = aStr.IndexOf(",", num5);
							}
							if (num6 < 0)
							{
								num6 = aStr.Length;
							}
						}
						if (num7 >= 4)
						{
							return result;
						}
						result.a = 1f;
						return result;
					}
					string text = aStr.Substring(1, aStr.Length - 1);
					try
					{
						result.r = (float)int.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture) / 255f;
						result.g = (float)int.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture) / 255f;
						result.b = (float)int.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture) / 255f;
					}
					catch (Exception ex)
					{
						Debug.Log("Could not convert " + aStr + " to Color. " + ex);
					}
					if (text.Length != 8)
					{
						result.a = 1f;
						return result;
					}
					result.a = (float)int.Parse(text.Substring(6, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture) / 255f;
					return result;
				}
				catch (Exception ex2)
				{
					Debug.Log("Could not convert " + aStr + " to Color. " + ex2);
					return result;
				}
			}
			return result;
		}

		public static Vector3 MakeVector3(this string aStr)
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (aStr != null && aStr.Length > 0)
			{
				try
				{
					if (aStr.IndexOf(",", 0) < 0)
					{
						return result;
					}
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					num2 = aStr.IndexOf(",", num);
					while (num2 > num)
					{
						if (num3 > 3)
						{
							return result;
						}
						result[num3++] = float.Parse(aStr.Substring(num, num2 - num), CultureInfo.InvariantCulture);
						num = num2 + 1;
						if (num < aStr.Length)
						{
							num2 = aStr.IndexOf(",", num);
						}
						if (num2 < 0)
						{
							num2 = aStr.Length;
						}
					}
					return result;
				}
				catch (Exception ex)
				{
					Debug.Log("Could not convert " + aStr + " to Vector3. " + ex);
					return new Vector3(0f, 0f, 0f);
				}
			}
			return result;
		}

		public static Vector4 MakeVector4(this string aStr)
		{
			Vector4 result = new Vector4(0f, 0f, 0f, 0f);
			if (aStr != null && aStr.Length > 0)
			{
				try
				{
					if (aStr.IndexOf(",", 0) < 0)
					{
						return result;
					}
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					num2 = aStr.IndexOf(",", num);
					while (num2 > num)
					{
						if (num3 > 4)
						{
							return result;
						}
						result[num3++] = float.Parse(aStr.Substring(num, num2 - num), CultureInfo.InvariantCulture);
						num = num2 + 1;
						if (num < aStr.Length)
						{
							num2 = aStr.IndexOf(",", num);
						}
						if (num2 < 0)
						{
							num2 = aStr.Length;
						}
					}
					return result;
				}
				catch (Exception ex)
				{
					Debug.Log("Could not convert " + aStr + " to Vector3. " + ex);
					return new Vector4(0f, 0f, 0f, 0f);
				}
			}
			return result;
		}

		public static int IndexOfChars(this string str, string searchChars, int startAt)
		{
			for (int i = startAt; i < str.Length; i++)
			{
				char value = str[i];
				if (searchChars.IndexOf(value) >= 0)
				{
					return i;
				}
			}
			return -1;
		}

		public static int IndexOfEndOfLine(this string str, int startAt)
		{
			int num = str.IndexOf('\n', startAt);
			if (num >= startAt)
			{
				if (num > 0 && str[num - 1] == '\r')
				{
					return num - 1;
				}
				return num;
			}
			return str.IndexOf('\r', startAt);
		}

		public static char StartOfNextLine(this string str, int startAt)
		{
			int num = str.IndexOfEndOfLine(startAt);
			if (num > str.Length - 3)
			{
				return ' ';
			}
			return str[num + 1];
		}

		public static int NextLineIndex(this string str, int startAt)
		{
			int num = str.IndexOfEndOfLine(startAt);
			if (num > str.Length - 3)
			{
				return -1;
			}
			return num + 1;
		}

		public static int EndOfCharRepetition(this string str, int startAt)
		{
			if (startAt < str.Length)
			{
				int num = startAt;
				char c = str[num];
				while (num < str.Length - 1)
				{
					num++;
					if (str[num] != c)
					{
						return num;
					}
				}
			}
			return str.Length;
		}

		public static string Truncate(this string str, int maxLength)
		{
			if (str.Length > maxLength)
			{
				return str.Substring(0, maxLength);
			}
			return str;
		}

		public static string TrimChars(this string str, string trimChars)
		{
			int i = 0;
			int num;
			for (num = str.Length; i < num; i++)
			{
				char value = str[i];
				if (trimChars.IndexOf(value) < 0)
				{
					break;
				}
			}
			while (num > i)
			{
				char value2 = str[num];
				if (trimChars.IndexOf(value2) < 0)
				{
					break;
				}
				num--;
			}
			if (i > 0 || num < str.Length)
			{
				return str.Substring(i, num - i);
			}
			return str;
		}

		public static string SubstringAfter(this string str, string after)
		{
			int num = str.IndexOf(after);
			if (num >= 0)
			{
				num += after.Length;
				return str.Substring(num, str.Length - num);
			}
			return str;
		}

		public static List<int> ToIntList(this string str, char separator)
		{
			List<int> list = new List<int>();
			int num = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || c == '\t' || i == str.Length - 1)
				{
					if (num < i && flag)
					{
						int result = 0;
						if (int.TryParse(str.Substring(num, i - num), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
						{
							list.Add(result);
						}
						flag = false;
					}
					num = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num = i;
				}
			}
			return list;
		}

		public static float[] ToFloatArray(this string str, char separator)
		{
			List<float> floats = new List<float>();
			str.IntoFloatList(ref floats, separator);
			return floats.ToArray();
		}

		public static List<float> ToFloatList(this string str, char separator)
		{
			List<float> floats = new List<float>();
			str.IntoFloatList(ref floats, separator);
			return floats;
		}

		public static void IntoFloatList(this string str, ref List<float> floats, char separator)
		{
			int num = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || c == '\t' || i == str.Length - 1)
				{
					if (num < i && flag)
					{
						float result = 0f;
						if (float.TryParse(str.Substring(num, i - num), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
						{
							floats.Add(result);
						}
						flag = false;
					}
					num = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num = i;
				}
			}
		}

		public static List<Vector2> ToVector2List(this string str, char separator)
		{
			List<Vector2> vectors = new List<Vector2>();
			str.IntoVector2List(ref vectors, separator, 2);
			return vectors;
		}

		public static List<Vector2> ToVector2List(this string str, char separator, int floatsPerValue)
		{
			List<Vector2> vectors = new List<Vector2>();
			str.IntoVector2List(ref vectors, separator, floatsPerValue);
			return vectors;
		}

		public static void IntoVector2List(this string str, ref List<Vector2> vectors, char separator, int floatsPerValue)
		{
			int num = 0;
			Vector2 item = new Vector2(0f, 0f);
			int num2 = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || i == str.Length - 1)
				{
					if (num2 < i && flag)
					{
						float result = 0f;
						if (float.TryParse(str.Substring(num2, i - num2), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
						{
							if (num < 2)
							{
								item[num] = result;
							}
							num++;
							if (num == floatsPerValue)
							{
								num = 0;
								vectors.Add(item);
								item = new Vector2(0f, 0f);
							}
						}
						flag = false;
					}
					num2 = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num2 = i;
				}
			}
		}

		public static Vector3 ToVector3(this string str, char separator, Vector3 defaultValue)
		{
			List<Vector3> vectors = new List<Vector3>();
			str.IntoVector3List(ref vectors, separator);
			if (vectors.Count > 0)
			{
				return vectors[0];
			}
			return defaultValue;
		}

		public static List<Vector3> ToVector3List(this string str, char separator)
		{
			List<Vector3> vectors = new List<Vector3>();
			str.IntoVector3List(ref vectors, separator);
			return vectors;
		}

		public static void IntoVector3List(this string str, ref List<Vector3> vectors, char separator)
		{
			int num = 0;
			Vector3 item = new Vector3(0f, 0f, 0f);
			int num2 = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || i == str.Length - 1)
				{
					if (num2 < i && flag)
					{
						float result = 0f;
						if (float.TryParse(str.Substring(num2, i - num2), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
						{
							item[num++] = result;
							if (num == 3)
							{
								num = 0;
								vectors.Add(item);
								item = new Vector3(0f, 0f, 0f);
							}
						}
						flag = false;
					}
					num2 = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num2 = i;
				}
			}
		}

		public static string FilterComments(this string str, string beginComment, string endComment, string replaceWith)
		{
			int num = 0;
			int startIndex = 0;
			for (num = str.IndexOf(beginComment, startIndex); num > 0; num = str.IndexOf(beginComment, num))
			{
				startIndex = str.IndexOf(endComment, num);
				if (startIndex <= num)
				{
					break;
				}
				startIndex += endComment.Length;
				str = str.Substring(0, num) + replaceWith + str.Substring(startIndex, str.Length - startIndex);
			}
			return str;
		}

		public static string XmlDecode(this string str)
		{
			str = str.Replace("&lt;", "<");
			str = str.Replace("&gt;", ">");
			str = str.Replace("&amp;", "&");
			str = str.Replace("&apos;", "'");
			str = str.Replace("&quot;", "\"");
			return str;
		}

		public static string XmlEncode(this string str)
		{
			str = str.Replace("&", "&amp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			str = str.Replace("'", "&apos;");
			str = str.Replace("\"", "&quot;");
			return str;
		}

		public static string JsonDecode(this string str)
		{
			str = str.Replace("\\/", "/");
			str = str.Replace("\\n", "\n");
			str = str.Replace("\\r", "\r");
			str = str.Replace("\\t", "\t");
			str = str.Replace("\\\"", "\"");
			str = str.Replace("\\\\", "\\");
			return str;
		}

		public static string JsonEncode(this string str)
		{
			str = str.Replace("\"", "\\\"");
			str = str.Replace("\\", "\\\\");
			str = str.Replace("\n", "\\n");
			str = str.Replace("\r", "\\r");
			str = str.Replace("\t", "\\t");
			return str;
		}
	}
}
