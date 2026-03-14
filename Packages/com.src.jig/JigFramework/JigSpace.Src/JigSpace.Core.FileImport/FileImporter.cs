using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crosstales.Common.Util;
using Crosstales.FB;
using UnityEngine;

namespace JigSpace.Core.FileImport
{
	public static class FileImporter
	{
		public struct ImportParameters
		{
			public string FolderPlayerPrefName;

			public List<string> AllowedExtensions;

			public FileImportOptions ImportOptions;

			public Action<FileImportResult> SingleFileCallback;

			public Action<List<FileImportResult>> MultipleFilesCallback;
		}

		public struct SaveImageParameters
		{
			public Texture2D Image;

			public Action FailureCallback;

			public Action<string> SuccessCallback;
		}

		private static ImportParameters _importParameters;

		public static SaveImageParameters SaveImageParams;

		public static void PickSingleFile(Action<FileImportResult> onImportCallback, FileImportOptions importOptions)
		{
			string text = "All ";
			string text2 = string.Empty;
			List<string> list = new List<string>();
			FileType[] fileTypes = importOptions.FileTypes;
			for (int i = 0; i < fileTypes.Length; i++)
			{
				FileType fileType = fileTypes[i];
				string text3 = fileType.ToString().ToLower();
				text2 += text3;
				text = text + " " + text3 + "s,";
				string[] allowedExtensionsForType = FileImportSettings.GetAllowedExtensionsForType(fileType);
				if (allowedExtensionsForType != null && allowedExtensionsForType.Any())
				{
					list.AddRange(allowedExtensionsForType);
				}
			}
			string @string = PlayerPrefs.GetString("Last" + text2 + "UploadFolder", string.Empty);
			ExtensionFilter[] extensions = MakeExtensionFilters(list.ToArray(), text);
			Singleton<FileBrowser>.Instance.OnOpenFilesComplete += OnOpenSingleFileComplete;
			ImportParameters importParameters = default(ImportParameters);
			importParameters.FolderPlayerPrefName = text2;
			importParameters.AllowedExtensions = list;
			importParameters.ImportOptions = importOptions;
			importParameters.SingleFileCallback = onImportCallback;
			_importParameters = importParameters;
			Singleton<FileBrowser>.Instance.OpenFilesAsync("Select a file to import", @string, "", false, extensions);
		}

		private static void OnSingleFilepathPicked(string pickedfilePath, string playerPrefName, List<string> allowedExtensions, FileImportOptions options, Action<FileImportResult> callback)
		{
			if (!string.IsNullOrEmpty(pickedfilePath))
			{
				PlayerPrefs.SetString("Last" + playerPrefName + "UploadFolder", Path.GetDirectoryName(pickedfilePath));
				FileImportError error = ValidateFile(pickedfilePath, allowedExtensions.ToArray(), options);
				FileInfo fileInfo = new FileInfo(pickedfilePath);
				FileImportResult importResult = new FileImportResult(error, pickedfilePath, FileExtensions.GetFileType(fileInfo.Extension));
				if (importResult.IsSuccess)
				{
					long fileSizeBytes = fileInfo.Length;
					
				}
				else
				{
					callback?.Invoke(importResult);
				}
			}
			else
			{
				callback?.Invoke(new FileImportResult(null, null, FileType.UNKNOW));
			}
		}

