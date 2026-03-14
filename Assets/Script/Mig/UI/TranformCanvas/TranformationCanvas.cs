using Mig.Model;
using DG.Tweening;
using Mig;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Mig.UI.ModelCanvas;
using System;
using UnityEditor;
using Mig.Core;

namespace Mig.UI.TranformationCanvas
{
    public class TranformationCanvas : MonoBehaviour
    {
        private Transform targetTransform;
        public Transform materialCanvas;
        public RectTransform imageRectTransform;
        public float motoPosition;
        public float targetPosition;

        public InputField positionXInputField;
        public InputField positionYInputField;
        public InputField positionZInputField;
        public InputField rotationXInputField;
        public InputField rotationYInputField;
        public InputField rotationZInputField;
        public InputField scaleXInputField;
        public InputField scaleYInputField;
        public InputField scaleZInputField;
        public Slider _transparencySlider;
        public InputField _transparencyInputField;

        public Button _moveButton;
        public Button _exitButton;
        public Button _colorButton;
        public InputField _colorInputField;

        public InputField metallicInputField;
        public InputField smoothnessInputField;
        public Slider metallicSlider;
        public Slider smoothnessSlider;

        public InputField tilingXInputField;
        public InputField tilingYInputField;

        public InputField offsetXInputField;
        public InputField offsetYInputField;

        public ToolkitCanvas ToolkitCanvas;

        public CustomPaintWindow customPaintWindow;

        private bool isShowCustomColorWindow = false;

        private MigMaterial curMaterial 
        { 
            get
            {
                return ModelManager.Instance.CurrentMaterial;
            } 
        }

        protected void Awake()
        {
            motoPosition = imageRectTransform.position.x;
        }

        private void OnEnable()
        {
            JigSingleton<ModelManager>.Instance.OnModelLoadCompleteEvent += OnModelLoad;
            EventManager.StartListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
            EventManager.StartListening(Events.OnInputHitModel, OnInputHitModel);
            EventManager.StartListening(MigEventCommon.OnModelPropertiesChange, OnModelPropertiesChange);
        }


        private void OnDisable()
        {
            if (JigSingleton<ModelManager>.Instance)
            {
                JigSingleton<ModelManager>.Instance.OnModelLoadCompleteEvent -= OnModelLoad;
            }
            EventManager.StopListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
            EventManager.StopListening(Events.OnInputHitModel, OnInputHitModel);
            EventManager.StopListening(MigEventCommon.OnModelPropertiesChange, OnModelPropertiesChange);

        }

        private void OnModelPropertiesChange(object arg0, object arg1)
        {
            this.UpdateUIValue();
        }

        private void OnInputHitModel(object arg0, object arg1)
        {
            this.transform.GetChild(0).gameObject.SetActive((bool)arg0);
        }

        private void OnSelectedChanged(object arg0, object arg1)
        {
            if (arg0 == null)
            {
                Debug.LogError("deselect model should hide plane");
                return;
            }
            GameObject obj = (GameObject)arg0;

            targetTransform = obj.transform;

            Debug.Log("targetTransform.localRotation.x" + targetTransform.eulerAngles.x);

            if (obj.GetComponent<MeshRenderer>() != null)
            {
                materialCanvas.gameObject.SetActive(true);
            }
            else
            {
                materialCanvas.gameObject.SetActive(false);
            }
            UpdateUIValue();
        }

