using Mig.Core;
using Mig.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TMPro;
using TriLibCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mig.ProjectView.UI
{
    public class ProjectViewController : MonoBehaviour
    {
        public GameObject ProjectItemPrefab;

        [Header("New Project Window")]
        public GameObject NewProjectWindow;

        public TMP_InputField inputField;

        public Button AddNew;

        public Button CreateNewProject;

        public Button CloseNewProject;
        private void OnEnable()
        {
            Init();
        }

        private List<GameObject> spawnedProjectUIList = new();

        private CancellationTokenSource loadImageTaskToken = new CancellationTokenSource();

        public void Start()
        {
            ProjectItemPrefab.gameObject.SetActive(false);
            NewProjectWindow.SetActive(false);

            inputField.onEndEdit.AddListener(onInputFieldEndEdit);
            AddNew.onClick.AddListener(OnAddNewProject);
            CreateNewProject.onClick.AddListener(OnCreateNewProject);
            CloseNewProject.onClick.AddListener(OnCloseNewProjectWindow);
        }

        private void OnCloseNewProjectWindow()
        {
            NewProjectWindow.SetActive(false);

        }

        private void OnCreateNewProject()
        {
            NewProjectWindow.SetActive(true);
        }

        private void onInputFieldEndEdit(string content)
        {
            AddNew.enabled = !string.IsNullOrEmpty(content);
        }

        private void OnAddNewProject()
        {
            ProjectManager.CurrentProjectName = inputField.text;

            SceneManager.LoadScene("MainScene");
        }

        private void Init()
        {
            var content = ProjectItemPrefab.transform.parent;

            foreach (GameObject item in spawnedProjectUIList)
            {
                if (item.gameObject == ProjectItemPrefab)
                {
                    continue;
                }
                Destroy(item.gameObject);
            }

            var webProjectList = FTPClient.GetFTPDirList(PathManager.GetCurrentFTPDirRoot());

            if (webProjectList.Count == 0)
            {
                NewProjectWindow.SetActive(true);
                return;
            }

            foreach (var projectName in webProjectList)
            {
                var newItem = Instantiate(ProjectItemPrefab, ProjectItemPrefab.transform.parent, false);

                newItem.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(projectName);

                newItem.GetComponent<Button>().onClick.AddListener(()=> {
                    OnProjectSelect(newItem);
                    }
                );

                newItem.gameObject.SetActive(true);

                spawnedProjectUIList.Add(newItem);
            }
            GetProjectSnapshotFromWeb(webProjectList, loadImageTaskToken.Token);
        }

        private async void GetProjectSnapshotFromWeb(List<string> projectList, CancellationToken token)
        {
            for (int i = 0; i < projectList.Count; i ++)
            {
                var item = projectList[i];
                if (token.IsCancellationRequested)
                {
                    return;
                }
                var address = Path.Combine(PathManager.GetCurrentFTPDirRoot(), item, item + ".png");
                var imageBytes = await FTPClient.DownloadToBytesAsync(address, token);
                if (token.IsCancellationRequested)
                {
                    return;
                }
                if (imageBytes.Length == 1)
                {
                    continue;
                }
                var tex = new Texture2D(520, 480);
                tex.LoadImage(imageBytes);
                spawnedProjectUIList[i].GetComponent<Image>().sprite = tex.ConvertToSprite();
            }
        }

        private void OnProjectSelect(GameObject itemGo)
        {
            this.loadImageTaskToken.Cancel();

            ProjectManager.CurrentProjectName = itemGo.GetComponentInChildren<TextMeshProUGUI>().text;

            SceneManager.LoadScene("MainScene");
        }

        private void OnDestroy()
        {
            this.loadImageTaskToken.Cancel();
        }
    }
}

