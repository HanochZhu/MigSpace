using Crosstales.FB;
using Mig;
using Mig.Model;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorPropertiesControl : MainCanvasUIWindowBase
{
    public Button textureButton; // 需要赋予点击事件的按钮
    public Button colorButton;
    public string[] fileExtensions = { "png", "jpg", "jpeg" }; // 允许的文件扩展名
    private void OnEnable()
    {
        EventManager.StartListening(Events.OnColorImagePointerUp, OnColorImagePointerUp);   
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.OnColorImagePointerUp, OnColorImagePointerUp);
    }

    private void Start()
    {
        textureButton.onClick.AddListener(LoadTexture);
    }

    private void OnColorImagePointerUp(object arg0, object arg1)
    {
        colorButton.image.color = (Color)arg0;
    }
    private void LoadTexture()
    {
        // 使用 Crosstales FileBrowser 打开文件选择对话框
        string filePath = FileBrowser.Instance.OpenSingleFile("Select Texture", "", "Open", fileExtensions);

        // 确保路径非空且文件存在
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            // 读取文件内容并转换为 Texture2D
            Texture2D loadedTexture = new Texture2D(2, 2);
            byte[] fileData = File.ReadAllBytes(filePath);
            loadedTexture.LoadImage(fileData); // 从文件加载图像数据

            // 获取加载的模型
            GameObject loadedModel = ModelManager.Instance.CurrentSelectGameObject.gameObject;

            // 获取模型上的所有 Renderer 组件
            Renderer[] renderers = loadedModel.GetComponentsInChildren<Renderer>();


            // 为渲染器的材质分配纹理
            if (ModelManager.Instance.CurrentMaterial != null)
            {
                var changeMatOpt = OperatorCommandFactory.CreatOperatorMainTextureChangeCommand(renderers[0], loadedTexture);

                OperatorCommandManager.Instance.Execute(changeMatOpt);
            }
            else
            {
                Debug.Log("Object Renderer is not assigned.");
            }
        }
        else
        {
            Debug.Log("Invalid file path or file does not exist.");
        }
    }
}
