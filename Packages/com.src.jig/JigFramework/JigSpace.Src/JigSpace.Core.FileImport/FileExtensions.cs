using System;
using System.Collections.Generic;
using System.Linq;

namespace JigSpace.Core.FileImport
{
	public static class FileExtensions
	{
		public const string OBJ = "obj";

		public const string FBX = "fbx";

		public const string STL = "stl";

		public const string STP = "stp";

		public const string STEP = "step";

		public const string SLDASM = "sldasm";

		public const string SLDPRT = "sldprt";

		public const string SLDZ = "sldz";

		public const string PNG = "png";

		public const string JPG = "jpg";

		public const string JPEG = "jpeg";

		public const string HEIC = "heic";

		public const string ASTC = "astc";

		public const string MP4 = "mp4";

		public const string MOV = "mov";

		public const string AVI = "avi";

		public const string MP3 = "mp3";

		public const string OGG = "ogg";

		public const string WAV = "wav";

		public const string M4A = "m4a";

		public const string JSON = "json";

		public static FileType GetFileType(string extention)
		{
			extention = extention.Replace(".", "").ToLower();
			switch (extention)
			{
			case "obj":
			case "fbx":
			case "stl":
			case "stp":
			case "step":
			case "sldasm":
			case "sldprt":
			case "sldz":
				return FileType.MODEL;
			case "png":
			case "jpg":
			case "jpeg":
			case "heic":
			case "astc":
				return FileType.IMAGE;
			case "mp4":
			case "mov":
			case "avi":
				return FileType.VIDEO;
			case "mp3":
			case "ogg":
			case "wav":
			case "m4a":
				return FileType.SOUND;
			case "json":
				return FileType.TEXT;
			default:
				return FileType.UNKNOW;
			}
		}

		public static string MakeAllowedFileFormatsString(params string[] formats)
		{
			return string.Join(",", formats).ToLower();
		}

		public static string MakeShortFileFormatsString(params string[] formats)
		{
			return string.Join(", ", formats.Select(delegate(string x)
			{
				string[] array = x.Split(',', StringSplitOptions.None);
				return (array == null) ? null : array[0];
			})).ToLower();
		}

		public static string[] MakeSeperateFormatsArray(params string[] formats)
		{
			List<string> list = formats.ToList();
			for (int num = list.Count - 1; num >= 0; num--)
			{
				string[] array = list[num].Split(',', StringSplitOptions.None);
				if (array.Length > 1)
				{
					for (int i = 0; i < array.Length; i++)
					{
						list.Add(array[i]);
					}
					list.Remove(list[num]);
				}
			}
			return list.ToArray();
		}
	}
}
