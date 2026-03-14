using Mig.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mig.UI.ModelCanvas
{
    public class ToolkitCanvas : MonoBehaviour
    {
        private SingleButton _currentButton;

        public SingleButton _moveButton;
        public SingleButton _rotateButton;
        public SingleButton _scaleButton;
        public SingleButton _deleteButton;
        public SingleButton _annotateButton;
        public SingleButton _paintButton;


        public CustomPaintWindow customPainterWindow;

        protected void Awake()
        {
            EventManager.StartListening(MigEventCommon.OnGizmoStateChanged, OnGizmoStateChanged);


            _moveButton.onClick.AddListener(onMoveGizmoButtonClick);
            _rotateButton.onClick.AddListener(onRotateGizmoButtonClick);
            _scaleButton.onClick.AddListener(onScaleGizmoButtonClick);
            _deleteButton.onClick.AddListener(onDeleteButtonClick);
            _annotateButton.onClick.AddListener(onAddAnnotationClick);
            _paintButton.onClick.AddListener(onClickPaint);
            _paintButton.OnSingleButtonDeselect += OnDeselectPaintButton;

            customPainterWindow.gameObject.SetActive(false);
        }

        private void OnGizmoStateChanged(object arg0, object arg1)
        {
            GizmoState GizmoState = (GizmoState)arg0;

            if (_currentButton)
            {
                _currentButton.SetButtonPressState(false);
            }

            switch(GizmoState)
            {
                case GizmoState.Move:
                    _currentButton = _moveButton; 
                    break;
                case GizmoState.Rotate:
                _currentButton = _rotateButton; 
                    break;
                case GizmoState.Scale:
                _currentButton = _scaleButton; 
                    break;
                default:
                    _currentButton = null;
                    return;
            }
            _currentButton.SetButtonPressState(true);
        }

        private void OnDeselectPaintButton()
        {
            EventManager.TriggerEvent(MigEventCommon.OnPaintModel, _paintButton.IsButtonPressed);
            customPainterWindow.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            EventManager.StopListening(MigEventCommon.OnGizmoStateChanged, OnGizmoStateChanged);

            _moveButton.onClick.RemoveAllListeners();
            _rotateButton.onClick.RemoveAllListeners();
            _scaleButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
            _annotateButton.onClick.RemoveAllListeners();
            _paintButton.onClick.RemoveAllListeners();
            _paintButton.OnSingleButtonDeselect -= OnDeselectPaintButton;

        }

        private void onClickPaint()
        {
            customPainterWindow.gameObject.SetActive(true);
            SwepButtonState(_paintButton);
            EventManager.TriggerEvent(MigEventCommon.OnPaintModel, _paintButton.IsButtonPressed);
        }

        private void onAddAnnotationClick()
        {
            SwepButtonState(_annotateButton);
            EventManager.TriggerEvent(MigEventCommon.AnnotateModel);
        }

        private void onDeleteButtonClick()
        {
            SwepButtonState(_deleteButton);
            EventManager.TriggerEvent(MigEventCommon.OnDeleteModelUIClick);
        }

        private void onMoveGizmoButtonClick()
        {
            SwepButtonState(_moveButton);
            EventManager.TriggerEvent(MigEventCommon.SetCurrentGizmoState, GizmoState.Move);
        }

        private void onScaleGizmoButtonClick()
        {
            SwepButtonState(_scaleButton);
            EventManager.TriggerEvent(MigEventCommon.SetCurrentGizmoState, GizmoState.Scale);
        }

        private void onRotateGizmoButtonClick()
        {
            SwepButtonState(_rotateButton);
            EventManager.TriggerEvent(MigEventCommon.SetCurrentGizmoState, GizmoState.Rotate);
        }

        public void SwepButtonState(SingleButton curButton)
        {
            if (_currentButton && _currentButton == curButton)
            {
                _currentButton.SetButtonPressState(!_currentButton.IsButtonPressed);
                _currentButton = null;
                return;
            }
            _currentButton?.SetButtonPressState(false);
            _currentButton = curButton;
            _currentButton.SetButtonPressState(true);
        }
    }
}