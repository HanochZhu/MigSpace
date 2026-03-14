using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCavasControl : MonoBehaviour
{
    public string HomeSceneName = "ProjectView";
    public bl_CameraOrbit CameraOrbit;
    private void OnEnable()
    {
        EventManager.StartListening(Events.OnLoadHomeScene, LoadHomeScene);
        EventManager.StartListening(Events.OnCameraViewClick, OnCameraViewClick);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.OnLoadHomeScene, LoadHomeScene);
        EventManager.StopListening(Events.OnCameraViewClick, OnCameraViewClick);
    }

    private void LoadHomeScene(object arg0, object arg1)
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
    private int side = 0;
    public void OnCameraViewClick(object arg0, object arg1)
    {
        CameraOrbit.SetViewPoint(side);
        if (side == 4)
            side = 0;
        else
            side++;
    }
}
