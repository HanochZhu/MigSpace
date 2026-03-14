using DG.Tweening;
using Mig;
using Mig.Model;
using Mig.UI.ModelCanvas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomPaintWindow : MonoBehaviour
{
    private enum PaintWindowState
    {
        COLOR,
        MAT,
        TEXTURE
    }
    public Button _colorButton;
    public Button _materialButton;
    public Button _textureButton;
    public Image _backImage;
    //public Transform PaintScrollView;
    public float moveDuration;
    private PaintWindowState paintWindowState = PaintWindowState.COLOR;

    public MaterialListUI materialListUI;
    public TextureListUI textureListUI;
    public ColorGradientPicker colorPickerWindow;

    // Start is called before the first frame update
    void Start()
    {
        _colorButton.onClick.AddListener(OnColorButtonClick);
        _materialButton.onClick.AddListener(OnMaterialButtonClick);
        _textureButton.onClick.AddListener(OnTextureButtonClick);

        UpdateWindow();
    }

    public void OnColorButtonClick() 
    {
        paintWindowState = PaintWindowState.COLOR;
        MoveBackButtonImage(_colorButton.GetComponent<RectTransform>().anchoredPosition.x);
        UpdateWindow();
    }
    public void OnMaterialButtonClick()
    {
        paintWindowState = PaintWindowState.MAT;
        MoveBackButtonImage(_materialButton.GetComponent<RectTransform>().anchoredPosition.x);
        UpdateWindow();
    }

    public void OnTextureButtonClick()
    {
        paintWindowState = PaintWindowState.TEXTURE;

        MoveBackButtonImage(_textureButton.GetComponent<RectTransform>().anchoredPosition.x);
        UpdateWindow();
    }

    public void MoveBackButtonImage(float targetPos) 
    {
        _backImage.rectTransform.DOAnchorPosX(targetPos, moveDuration);
    }

    public void ClickPaintScroll(bool isOpenColor)
    {
        if (ModelManager.Instance.CurrentSelectGameObject == null)
        {
            Debug.Log("No model loaded!");
            return;
        }

        if (!isOpenColor)
        {
            HideAll();
            return;
        }

        paintWindowState = PaintWindowState.COLOR;

        UpdateWindow();
    }

    private void HideAll()
    {
        this.gameObject.SetActive(false);
    }

    private void UpdateWindow()
    {
        textureListUI.SetActive(paintWindowState == PaintWindowState.TEXTURE);
        materialListUI.SetActive(paintWindowState == PaintWindowState.MAT);
        colorPickerWindow.SetActive(paintWindowState == PaintWindowState.COLOR);
    }
}
