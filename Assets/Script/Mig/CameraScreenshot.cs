using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraScreenshot : MonoBehaviour
{
    public Camera screenshotCamera;
    public Image targetImage;

    void Update()
    {
        // 检测是否按下了截图的按键，这里假设按下了空格键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 获取屏幕的宽高
            int width = Screen.width;
            int height = Screen.height;

            // 创建一个Texture2D来存储截图
            Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

            // 设置相机的渲染目标为截图Texture
            screenshotCamera.targetTexture = RenderTexture.GetTemporary(width, height, 24);

            // 渲染相机，截取当前画面
            screenshotCamera.Render();

            // 读取渲染的结果到Texture2D中
            RenderTexture.active = screenshotCamera.targetTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshotTexture.Apply();

            // 释放渲染目标
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(screenshotCamera.targetTexture);
            screenshotCamera.targetTexture = null;

            // 将截图赋给Image组件的Sprite
            targetImage.sprite = Sprite.Create(screenshotTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

}
