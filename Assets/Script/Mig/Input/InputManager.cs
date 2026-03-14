using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mig
{
    public class InputManager : MonoBehaviour
    {
        public bl_CameraOrbit cameraOrbit;

        public MouseEventHandle mouseEvent;
        private bool _isTimingForDoubleClick;
        private bool _isGizmosDrag;
        private void OnEnable()
        {
            EventManager.StartListening(Events.OnGizmosDragBegin, OnGizmosDragBegin);
            EventManager.StartListening(Events.OnGizmosDragEnd, OnGizmosDragEnd);

            mouseEvent.OnMouseLeftClick += ClickInScene;
        }

        private void OnDisable()
        {
            mouseEvent.OnMouseLeftClick -= ClickInScene;

            EventManager.StopListening(Events.OnGizmosDragBegin, OnGizmosDragBegin);
            EventManager.StopListening(Events.OnGizmosDragEnd, OnGizmosDragEnd);
        }

        private void OnGizmosDragBegin(object arg0, object arg1)
        {
            _isGizmosDrag = true;
        }

        private void OnGizmosDragEnd(object arg0, object arg1)
        {
            _isGizmosDrag = false;
        }

        public bool IsShortcutKeyPressed
        {
            get
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    return Input.GetKey(KeyCode.RightControl);
                }
                return true;
            }
        }

        public void Update()
        {
            if (_isGizmosDrag)
                return;

            ClickMouseInput();
            KeyboardInput();
        }

        private void KeyboardInput()
        {
            if (IsShortcutKeyPressed)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {

                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        Debug.Log("Redo Input");

                    }
                    else
                    {
                        Debug.Log("Undo Input");
                        OperatorCommandManager.Instance.Undo();
                    }
                }
                else
                {
                    if (!Input.GetKeyDown(KeyCode.G) /*|| JigSingleton<SelectionManager>.Instance.SelectedElements == null || !JigSingleton<SelectionManager>.Instance.SelectedElements.Any()*/)
                    {
                        return;
                    }
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        //JigSingleton<GroupManager>.Instance.RemoveGroup(JigSingleton<SelectionManager>.Instance.SelectedElement.JigElement().Group);
                        return;
                    }
                    //JigSingleton<GroupManager>.Instance.CreateGroup(JigSingleton<SelectionManager>.Instance.SelectedElements.Select((GameObject x) => x.JigElement()).ToList());
                }
            }
#if UNITY_EDITOR
            else
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    OperatorCommandManager.Instance.Undo();
                }
            }
#endif
        }
        private float firstClickTime;
        private float timeBetweenClicks = 0.2f;
        private bool coroutineAllowed;
        /// <summary>
        /// DoubleClickEvent
        /// </summary>
        private void ClickMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!coroutineAllowed)
                {
                    if (Time.time - firstClickTime < timeBetweenClicks)
                    {
                        StartCoroutine(DoubleClickEvent());
                        coroutineAllowed = true;
                    }
                    else
                    {
                        firstClickTime = Time.time;
                    }
                }
            }
        }


        private IEnumerator DoubleClickEvent()
        {
            cameraOrbit.OnDoubleClick();

            yield return new WaitForSeconds(0.2f);

            coroutineAllowed = false;
        }

        private void ClickInScene()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 5000f, 1 << MigLayerManager.interactable))
            {
                if (hitInfo.collider.gameObject == null)
                {
                    EventManager.TriggerEvent(MigEventCommon.OnClickModel, null);
                    return;
                }
                GameObject obj = hitInfo.collider.gameObject;

                EventManager.TriggerEvent(MigEventCommon.OnClickModel, obj);
            }
            else
            {
                EventManager.TriggerEvent(MigEventCommon.OnClickModel, null);
            }
        }
    }

}

