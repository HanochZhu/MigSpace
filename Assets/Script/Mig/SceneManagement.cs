using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // 新建场景
    public void NewScene()
    {
        // 通过加载空场景来创建新场景
        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    // 加载场景
    public void LoadScene(string sceneName)
    {
        // 根据场景名称加载场景
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    // 保存场景
    public void SaveScene()
    {
        // 获取当前场景的索引
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 保存场景
        //SceneManager.SaveScene(SceneManager.GetSceneAt(sceneIndex));
    }
}
