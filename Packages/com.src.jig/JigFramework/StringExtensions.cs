using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtensions
{
	public static void CopyToClipboard(this string str)
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = str;
		textEditor.SelectAll();
		textEditor.Copy();
	}

	public static string Escaped(this string str)
	{
		return str.Replace("\\\"", "\"");
	}

	public static string AddGuid(this string str)
	{
		return $"{Guid.NewGuid()}_{str}";
	}

	public static float ToFloat(this string str)
	{
		float result;
		if (str != null && float.TryParse(str, out result))
		{
			return result;
		}
		return 0f;
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

	public static int IndexOfEndCharRepetition(this string str, int startAt)
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

	public static string GuidPart(this string str)
	{
		char[] separator = new char[1] { '_' };
		Guid guid;
		if (JigUtilities.TryParseGuid(str.Split(separator, 2)[0], out guid))
		{
			return guid.ToString();
		}
		return null;
	}

	public static string NamePart(this string str)
	{
		char[] separator = new char[1] { '_' };
		string[] array = str.Split(separator, 2);
		Guid guid;
		if (array.Length > 1 && JigUtilities.TryParseGuid(array[0], out guid))
		{
			return array[1];
		}
		return str;
	}

	public static string UpperCaseFirstLetter(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		char[] array = str.ToCharArray();
		array[0] = char.ToUpper(array[0]);
		return new string(array);
	}

	public static string LowerCaseFirstLetter(this string str, bool skipParentheses = false)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		char[] array = str.ToCharArray();
		int num = 0;
		if (skipParentheses && array[0] == '(')
		{
			num = 1;
		}
		if (array.Length <= 1 || !char.IsUpper(array[1]))
		{
			array[num] = char.ToLower(array[num]);
		}
		return new string(array);
	}

	public static bool IsNullOrEmpty(this string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static string ReplaceLast(this string str, string target, string value)
	{
		int num = str.LastIndexOf(target);
		if (num >= 0)
		{
			StringBuilder stringBuilder = new StringBuilder(str);
			stringBuilder.Remove(num, target.Length);
			stringBuilder.Insert(num, value);
			return stringBuilder.ToString();
		}
		return str;
	}

	public static int IndexOfReverse(this string str, char value, int startPos)
	{
		for (int num = startPos; num >= 0; num--)
		{
			if (str[num] == value)
			{
				return num;
			}
		}
		return 0;
	}

	public static int IndexOfForward(this string str, char value, int startPos)
	{
		for (int i = startPos; i < str.Length; i++)
		{
			if (str[i] == value)
			{
				return i;
			}
		}
		return str.Length;
	}

	public static bool IsEmail(this string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return false;
		}
		return Regex.IsMatch(email, "^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");
	}

	public static string Truncate(this string str, int length, bool ellipsis = true)
	{
		if (str.Length > length)
		{
			if (!ellipsis)
			{
				return str.Substring(0, length);
			}
			return str.Substring(0, length - 3) + "...";
		}
		return str;
	}

	public static string ReplaceInvalidFileNameChars(this string str, string replacement = "")
	{
		char[] invalidChars = Path.GetInvalidFileNameChars();
		return string.Concat(str.Select((char ch) => (!invalidChars.Contains(ch)) ? ch.ToString() : replacement));
	}

	public static string RemoveControlCharacters(this string str, char[] excludedChars = null)
	{
		StringBuilder stringBuilder = new StringBuilder(str.Length);
		if (excludedChars != null && excludedChars.Length != 0)
		{
			string text = str;
			foreach (char c in text)
			{
				if (!char.IsControl(c) || excludedChars.Contains(c))
				{
					stringBuilder.Append(c);
				}
			}
		}
		else
		{
			string text = str;
			foreach (char c2 in text)
			{
				if (!char.IsControl(c2))
				{
					stringBuilder.Append(c2);
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static string RemoveControlCharactersForJson(this string str)
	{
		return str.RemoveControlCharacters(new char[5] { '\n', '\r', '\t', '\b', '\f' });
	}

	public static string RemoveControlCharactersWithRegex(this string str)
	{
		return Regex.Replace(str, "\\p{Cc}", string.Empty);
	}

	public static JigMetaData.CopyPermissions ToCopyPermission(this string copyDisplay)
	{
		switch (copyDisplay)
		{
		default:
			return JigMetaData.CopyPermissions.owner_user;
		case "Team only":
			return JigMetaData.CopyPermissions.owner_tenant;
		case "Anyone can copy":
			return JigMetaData.CopyPermissions.anyone;
		}
	}
}
