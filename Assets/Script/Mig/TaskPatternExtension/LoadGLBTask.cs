using Mig.Core.TaskPattern;
using Mig.Model;
using Mig.Model.ModelLoader;
using System;
using System.IO;
using UnityEngine;

namespace Mig
{
    public class LoadGLBTask : TaskHandlerBase
    {
        private string glbDir;

        public LoadGLBTask(string glbDir, Action<bool> taskCallback) : base(taskCallback)
        {
            this.glbDir = glbDir;
        }

        public override void Execute()
        {
            EventManager.TriggerEvent(MigEventCommon.OnLoadingModelBegin, "Loading");
            ModelManager.Instance.OnModelLoadCompleteEvent += onModelLoaded;

            if (!Directory.Exists(PathManager.GetAccountTempModelFolder()))
            {
                Debug.Log($"Path:{PathManager.GetAccountTempModelFolder()} is not aviliable");
                m_taskCallback?.Invoke(false);
                return;
            }

            var glbFiles = Directory.GetFiles(glbDir);

            Debug.Log($"[Mig] loading glb from {glbFiles[0]}");

            if (glbFiles.Length == 0)
            {
                Debug.LogError("[Mig] Failed to get glb path from {this.glbDir}");
                m_taskCallback?.Invoke(false);
                return;
            }
            ModelManager.Instance.LoadGLBFromFileAsync(glbFiles[0], new GlbFileLoader());
        }

        private void onModelLoaded()
        {
            ModelManager.Instance.OnModelLoadCompleteEvent -= onModelLoaded;
            base.Execute();
        }
    }

}

