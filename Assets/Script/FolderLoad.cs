using Crosstales.FB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderLoad : Singelton<FolderLoad>
{
    private string[] extensions = { "Image", "txt", "jpg", "pdf", "png" };


    public string OpenSingleFile()
    {
        string path = FileBrowser.Instance.OpenSingleFile("Open file", "", "", extensions);
        //string path = FileBrowser.Instance.OpenSingleFile("txt");
        //string path = FileBrowser.Instance.OpenSingleFile();
        Debug.Log($"OpenSingleFile: '{path}'", this);
        return path;
    }
}
