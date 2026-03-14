using DG.Tweening;
using Mig.Model;
using Mig.Model.ModelLoader;
using Mig.Snapshot;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mig.UI.MainCavas
{
    public class MainCanvas : MonoBehaviour
    {
        public Button homeButton;
        public Button _loadModelButton;
        public Button saveButton;
        public Button PlayButton;
        public Button objectListButton;
        public Button cameraButton;
        public Button revertButton;
        public Button nextButton;

        public RectTransform ObjectList;

        #region ObjectList
        public float ObjectListTargetX;
        private float motoObjectListTargetX;
        private bool isObjectListClick;
        #endregion

        private void Awake()
        {
            motoObjectListTargetX = ObjectList.anchoredPosition.x;

        }
        private void Start()
        {
            homeButton.onClick.AddListener(OnHomeButtonClick);

            // It is better to use event.
            _loadModelButton.onClick.AddListener(OnLoadFbxFromFile);

            saveButton.onClick.AddListener(OnSaveCurrentModel);

            objectListButton.onClick.AddListener(OnObjectListButtonClick);

            cameraButton.onClick.AddListener(OnCameraButtonClick);

            revertButton.onClick.AddListener(OnRevertButtonClick);
        }

        private void OnRevertButtonClick()
        {
            OperatorCommandManager.Instance.Undo();
        }

        private void OnCameraButtonClick()
        {
            
        }

        private void OnHomeButtonClick()
        {
            LoadHomeScene();
        }

        public void SetEnterPresentationTrigger(Action OnEnterPresentationTrigger)
        {
            PlayButton.onClick.RemoveAllListeners();
            PlayButton.onClick.AddListener(() => OnEnterPresentationTrigger());
        }

        private void OnSaveCurrentModel()
        {
            SnapshotManager.Instance.UpdateCurrentSnapShot();
            ModelManager.Instance.SaveProjectToWeb(new ProjectSerializer(), new ProjectSaveToWeb(), ProjectManager.CurrentProjectName);

        }

        private void OnLoadFbxFromFile()
        {
            ModelManager.Instance.LoadFormFilePickerAsync(new ModelFilePickAndLoad());
        }

        public void OnObjectListButtonClick()
        {
            ObjectList.DOAnchorPosX(isObjectListClick ? motoObjectListTargetX : ObjectListTargetX, 0.3f)
                              .SetEase(Ease.Linear);
            isObjectListClick = !isObjectListClick;
        }

        public void LoadHomeScene()
        {
            StartCoroutine(LoadSceneAsync("ProjectView"));
        }

        IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                if (progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