        public void OnModelLoad()
        {
            targetTransform = ModelManager.Instance.CurrentGameObjectRoot.transform;

            if (curMaterial != null)
            {
                metallicSlider.value = curMaterial.Metallic;
                metallicInputField.text = curMaterial.Metallic.ToString("F2");
                smoothnessSlider.value = curMaterial.Smoothness;
                smoothnessInputField.text = curMaterial.Smoothness.ToString("F2");


                tilingXInputField.text = curMaterial.mainTextureScale.x.ToString("F2");
                tilingYInputField.text = curMaterial.mainTextureScale.y.ToString("F2");

                offsetXInputField.text = curMaterial.mainTextureOffset.x.ToString("F2");
                offsetYInputField.text = curMaterial.mainTextureOffset.y.ToString("F2");

            }
        }
        void Start()
        {
            positionXInputField.onValueChanged.AddListener(delegate { UpdatePositionX(); });
            positionYInputField.onValueChanged.AddListener(delegate { UpdatePositionY(); });
            positionZInputField.onValueChanged.AddListener(delegate { UpdatePositionZ(); });

            rotationXInputField.onValueChanged.AddListener(delegate { UpdateRotationX(); });
            rotationYInputField.onValueChanged.AddListener(delegate { UpdateRotationY(); });
            rotationZInputField.onValueChanged.AddListener(delegate { UpdateRotationZ(); });

            scaleXInputField.onValueChanged.AddListener(delegate { UpdateScaleX(); });
            scaleYInputField.onValueChanged.AddListener(delegate { UpdateScaleY(); });
            scaleZInputField.onValueChanged.AddListener(delegate { UpdateScaleZ(); });

            tilingXInputField.onValueChanged.AddListener(delegate { UpdateTilingX(); });
            tilingYInputField.onValueChanged.AddListener(delegate { UpdateTilingY(); });

            offsetXInputField.onValueChanged.AddListener(delegate { UpdateOffset(); });
            offsetYInputField.onValueChanged.AddListener(delegate { UpdateOffset(); });

            _transparencyInputField.onValueChanged.AddListener(delegate { UpdateTransparencyFromInput(); });

            metallicInputField.onValueChanged.AddListener(UpdateMetallicFromInput);
            metallicSlider.onValueChanged.AddListener(UpdateMetallicFromSlider);
            smoothnessInputField.onValueChanged.AddListener(UpdateSmoothnessFromInput);
            smoothnessSlider.onValueChanged.AddListener(UpdateSmoothnessFromSlider);
            _transparencySlider.onValueChanged.AddListener(UpdateTransparencyFromSlider);

            _moveButton.onClick.AddListener(() => OnMoveButtonClick(true));
            _exitButton.onClick.AddListener(() => OnMoveButtonClick(false));
            _colorButton.onClick.AddListener(() => {
                isShowCustomColorWindow = !isShowCustomColorWindow;
                customPaintWindow.ClickPaintScroll(isShowCustomColorWindow);
            });
        }

        private void OnMoveButtonClick(bool isOpen)
        {
            if (isOpen)
            {
                MoveImageToPosition(targetPosition);

            }
            else
            {
                MoveImageToPosition(motoPosition);
            }
        }

        // InputField编辑结束时更新Slider和材质透明度
        private void UpdateTransparencyFromInput()
        {
            if (curMaterial != null &&  float.TryParse(_transparencyInputField.text, out float value))
            {
                var changeTransparencyOpt = OperatorCommandFactory.CreateOperatorTransparencyChangeCommand(ModelManager.Instance.CurrentMaterial, value);
                OperatorCommandManager.Instance.Execute(changeTransparencyOpt);

                _transparencySlider.value = value;
            }
        }

        // Slider值改变时更新材质透明度和InputField
        private void UpdateTransparencyFromSlider(float value)
        {
            if (curMaterial != null)
            {
                // 更新材质透明度
                Color color = curMaterial.mainColor;
                color.a = value;
                curMaterial.mainColor = color;

                _transparencyInputField.text = value.ToString("F2");
                UpdateUIValue();
            }
        }

        private void UpdateMetallicFromInput(string value)
        {
            if (curMaterial != null && float.TryParse(value, out float metallic))
            {
                var changemetallicOpt = OperatorCommandFactory.CreateOperatorMetallicChangeCommand(ModelManager.Instance.CurrentMaterial, metallic);
                OperatorCommandManager.Instance.Execute(changemetallicOpt);

                metallicSlider.value = metallic;
            }
        }