		public static void PickFiles(Action<List<FileImportResult>> callback, FileImportOptions options)
		{
			string text = "All ";
			string text2 = string.Empty;
			List<string> list = new List<string>();
			FileType[] fileTypes = options.FileTypes;
			for (int i = 0; i < fileTypes.Length; i++)
			{
				FileType fileType = fileTypes[i];
				string text3 = fileType.ToString().ToLower();
				text2 += text3;
				text = text + " " + text3 + "s,";
				string[] allowedExtensionsForType = FileImportSettings.GetAllowedExtensionsForType(fileType);
				if (allowedExtensionsForType != null && allowedExtensionsForType.Any())
				{
					list.AddRange(allowedExtensionsForType);
				}
			}
			string @string = PlayerPrefs.GetString("Last" + text2 + "UploadFolder", string.Empty);
			ExtensionFilter[] extensions = MakeExtensionFilters(list.ToArray(), text);
			Singleton<FileBrowser>.Instance.OnOpenFilesComplete += OnOpenMultipleFilesComplete;
			ImportParameters importParameters = default(ImportParameters);
			importParameters.FolderPlayerPrefName = text2;
			importParameters.AllowedExtensions = list;
			importParameters.ImportOptions = options;
			importParameters.SingleFileCallback = null;
			importParameters.MultipleFilesCallback = callback;
			_importParameters = importParameters;
			Singleton<FileBrowser>.Instance.OpenFilesAsync("Select the files to import", @string, "", true, extensions);
		}

		private static void OnOpenSingleFileComplete(bool selected, string singleFile, string[] files)
		{
			if (selected && !singleFile.IsNullOrEmpty())
			{
				OnSingleFilepathPicked(singleFile, _importParameters.FolderPlayerPrefName, _importParameters.AllowedExtensions, _importParameters.ImportOptions, _importParameters.SingleFileCallback);
			}
			Singleton<FileBrowser>.Instance.OnOpenFilesComplete -= OnOpenSingleFileComplete;
		}

		private static void OnOpenMultipleFilesComplete(bool selected, string singleFile, string[] files)
		{
			if (selected && files != null)
			{
				PlayerPrefs.SetString("Last" + _importParameters.FolderPlayerPrefName + "UploadFolder", Path.GetDirectoryName(files.FirstOrDefault()));
				List<FileImportResult> list = new List<FileImportResult>();
				foreach (string text in files)
				{
					if (!string.IsNullOrEmpty(text))
					{
						FileImportError error = ValidateFile(text, _importParameters.AllowedExtensions.ToArray(), _importParameters.ImportOptions);
						list.Add(new FileImportResult(error, text, FileExtensions.GetFileType(new FileInfo(text).Extension)));
					}
				}
				_importParameters.MultipleFilesCallback?.Invoke(list);
			}
			Singleton<FileBrowser>.Instance.OnOpenFilesComplete -= OnOpenMultipleFilesComplete;
		}

		private static void OnOpenSingleFolderComplete(bool selected, string singleFolder, string[] file)
		{
			if (selected && !singleFolder.IsNullOrEmpty())
			{
				string[] files = Directory.GetFiles(singleFolder);
				List<FileImportResult> list = new List<FileImportResult>();
				if (files != null && files.Length != 0)
				{
					PlayerPrefs.SetString("Last" + _importParameters.FolderPlayerPrefName + "UploadFolder", Path.GetDirectoryName(files.FirstOrDefault()));
					string[] array = files;
					foreach (string text in array)
					{
						if (!string.IsNullOrEmpty(text))
						{
							FileImportError error = ValidateFile(text, _importParameters.AllowedExtensions.ToArray(), _importParameters.ImportOptions);
							list.Add(new FileImportResult(error, text, FileExtensions.GetFileType(new FileInfo(text).Extension)));
						}
					}
				}
				_importParameters.MultipleFilesCallback?.Invoke(list);
			}
			Singleton<FileBrowser>.Instance.OnOpenFoldersComplete -= OnOpenSingleFolderComplete;
		}

