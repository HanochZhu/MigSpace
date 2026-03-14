namespace JigSpace.Core.FileImport
{
	public class FileImportErrorTenantStorage : FileImportError
	{
		public int UsedStorageMB;

		public int TotalStorageMB;

		public int SelectedFileMB;

		public FileImportErrorTenantStorage(string message, string title, int usedStorageMB, int totalStorageMB, int selectedFileMB)
			: base(message, title, FileImportErrorType.TENANT_STORAGE)
		{
			UsedStorageMB = usedStorageMB;
			TotalStorageMB = totalStorageMB;
			SelectedFileMB = selectedFileMB;
		}
	}
}
