using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Mig.Model;

namespace Mig
{
    public class ColorGradientPicker : MainCanvasUIWindowBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public Image gradientImage; // 显示颜色渐变的 Image

        private Texture2D gradientTexture; // 渐变纹理
        private bool isDragging = false;
        private bool isPointUp = false;
        private Color m_selectedColor;
        private Color m_originalColor;
        public Slider hueSlider;
        public Slider alphaSlider;

        private void Awake()
        {
            hueSlider.onValueChanged.AddListener(OnHueSliderValueChanged);
            alphaSlider.onValueChanged.AddListener(OnAlphaSliderValueChanged);
        }

        void Start()
        {
            int size = 256;
            gradientTexture = new Texture2D(size, size);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float hue = hueSlider.value; // 获取滑块的色相值
                    Color color = Color.HSVToRGB(hue, 1 - (float)x / size, 1 - (float)y / size); // 从右上到左下的渐变
                    gradientTexture.SetPixel(x, y, color);
                }
            }

            gradientTexture.Apply();
            gradientImage.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, size, size), Vector2.one * 0.5f);


            Texture2D hueTexture = new Texture2D(size, 1);
            for (int x = 0; x < size; x++)
            {
                float hue = Mathf.Lerp(0, 1, (float)x / size); // 色相值从左到右变化
                Color color = Color.HSVToRGB(hue, 1, 1); // 获取色相值对应的颜色
                hueTexture.SetPixel(x, 0, color);
            }
            hueTexture.Apply();
            Sprite hueSprite = Sprite.Create(hueTexture, new Rect(0, 0, size, 1), new Vector2(0.5f, 0.5f));

            // 设置滑动条的背景图片为渐变色的纹理
            hueSlider.transform.GetChild(0).GetComponent<Image>().sprite = hueSprite;
        }

        void OnHueSliderValueChanged(float value)
        {
            int size = 256;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Color color = Color.HSVToRGB(value, 1 - (float)x / size, 1 - (float)y / size);
                    gradientTexture.SetPixel(x, y, color);
                }
            }
            gradientTexture.Apply();
            gradientImage.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, size, size), Vector2.one * 0.5f);

            // 设置滑动条的滑块颜色为当前色相值的颜色
            Color handleColor = Color.HSVToRGB(value, 1, 1);
            hueSlider.handleRect.GetComponent<Image>().color = handleColor;
        }

        public void OnAlphaSliderValueChanged(float value)
        {
            if (ModelManager.Instance.CurrentSelectGameObject == null)
            {
                return;
            }

            var changeTransparencyOpt = OperatorCommandFactory.CreateOperatorTransparencyChangeCommand(ModelManager.Instance.CurrentMaterial, 1 - value);

            OperatorCommandManager.Instance.Execute(changeTransparencyOpt);

            EventManager.TriggerEvent(MigEventCommon.OnModelPropertiesChange);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!ModelManager.Instance.CurrentSelectGameObject)
                return;
            isDragging = true;
            m_originalColor = ModelManager.Instance.CurrentMaterial.mainColor;
            GetColorFromPointer(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointUp = true;

            if (ModelManager.Instance.CurrentMaterial != null)
            {
                ModelManager.Instance.CurrentMaterial.mainColor = m_originalColor;

                var changeColorOpt = OperatorCommandFactory.CreateColorChangeCommand(ModelManager.Instance.CurrentMaterial, m_selectedColor);

                OperatorCommandManager.Instance.Execute(changeColorOpt);

                EventManager.TriggerEvent(Events.OnColorImagePointerUp, m_selectedColor);
            }

            isDragging = false;
            isPointUp = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                GetColorFromPointer(eventData);
            }
        }

        private void GetColorFromPointer(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                gradientImage.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
            {
                // 转换为纹理的坐标
                float normalizedX = (localPoint.x + gradientImage.rectTransform.rect.width / 2) / gradientImage.rectTransform.rect.width;
                float normalizedY = (localPoint.y + gradientImage.rectTransform.rect.height / 2) / gradientImage.rectTransform.rect.height;

                int x = Mathf.Clamp((int)(normalizedX * gradientTexture.width), 0, gradientTexture.width - 1);
                int y = Mathf.Clamp((int)(normalizedY * gradientTexture.height), 0, gradientTexture.height - 1);

                Color selectedColor = gradientTexture.GetPixel(x, y);

                // 将颜色应用于材质
                if (ModelManager.Instance.CurrentMaterial != null)
                {
                    ModelManager.Instance.CurrentMaterial.mainColor = selectedColor;
                    m_selectedColor = selectedColor;
                }
            }
            EventManager.TriggerEvent(MigEventCommon.OnModelPropertiesChange);
        }
    }
}