		private static FileImportError ValidateSize(byte[] data, FileType fileType, FileImportOptions options, string fileExtension)
		{
			if (data == null)
			{
				return null;
			}
			FileImportError fileImportError = null;
			switch (fileType)
			{
			case FileType.IMAGE:
				if (options.ImageMaxSize >= 0 && data.Length > options.ImageMaxSize * 1024 * 1024)
				{
					fileImportError = new FileImportError("To maintain optimal AR performances please use " + $"files no larger than <b>{options.ImageMaxSize} MB</b>. " + "Please pick a smaller image, or optimise it further.\n\nCheck our <style=Link><link=https://link.jig.space/e/image-import-guidelines>import guidelines here.</link></style>", "Your image is too large.", FileImportErrorType.FILE_SIZE);
				}
				break;
			case FileType.SOUND:
				if (options.SoundMaxSize >= 0 && data.Length > options.SoundMaxSize * 1024 * 1024)
				{
					fileImportError = new FileImportError("To maintain optimal AR performances please use " + $"files no larger than <b>{options.SoundMaxSize} MB</b>. " + "Please pick a shorter audio file, or optimise it further.\n\nCheck our <style=Link><link=https://link.jig.space/e/sound-import-guidelines>import guidelines here.</link></style>", "Your audio file is too large.", FileImportErrorType.FILE_SIZE);
				}
				break;
			case FileType.VIDEO:
				if (options.VideoMaxSize >= 0 && data.Length > options.VideoMaxSize * 1024 * 1024)
				{
					fileImportError = new FileImportError("To maintain optimal AR performances please use " + $"files no larger than <b>{options.VideoMaxSize} MB</b>. " + "Please pick a shorter video, or optimise it further.\n\nCheck our <style=Link><link=https://link.jig.space/e/video-import-guidelines>import guidelines here.</link></style>", "Your video is too large.", FileImportErrorType.FILE_SIZE);
				}
				break;
			case FileType.MODEL:
				if (options.ModelMaxSize >= 0 && data.Length > options.ModelMaxSize * 1024 * 1024)
				{
				}
				break;
			default:
				fileImportError = new FileImportError("", "This file type is not supported, please try an other file");
				break;
			case FileType.TEXT:
				break;
			}
			if (fileImportError != null)
			{
			}
			return fileImportError;
		}

		private static FileImportError ValidateFile(string filePath, string[] allowedExtensions, FileImportOptions options)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return null;
			}
			FileInfo fileInfo = new FileInfo(filePath);
			string text = fileInfo.Extension.Replace(".", string.Empty).ToLower();
			if (!FileExtensions.MakeSeperateFormatsArray(allowedExtensions).Contains(text))
			{
				if (options.FileTypes.Length == 1)
				{

				}
				string text2 = string.Empty;
				FileType[] fileTypes = options.FileTypes;
				for (int i = 0; i < fileTypes.Length; i++)
				{
					FileType fileType = fileTypes[i];
					string[] allowedExtensionsForType = FileImportSettings.GetAllowedExtensionsForType(fileType);
					text2 = text2 + " <b>" + FileExtensions.MakeShortFileFormatsString(allowedExtensionsForType).ReplaceLast(",", " and") + "</b> for " + fileType.ToString().ToLower() + "s,";
				}
				return new FileImportError("We currently only support " + text2.Remove(text2.LastIndexOf(",")) + ".\n\nPlease pick a different file or re-export your file in the appropriate format.\n", "Format not supported");
			}
			FileType fileType2 = FileExtensions.GetFileType(fileInfo.Extension);
			return ValidateSize(File.ReadAllBytes(filePath), fileType2, options, Path.GetExtension(filePath));
		}

		private static ExtensionFilter[] MakeExtensionFilters(string[] allowedFileFormats, string combinedFilterName)
		{
			List<ExtensionFilter> list = new List<ExtensionFilter>
			{
				new ExtensionFilter(combinedFilterName, FileExtensions.MakeSeperateFormatsArray(allowedFileFormats))
			};
			foreach (string text in allowedFileFormats)
			{
				if (text.Split(',', StringSplitOptions.None).Count() > 1)
				{
					list.Add(new ExtensionFilter(text.ToUpper(), text.Split(',', StringSplitOptions.None)));
					continue;
				}
				list.Add(new ExtensionFilter(text.ToUpper(), text));
			}
			return list.ToArray();
		}
	}
}
