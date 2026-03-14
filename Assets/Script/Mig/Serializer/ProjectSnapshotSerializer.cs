using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Mig
{
    public class ProjectSnapshotSerializer : ISerializer
    {

        public static string DefaultProjectSnapshotName = "0.png";
        public string GetName()
        { 
            return ProjectManager.CurrentProjectName;
        }

        public async Task<bool> Serialize()
        {
            var texPath = Path.Combine(PathManager.GetAccountTempSnapshotTexFolder(), DefaultProjectSnapshotName);

            if (!Directory.Exists(PathManager.GetAccountTempSnapshotTexFolder()) ||
                !File.Exists(texPath))
            {
                return false;
            }

            return true;
        }
    }
}
