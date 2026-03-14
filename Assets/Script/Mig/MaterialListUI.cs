using Mig;
using Mig.Core;
using Mig.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MaterialListUI : MainCanvasUIWindowBase
{
    public MaterialButton materialButtonPrefab; // 材质按钮预制体

    public Transform ScrollContent;

    //public InputField colorInputField; // 输入颜色的 InputField
    //public Slider transparencySlider; // 控制透明度的 Slider
    //public InputField transparencyInputField; // 控制透明度的 InputField

    private MigMaterial currentMaterial; // 当前选中的材质

    private void OnEnable()
    {
        EventManager.StartListening(Events.OnColorImagePointerUp, OnColorImagePointerUp);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.OnColorImagePointerUp, OnColorImagePointerUp);
    }
    private void OnColorImagePointerUp(object arg0, object arg1)
    {
        GetMaterial();
        Color color = (Color)arg0;
        Debug.Log (color);
        string colorString = ColorUtility.ToHtmlStringRGB(color); // 将颜色转换成#RRGGBB格式
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    // 激活 PaintScrollView 并生成材质按钮
    public override void OnShow()
    {
        base.OnShow();
        foreach (Transform child in ScrollContent)
        {
            Destroy(child.gameObject);
        }

        var items = MigMaterialLibrary.Instance.migMaterialItems;
        // 根据材质数组生成按钮
        for (int i = 0; i < items.Count; i++)
        {
            // 实例化按钮并设置父物体为 ScrollContent
            MaterialButton button = Instantiate(materialButtonPrefab, ScrollContent);
            button.MigMaterialGUID = items[i].Guid;
            // 设置按钮的文本为材质名称
            button.Text.text = items[i].Name;
            button.Image.sprite = items[i].Icon;
        }
    }

    // 当输入颜色值结束时
    private void OnColorInputChanged(string input)
    {
        GetMaterial();
        if (currentMaterial == null) return;

        Color newColor;
        if (ColorUtility.TryParseHtmlString("#" + input, out newColor))
        {
            Debug.Log("newColor" + newColor);
            // 如果成功解析颜色，将其应用于当前材质
            currentMaterial.mainColor = newColor;
            EventManager.TriggerEvent(Events.OnColorImagePointerUp, currentMaterial.mainColor);
        }
        else
        {
            Debug.Log("Invalid color format. Please use #RRGGBB or named colors.");
        }
    }

    public void GetMaterial() 
    {
        if (ModelManager.Instance.CurrentMaterial != null)
            currentMaterial = ModelManager.Instance.CurrentMaterial;
    }
}
