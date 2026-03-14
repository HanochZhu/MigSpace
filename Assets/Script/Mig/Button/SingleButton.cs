using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class SingleButton : Button
{
    public bool IsButtonPressed
    {
        get
        {
            return isPressed;
        }
    }
    private bool isPressed;
    private Image buttonImage;
    private Sprite originalSprite;  // 保存初始的Sprite
    private Color originalColor;    // 保存初始的颜色

    public Action OnSingleButtonDeselect;

    protected override void Awake()
    {
        base.Awake();
        Inition();
    }

    public void Inition()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalSprite = buttonImage.sprite;  // 获取并保存初始的Sprite
            originalColor = buttonImage.color;    // 获取并保存初始的颜色
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        SetButtonPressState(true);
    }


    public void SetButtonPressState(bool isPress)
    {
        buttonImage.sprite = isPress ? spriteState.pressedSprite : originalSprite;
        Color targetColor = isPress ? colors.pressedColor : originalColor;
        // 使用 DOTween 进行颜色平滑过渡
        buttonImage.DOColor(targetColor, colors.fadeDuration);

        isPressed = isPress;

        if (!isPressed)
        {
            OnSingleButtonDeselect?.Invoke();
        }
    }
}