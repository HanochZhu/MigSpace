using Crosstales.FB;
using Mig.Model;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class NormalMapLoad : MonoBehaviour
{
    public Button textureButton; // 需要赋予点击事件的按钮
    public string[] fileExtensions = { "png", "jpg", "jpeg" }; // 允许的文件扩展名

    private void Start()
    {
        textureButton.onClick.AddListener(LoadTexture);
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

            // 为渲染器的材质分配纹理
            if (ModelManager.Instance.CurrentMaterial != null)
            {
                ModelManager.Instance.CurrentMaterial.NormalMap = loadedTexture; // 设置材质的主纹理
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