        private void UpdateMetallicFromSlider(float value)
        {
            if (curMaterial != null)
            {
                curMaterial.Metallic = value;
                metallicInputField.text = value.ToString("F2");
                UpdateUIValue();
            }
        }

        private void UpdateSmoothnessFromInput(string value)
        {
            if (curMaterial != null && float.TryParse(value, out float smoothness))
            {
                var changeSmoothnessOpt = OperatorCommandFactory.CreateOperatorSmoothnessChangeCommand(ModelManager.Instance.CurrentMaterial, smoothness);
                OperatorCommandManager.Instance.Execute(changeSmoothnessOpt);

                smoothnessSlider.value = smoothness;
            }
        }

        private void UpdateSmoothnessFromSlider(float value)
        {
            if (curMaterial != null)
            {
                curMaterial.Smoothness = value;
                smoothnessInputField.text = value.ToString("F2");
                UpdateUIValue();
            }
        }

        private void UpdateTilingX()
        {
            if (curMaterial == null) return;

            float tilingX = 1f;

            if (float.TryParse(tilingXInputField.text, out tilingX))
            {
                var changeTilingXOpt = OperatorCommandFactory.CreateOperatorTilingChangeCommand(ModelManager.Instance.CurrentMaterial, new Vector2(tilingX, ModelManager.Instance.CurrentMaterial.mainTextureScale.y));
                OperatorCommandManager.Instance.Execute(changeTilingXOpt);
            }
            else
            {
                Debug.LogWarning("Invalid Tiling input!");
            }
        }

        private void UpdateTilingY()
        {
            if (curMaterial == null) return;

            float tilingY = 1f;

            if (float.TryParse(tilingYInputField.text, out tilingY))
            {
                var curMat = ModelManager.Instance.CurrentMaterial;
                var changeTilingYOpt = OperatorCommandFactory.CreateOperatorTilingChangeCommand(curMat, 
                    new Vector2(curMat.mainTextureScale.x, 
                    tilingY)
                    );
                OperatorCommandManager.Instance.Execute(changeTilingYOpt);
            }
            else
            {
                Debug.LogWarning("Invalid Tiling input!");
            }
        }

        private void UpdateOffset()
        {
            if (curMaterial == null) return;

            float offsetX = 0f, offsetY = 0f;

            if (float.TryParse(offsetXInputField.text, out offsetX) &&
                float.TryParse(offsetYInputField.text, out offsetY))
            {
                var changeOffsetOpt = OperatorCommandFactory.CreateOperatorOffsetChangeCommand(ModelManager.Instance.CurrentMaterial, new Vector2(offsetX, offsetY));
                OperatorCommandManager.Instance.Execute(changeOffsetOpt);
                UpdateUIValue();
            }
            else
            {
                Debug.LogWarning("Invalid Offset input!");
            }
        }

