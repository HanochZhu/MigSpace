using Mig;
using Mig.Model;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class MaterialButton : MonoBehaviour
{
    public Guid MigMaterialGUID;

    public Image Image;

    public Text Text;

    public Button Button;
    // 更改模型材质
    

    public ButtonClickedEvent onClick
    {
        get { return Button.onClick; }
    }

    private void Awake()
    {
        onClick.AddListener(ChangeMaterial);
    }

    public void ChangeMaterial()
    {
        // 获取加载的模型
        GameObject selectModel = ModelManager.Instance.CurrentSelectGameObject;

        if (selectModel == null)
        {
            EventManager.TriggerEvent(MigEventCommon.OnPrompt, "Select a gameObject");
            return;
        }

        // 获取模型上的所有 Renderer 组件
        Renderer[] renderers = selectModel.GetComponentsInChildren<Renderer>();

        // 遍历模型的所有 Renderer，为每个 Renderer 设置新的材质
        foreach (Renderer renderer in renderers)
        {
            var changeMatOpt = OperatorCommandFactory.CreateOperatorMaterialChangeCommand(renderer, MigMaterialGUID);

            OperatorCommandManager.Instance.Execute(changeMatOpt);
        }
        EventManager.TriggerEvent(Events.OnColorImagePointerUp, Color.blue);

        Debug.Log("Model material changed");
    }
}
