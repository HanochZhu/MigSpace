namespace JigSpace.Core.FileImport
{
	public class FileImportError
	{
		public string Title;

		public string Message;

		public FileImportErrorType ErrorType;

		public FileImportError(string message, string title = "Oops", FileImportErrorType errorType = FileImportErrorType.DEFAULT)
		{
			Message = message;
			Title = title;
			ErrorType = errorType;
		}
	}
}
