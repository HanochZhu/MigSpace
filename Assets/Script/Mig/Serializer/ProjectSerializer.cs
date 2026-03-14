using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Unity.SharpZipLib;
using Unity.SharpZipLib.Utils;
using Mig.Snapshot;
using Mig.Model;
using Mig.Model.Utils;

namespace Mig
{
    public class ProjectSerializer : ISerializer
    {
        public string GetName()
        {
            return ProjectManager.CurrentProjectName;
        }

        public async Task<bool> Serialize()
        {
            EventManager.TriggerEvent(MigEventCommon.OnSaveModelBegin, "start save project");

            if (Directory.Exists(PathManager.GetAccountTempFolder()))
            {
                Directory.Delete(PathManager.GetAccountTempFolder(), true);
            }
            // create a temp folder to store project in local
            Directory.CreateDirectory(PathManager.GetAccountTempFolder());
            Directory.CreateDirectory(PathManager.GetAccountTempModelFolder());
            Directory.CreateDirectory(PathManager.GetAccountTempSnapshotTexFolder());
            Directory.CreateDirectory(PathManager.GetAccountTempSnapshotElementFolder());

            if (ModelManager.Instance.CurrentGameObjectRoot == null)
            {
                Debug.Log("[Mig::ProjectSerializer] Failed to save. You need load a model from web or file");
                EventManager.TriggerEvent(MigEventCommon.OnSaveModelEnd,"");
                return false;
            }
            var modelSaveDir = PathManager.GetAccountTempModelFolder();
            var success = await ModelSaveUtils.SaveModelAsGLBTo(ModelManager.Instance.CurrentGameObjectRoot, modelSaveDir);

            if (!success)
            {
                Debug.Log($"[Mig::ProjectSerializer] Failed to save model to glb at temp folder {ModelManager.Instance.CurrentGameObjectRoot}");
                EventManager.TriggerEvent(MigEventCommon.OnSaveModelEnd, "");

                return false;
            }
            var snapshots = SnapshotManager.Instance.GetCurrentAllSnapShot();

            success = await SaveSnapshotTexToFile(snapshots, PathManager.GetAccountTempSnapshotTexFolder());

            if (!success)
            {
                EventManager.TriggerEvent(MigEventCommon.OnSaveModelEnd, "");
                return false;
            }

            var json = ProjectData.SerializedCurrentProject();

            File.WriteAllText(PathManager.GetDefaultAccountTempSnapshotElementFolder(), json);

            var desZipPath = PathManager.GetDefaultZipFilePath();

            var zipFolder = PathManager.GetAccountTempFolder();

            success = await Task.Factory.StartNew(() =>
            {
                ZipUtility.CompressFolderToZip(desZipPath, null, zipFolder);

                return true;
            });

            Debug.Log($"[Mig::ProjectSerializer] Zip success at {desZipPath}");

            return true;
        }

        private async Task<bool> SaveSnapshotTexToFile(List<SnapShotData> snapshots, string savePath)
        {
            try
            {
                foreach (var snapShot in snapshots)
                {
                    var imageByte = snapShot.Image.EncodeToPNG();

                    var imagePath = Path.Combine(savePath, snapShot.StepCount + ".png");

                    await File.WriteAllBytesAsync(imagePath, imageByte);
                }
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }

}
