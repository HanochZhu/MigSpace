using System.IO;
using UnityEngine;

public class FileWatcher
{
	public delegate void FileChanged(FileSystemEventArgs e);

	public delegate void FileRenamed(RenamedEventArgs e);

	private string _fileToWatch = "*.*";

	private FileSystemWatcher _watcher;

	private NotifyFilters _filters;

	private FileChanged _fnFileChanged;

	private FileRenamed _fnFileRenamed;

	private bool _watchSubDir;

	public void Init(string filePathToWatch, NotifyFilters filters, FileChanged fnFileChanged, FileRenamed fnFileRenamed, bool watchSubDir)
	{
		Debug.Log("FileWatcher - Attempting to watch file: " + filePathToWatch);
		_fileToWatch = filePathToWatch;
		_filters = filters;
		_fnFileChanged = fnFileChanged;
		_fnFileRenamed = fnFileRenamed;
		_watchSubDir = watchSubDir;
		_watcher = new FileSystemWatcher();
		string fileName = Path.GetFileName(_fileToWatch);
		string path = _fileToWatch.Replace(fileName, "");
		_watcher.Path = path;
		_watcher.NotifyFilter = _filters;
		_watcher.Filter = fileName;
		if (_fnFileRenamed != null)
		{
			_watcher.Renamed += OnRenamed;
		}
		if (_fnFileChanged != null)
		{
			_watcher.Changed += OnChanged;
			_watcher.Created += OnChanged;
			_watcher.Deleted += OnChanged;
		}
		if (File.Exists(_fileToWatch))
		{
			FileAttributes attributes = File.GetAttributes(_fileToWatch);
			if (attributes.HasFlag(FileAttributes.Directory))
			{
				_watcher.IncludeSubdirectories = _watchSubDir;
			}
		}
		_watcher.EnableRaisingEvents = true;
	}

	private void OnChanged(object source, FileSystemEventArgs e)
	{
		_fnFileChanged(e);
	}

	private void OnRenamed(object source, RenamedEventArgs e)
	{
		_fnFileRenamed(e);
	}
}
