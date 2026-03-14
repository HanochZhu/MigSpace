using Mig.Core;
using Mig.Core.TaskPattern;
using Mig.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Timeline;

namespace Mig
{
    /// <summary>
    /// might use in multi-state
    /// </summary>
    public class MigLoadingProjectController : IMigStateController
    {
        private Action<bool> onLoadCompleteCallback;
        private UIController uiController;
        public MigLoadingProjectController(UIController _uiController, Action<bool> onModelLoadComplete)
        {
            uiController = _uiController;
            onLoadCompleteCallback = onModelLoadComplete;
        }

        public void Awake()
        {
            if (string.IsNullOrEmpty(ProjectManager.CurrentProjectName))
            {
                onLoadCompleteCallback?.Invoke(true);
                return;
            }
            if(!Directory.Exists(PathManager.GetCachedRootFolder()))
            {
                Directory.CreateDirectory(PathManager.GetCachedRootFolder());
            }
            var address = Path.Combine(FTPClient.GetCurrentFTPDirRoot(), ProjectManager.CurrentProjectName, ProjectManager.CurrentProjectFileNameWithExtension);
            var saveZipPath = Path.Combine(PathManager.GetCachedRootFolder(), ProjectManager.CurrentProjectName + ".mig");
            var extractPath = PathManager.GetAccountTempFolder();
            var glbDir = PathManager.GetAccountTempModelFolder();

            DeserializeProjectTask deserializeProjectTask = new(onLoadCompleted);
            LoadGLBTask loadGLBTask = new(glbDir, onLoadCompleted);
            UnzipProjectFileTask unzipTask = new(saveZipPath, extractPath, onLoadCompleted);
            LoadingFromFTPTask loadingTask = new(address, saveZipPath,onLoadCompleted);

            loadingTask.NextTask = unzipTask;
            unzipTask.NextTask = loadGLBTask;
            loadGLBTask.NextTask = deserializeProjectTask;

            loadingTask.Execute();
            //ProjectManager.Instance.LoadProjectFileFromWeb(address, onLoadCompleted);
        }

        public void Sleep()
        {
        }

        public void Update()
        {
        }

        public void UpdateUI()
        {
        }

        private void onLoadCompleted(bool isSuccess)
        {
            EventManager.TriggerEvent(MigEventCommon.OnLoadingModelEnd, "load glb complete");
            if (isSuccess)
            {
                onLoadCompleteCallback?.Invoke(true);

                Debug.Log("Task complete");
                return;
            }

            Debug.LogError("[Mig] Failed to loading model from web");
            Debug.Assert(onLoadCompleteCallback != null, "[Mig] callback needs to assign");
            onLoadCompleteCallback?.Invoke(false);
        }
    }
}
