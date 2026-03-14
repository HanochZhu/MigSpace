namespace JigSpace.Core.FileImport
{
	public struct FileImportResult
	{
		public FileImportError Error;

		public string FilePath;

		public byte[] RawData;

		public FileType FileType;

		public bool IsSuccess => Error == null;

		public bool HasError => Error != null;

		public FileImportResult(FileImportError error, string filePath, FileType type, byte[] rawData = null)
		{
			Error = error;
			FilePath = filePath;
			RawData = rawData;
			FileType = type;
		}
	}
}