        public void MoveImageToPosition(float targetX, float duration = 0.3f, Ease ease = Ease.Linear)
        {
            imageRectTransform.DOAnchorPosX(targetX, duration)
                              .SetEase(ease);
            UpdateUIValue();
        }
        #region Tranform
        void UpdatePositionX()
        {
            if (targetTransform != null)
            {
                float x;
                if (float.TryParse(positionXInputField.text, out x))
                {
                    targetTransform.position = new Vector3(x, targetTransform.position.y, targetTransform.position.z);
                }
                else
                {
                    Debug.LogWarning("Invalid position input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }

        void UpdatePositionY()
        {
            if (targetTransform != null)
            {
                float y;
                if (float.TryParse(positionYInputField.text, out y))
                {
                    targetTransform.position = new Vector3(targetTransform.position.x, y, targetTransform.position.z);
                }
                else
                {
                    Debug.LogWarning("Invalid position input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }
        void UpdatePositionZ()
        {
            if (targetTransform != null)
            {
                float z;
                if (float.TryParse(positionZInputField.text, out z))
                {
                    targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, z);
                }
                else
                {
                    Debug.LogWarning("Invalid position input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }
        void UpdateRotationX()
        {
            if (targetTransform != null)
            {
                float x;
                if (float.TryParse(rotationXInputField.text, out x))
                {
                    targetTransform.rotation = Quaternion.Euler(x, targetTransform.eulerAngles.y, targetTransform.eulerAngles.z);
                }
                else
                {
                    Debug.LogWarning("Invalid rotation input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }
        void UpdateRotationY()
        {
            if (targetTransform != null)
            {
                float y;
                if (float.TryParse(rotationYInputField.text, out y))
                {
                    targetTransform.rotation = Quaternion.Euler(targetTransform.eulerAngles.x, y, targetTransform.eulerAngles.z);
                }
                else
                {
                    Debug.LogWarning("Invalid rotation input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }
        void UpdateRotationZ()
        {
            if (targetTransform != null)
            {
                float z;
                if (float.TryParse(rotationZInputField.text, out z))
                {
                    targetTransform.rotation = Quaternion.Euler(targetTransform.eulerAngles.x, targetTransform.eulerAngles.y, z);
                }
                else
                {
                    Debug.LogWarning("Invalid rotation input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }
        void UpdateScaleX()
        {
            if (targetTransform != null)
            {
                float x;
                if (float.TryParse(scaleXInputField.text, out x))
                {
                    targetTransform.localScale = new Vector3(x, targetTransform.localScale.y, targetTransform.localScale.z);
                }
                else
                {
                    Debug.LogWarning("Invalid scale input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }

        void UpdateScaleY()
        {
            if (targetTransform != null)
            {
                float y;
                if (float.TryParse(scaleXInputField.text, out y))
                {
                    targetTransform.localScale = new Vector3(targetTransform.localScale.x, y, targetTransform.localScale.z);
                }
                else
                {
                    Debug.LogWarning("Invalid scale input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }

        void UpdateScaleZ()
        {
            if (targetTransform != null)
            {
                float z;
                if (float.TryParse(scaleXInputField.text, out z))
                {
                    targetTransform.localScale = new Vector3(targetTransform.localScale.x, targetTransform.localScale.y, z);
                }
                else
                {
                    Debug.LogWarning("Invalid scale input!");
                }
            }
            EventManager.TriggerEvent(MigEventCommon.UpdateGizmo);
        }
        #endregion

        // Update is called once per frame
        void LateUpdate()
        {
            if (ModelManager.Instance.CurrentSelectGameObject && targetTransform != null)
            {
                var curModel = ModelManager.Instance.CurrentSelectGameObject;
                positionXInputField.text = curModel.transform.position.x.ToString("F2");
                positionYInputField.text = curModel.transform.position.y.ToString("F2");
                positionZInputField.text = curModel.transform.position.z.ToString("F2");

                rotationXInputField.text = targetTransform.eulerAngles.x.ToString("F2");
                rotationYInputField.text = targetTransform.eulerAngles.y.ToString("F2");
                rotationZInputField.text = targetTransform.eulerAngles.z.ToString("F2");

                scaleXInputField.text = curModel.transform.localScale.x.ToString("F2");
                scaleYInputField.text = curModel.transform.localScale.y.ToString("F2");
                scaleZInputField.text = curModel.transform.localScale.z.ToString("F2");
            }
        }

        public void UpdateUIValue()
        {
            if (targetTransform != null && ModelManager.Instance.CurrentMaterial != null)
            {
                var material = ModelManager.Instance.CurrentMaterial;   
                _transparencyInputField.text = material.mainColor.a.ToString("F2");
                metallicInputField.text = material.Metallic.ToString("F2");
                //smoothnessInputField.text = ModelManager.Instance.CurrentMaterial.GetFloat("_Glossiness").ToString("F2");
                tilingXInputField.text = material.mainTextureScale.x.ToString("F2");
                tilingYInputField.text = material.mainTextureScale.y.ToString("F2");
                offsetXInputField.text = material.mainTextureOffset.x.ToString("F2");
                offsetYInputField.text = material.mainTextureOffset.y.ToString("F2");
            }
        }
    }
}