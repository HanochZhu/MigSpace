namespace JigSpace.Core.FileImport
{
	public class FileImportSettings
	{
		public static int ImageMaxSize = 10;

		public static int ImageMaxResolution = 2048;

		public static int PhotoMaxResolution = 512;

		public static int VideoMaxSize = 50;

		public static int SoundMaxSize = 10;

		public static int ModelSmallMaxSize = 20;

		public static int ModelMediumMaxSize = 300;

		public static int ModelLargeMaxSize = 400;

		public static string[] AllowedImageExtensions = new string[3] { "jpg,jpeg", "png", "heic" };

		public static string[] AllowedSoundExtensions = new string[4] { "ogg", "mp3", "wav", "m4a" };

		public static string[] AllowedVideoExtensions = new string[2] { "mp4", "mov" };

		public static string[] AllowedTextExtensions = new string[1] { "json" };

		public static string[] AllowedModelExtensionsFree = new string[3] { "obj", "fbx", "step,stp" };

		public static string[] AllowedModelExtensionsPro = new string[5] { "obj", "fbx", "step,stp", "stl", "sldasm,sldprt,sldz" };

		public static string[] GetAllowedExtensionsForType(FileType fileType)
		{
			switch (fileType)
			{
			case FileType.IMAGE:
				return AllowedImageExtensions;
			case FileType.VIDEO:
				return AllowedVideoExtensions;
			case FileType.SOUND:
				return AllowedSoundExtensions;
			case FileType.TEXT:
				return AllowedTextExtensions;
			case FileType.MODEL:
			default:
				return null;
			}
		}
	}
}
