using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MainCanvasUIWindowBase : MonoBehaviour
{
    public GameObject UIPanel;

    public virtual void SetActive(bool isActive)
    {
        if (isActive)
        {
            OnShow();
        }
        else
        {
            OnHide();
        }
    }

    public virtual void OnShow()
    {
        UIPanel.gameObject.SetActive(true);
    }

    public virtual void OnHide()
    {
        UIPanel.gameObject.SetActive(false);
    }
}
