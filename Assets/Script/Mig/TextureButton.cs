using Mig;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;


public class TextureButton : MonoBehaviour
{
    [HideInInspector]
    public Texture2D Tex;

    public Image Image;

    public Button CancelButton;

    public ButtonClickedEvent onClick
    {
        get
        {
            return this.GetComponent<Button>().onClick;
        }
    }

    public ButtonClickedEvent onCancelClick
    {
        get
        {
            return CancelButton.onClick;
        }
    }
}
