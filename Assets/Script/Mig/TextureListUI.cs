using Crosstales.FB;
using Mig.Model;
using Mig.Utils;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextureListUI : MainCanvasUIWindowBase
{
    public TextureButton textureButton; // 材质按钮预制体

    public Button LoadTexButton;
    public string[] fileExtensions = { "png", "jpg", "jpeg" }; // 允许的文件扩展名

    protected void Awake()
    {
        LoadTexButton.onClick.AddListener(OpenTextureLoaderWindow);
        textureButton.onClick.AddListener(OpenTextureLoaderWindow);
        textureButton.onCancelClick.AddListener(CancelCurrentTexture);
    }

    private void OnEnable()
    {
        EventManager.StartListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
    }

    private void OnSelectedChanged(object arg0, object arg1)
    {
        GameObject select = arg0 as GameObject;
        if (select == null)
        {
            return;
        }

        var curModelMainTex = ModelManager.Instance.CurrentMaterial.mainTexture as Texture2D;
        if (curModelMainTex != null) 
        {
            LoadTexButton.gameObject.SetActive(false);
            textureButton.gameObject.SetActive(true);
            textureButton.Tex = curModelMainTex;
            textureButton.Image.sprite = curModelMainTex.ConvertToSprite();
        }
        else
        {
            LoadTexButton.gameObject.SetActive(true);
            textureButton.gameObject.SetActive(false);
        }
    }

    private void CancelCurrentTexture()
    {
        this.textureButton.gameObject.SetActive(false);
        this.LoadTexButton.gameObject.SetActive(true);
    }

    private void OpenTextureLoaderWindow()
    {
        // 使用 Crosstales FileBrowser 打开文件选择对话框
        string texPath = FileBrowser.Instance.OpenSingleFile("图片文件 (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif");

        // 确保路径非空且文件夹存在
        if (string.IsNullOrEmpty(texPath))
        {
            Debug.Log("Invalid folder path or folder does not exist.");
            return;
        }
        //LoadTexturesFromFolder(texPath);
        var tex = LoadTextureFromPath(texPath);
        textureButton.Tex = tex;
        textureButton.Image.sprite = tex.ConvertToSprite();

        textureButton.gameObject.SetActive(true);
        LoadTexButton.gameObject.SetActive(false);

        EventManager.TriggerEvent(MigEventCommon.OnChangeSelectModelTexture, tex);
    }
    public override void OnShow()
    {
        base.OnShow();
        //// 激活 PaintScrollView
        //ScrollContent.gameObject.SetActive(true);

        //// 清除现有的按钮
        //foreach (Transform child in ScrollContent)
        //{
        //    Destroy(child.gameObject);
        //}

        //// 根据材质数组生成按钮
        //for (int i = 0; i < textures.Length; i++)
        //{
        //    // 实例化按钮并设置父物体为 ScrollContent
        //    GameObject button = Instantiate(textureButtonPrefab, ScrollContent);

        //    button.GetComponent<Image>().sprite = images[i];

        //    // 获取 TextureButton 组件，并设置对应的材质
        //    TextureButton textureButton = button.GetComponent<TextureButton>();
        //    textureButton.Tex = textures[i];

        //    // 添加按钮点击事件
        //    int index = i;
        //    button.GetComponent<Button>().onClick.AddListener(() => OnTextureButtonClick(index));
        //}
    }

    public Texture2D LoadTextureFromPath(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {

        }

        return texture;
    }

    // 加载文件夹中的纹理
    //public void LoadTexturesFromFolder(string folderPath)
    //{
    //    Debug.Log("Loading textures from folder.");
    //    if (!Directory.Exists(folderPath))
    //    {
    //        Debug.LogWarning($"Folder {folderPath} does not exist.");
    //        return;
    //    }

    //    string[] files = Directory.GetFiles(folderPath, "*.*")
    //        .Where(file => fileExtensions.Contains(Path.GetExtension(file).ToLower().TrimStart('.')))
    //        .ToArray();

    //    Debug.Log($"Found {files.Length} files in the folder.");

    //    textures = new Texture2D[files.Length];
    //    images = new Sprite[files.Length];

    //    for (int i = 0; i < files.Length; i++)
    //    {
    //        Debug.Log($"Loading file: {files[i]}");
    //        byte[] fileData = File.ReadAllBytes(files[i]);
    //        Texture2D texture = new Texture2D(2, 2);
    //        if (texture.LoadImage(fileData))
    //        {
    //            textures[i] = texture;
    //            images[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //            Debug.Log($"Loaded texture: {textures[i].name}");
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Failed to load texture: {files[i]}");
    //        }
    //    }

    //    Debug.Log("Textures loaded and buttons refreshed.");
    //    OnShow();
    //}
}
