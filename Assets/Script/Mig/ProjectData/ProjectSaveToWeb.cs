using System;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text;
using Mig.Core;
using Mig.Snapshot;


namespace Mig
{
    public class ProjectSaveToWeb : IModelSaver
    {
        public Func<string> SnapShotSerializedFunc;
        private ModelOperateState m_SaveState;

        private Action<bool> onSaveCompleteCallback;
        
        public string ErrorMsg()
        {
            return "";
        }

        public float GetPercentage()
        {
            return FTPClient.GetUpLoadPercentage();
        }

        public ModelOperateState GetState()
        {
            return m_SaveState;
        }

        public void OnDispose()
        {

        }

        public async void Save(string pathORAddress, GameObject modelParent, Action<bool> onSaveComplete)
        {

        }

        public async void Save(string pathORAddress, ISerializer serializer, Action<bool> onSaveComplete)
        {
            onSaveCompleteCallback = onSaveComplete;

            var saveBytes = await serializer.Serialize();

            var image = SnapshotManager.Instance.GetProjectSnapshotImage();

            EventManager.TriggerEvent(MigEventCommon.OnSaveModelBegin, "Save Sucsess");

            await FTPClient.UploadBytes(Path.Combine(pathORAddress, serializer.GetName() + ".png"), image.EncodeToPNG(), async (isSuccess) =>
            {
                if (!isSuccess)
                {
                    OnUploadCallback(false);
                    return;
                }
                using (FileStream localFileStream = new FileStream(PathManager.GetDefaultZipFilePath(), FileMode.Open, FileAccess.Read))
                {
                    await FTPClient.UploadStream(Path.Combine(pathORAddress, serializer.GetName() + ".mig"), localFileStream, OnUploadCallback);
                }
            });
        }

        private void OnUploadCallback(bool result)
        {
            Debug.Log("[Mig] OnUploadCallback has send all file to server");
            EventManager.TriggerEvent(MigEventCommon.OnSaveModelEnd, "Save Sucsess");
            onSaveCompleteCallback?.Invoke(result);
        }
    }
}
