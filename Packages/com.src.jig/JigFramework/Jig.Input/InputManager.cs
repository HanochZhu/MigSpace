using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JigSpace;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : JigSingleton<InputManager>
{
	private Vector3 _pointerDownPosition;

	private Vector3 _previousMousePosition;

	private Vector3 _dragOffset;

	private Plane _dragProjectionPlane;

	private GameObject _hitElement;

	private float _holdDownTimer;

	private float _selectionHoldDownTime;

	private bool _isSelectionConfirmed;

	public float _scaleSensibility;

	public float _rotationSensibility;

	public float _panPinchThreshold = 0.7f;

	public int _panPinchConfirmationDelay = 8;

	public float PinchSensitivity;

	public float RotateSensitivity;

	public LayerMask GizmoLayer;

	private Vector3 _rotationAxis;

	public bool CanSelect;

	public bool IsUsingGizmo;

	public bool IsMoving;

	public bool IsScaling;

	public bool IsPanning;

	public bool IsPinching;

	public bool IsRotating;

	public bool IsWaitingForDoubleClick;

	public bool HasDoubleTapped;

	private bool _isTimingForDoubleClick;

	private float _timer;

	private float _timeLimit = 0.2f;

	private Camera _camera;

	private bl_CameraOrbit _cameraOrbit;

	private GameObject _centrelock;

	[SerializeField]
	private float[] _zoomSnapValues = new float[3] { 0.5f, 1f, 2f };

	[SerializeField]
	private float _zoomSnapStrength = 2.8f;

	public bool ClickedOverUI { get; private set; }

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

	public bool IsSnapShortcutPressed
	{
		get
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		}
	}

	public bool IsPrecisionShortcutPressed
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

	public static Vector3 MousePosition => Input.mousePosition;

	private void Start()
	{
		_selectionHoldDownTime = 0.3f;
		_camera = Camera.main;
		if (_camera != null)
		{
			_cameraOrbit = _camera.GetComponent<bl_CameraOrbit>();
		}
		_centrelock = JigSingleton<GameManager>.Instance.CentreLock;
	}

	private void Update()
	{
		if (SingleTouchStarted())
		{
			if (IsPointerOverUIObject())
			{
				ClickedOverUI = true;
			}
		}
		else if (SingleTouchEnded())
		{
			ClickedOverUI = false;
		}
		if (JigSingleton<GameManager>.Instance.IsJigLoaded)
		{
			if (!JigSingleton<GameManager>.Instance.IsAR)
			{
				EditingControls();
			}
			//else if (JigSingleton<ARManager>.Instance.CurrentMode == ARManager.ViewingMode.AR)
			//{
			//	ViewingControls();
			//}
			////else if (JigSingleton<ARManager>.Instance.CurrentMode == ARManager.ViewingMode.Flat)
			//{
			//	FlatControls();
			//}
		}
		if (IsShortcutKeyPressed)
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				if (!JigSingleton<GameManager>.Instance.IsEditingJig)
				{
					return;
				}
				JigSingleton<GameManager>.Instance.CheckBeforeUpdatingFormatVersion(delegate(bool updateFormatVersion)
				{
					//StartCoroutine(JigSingleton<ServerManager>.Instance.SaveJig(false, "", delegate
					//{
					//	GameManager.ShowPostSaveHints();
					//}, false, updateFormatVersion));
				});
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					//JigSingleton<HistoryManager>.Instance.Redo();
					//JigAnalytics.LogRedoButtonClicked();
				}
				else
				{
					//JigSingleton<HistoryManager>.Instance.Undo();
					//JigAnalytics.LogUndoButtonClicked();
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
		else if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
		{
			//if (!UIController.IsFocusingInputfield && JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.isActiveAndEnabled)
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.OnDeleteButtonClicked();
			//}
		}
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
			//if (!UIController.IsFocusingInputfield && JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.isActiveAndEnabled)
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.OnGroupStateButtonClicked();
			//}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//if (!UIController.IsFocusingInputfield)
			//{
			//	if (JigSingleton<GameManager>.Instance.IsAR)
			//	{
			//		JigSingleton<UIController>.Instance.MainCanvasController.ARUIPanelController.OnPreviousButtonClicked();
			//	}
			//	else if (JigSingleton<GameManager>.Instance.IsEditingJig)
			//	{
			//		JigSingleton<UIController>.Instance.MainCanvasController.StageRibbonPanelController.GoToStage(JigSingleton<StageManager>.Instance.CurrentStageIndex - 1);
			//	}
			//}
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			//if (!UIController.IsFocusingInputfield && JigSingleton<SelectionManager>.Instance.IsSomethingSelected)
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.UseTranslateGizmo();
			//}
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			//if (!UIController.IsFocusingInputfield && JigSingleton<SelectionManager>.Instance.IsSomethingSelected)
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.UseRotateGizmo();
			//}
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			//if (!UIController.IsFocusingInputfield && JigSingleton<SelectionManager>.Instance.IsSomethingSelected)
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController.UseScaleGizmo();
			//}
		}
		//else if (Input.GetKeyDown(KeyCode.P) && !UIController.IsFocusingInputfield && Input.GetKey(KeyCode.LeftShift))
		//{
		//	ToolBarController toolBarController = JigSingleton<UIController>.Instance.MainCanvasController.ToolBarController;
		//	if (toolBarController.isActiveAndEnabled)
		//	{
		//		toolBarController.TogglePin();
		//	}
		//}
	}

	public void PencilInteractionDidTap(string data)
	{
		Debug.Log("Received pencil double tap event");
		EventManager.TriggerEvent(Events.PencilInteractionDidTap);
	}

	public void CheckDoubleClick()
	{
		if (SingleTouchStarted())
		{
			if (_isTimingForDoubleClick)
			{
				EventManager.TriggerEvent(Events.DoubleTapped);
				RaycastHit hitInfo;
				if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hitInfo, 500f, JigSingleton<GameManager>.Instance.LayerMask) && hitInfo.collider.gameObject != null && hitInfo.collider.gameObject.CompareTag("element"))
				{
					GameObject gameObject2 = hitInfo.collider.gameObject;
				}
				_timer = 0f;
				_isTimingForDoubleClick = false;
			}
			else
			{
				_isTimingForDoubleClick = true;
			}
		}
		if (_isTimingForDoubleClick)
		{
			if (_timer > _timeLimit)
			{
				_isTimingForDoubleClick = false;
				_timer = 0f;
			}
			else
			{
				_timer += 1f * Time.deltaTime;
			}
		}
	}

	public bool DoubleClicking()
	{
		return !_isTimingForDoubleClick;
	}

	public bool NoTouch()
	{
		if (!Input.GetMouseButton(0))
		{
			return !Input.GetMouseButtonUp(1);
		}
		return false;
	}

	public Vector3 TouchPosition()
	{
		return Input.mousePosition;
	}

	public bool SingleTouchStarted()
	{
		return Input.GetMouseButtonDown(0);
	}

	public bool SingleTouchEnded()
	{
		return Input.GetMouseButtonUp(0);
	}

	public bool SingleTouchMoved()
	{
		Vector3 vector = _previousMousePosition - Input.mousePosition;
		if (Input.GetMouseButton(0))
		{
			return vector.sqrMagnitude > 0.9f;
		}
		return false;
	}

	public bool TouchMoved(int index)
	{
		Vector3 vector = _previousMousePosition - Input.mousePosition;
		if (Input.GetMouseButton(index))
		{
			return vector.sqrMagnitude > 0.9f;
		}
		return false;
	}

	public bool TouchEnded(int index)
	{
		return Input.GetMouseButtonUp(index);
	}

	public bool MultiTouchTouchStarted()
	{
		return false;
	}

	public bool SingleOrMultiTouchTouchStarted()
	{
		return SingleTouchStarted();
	}

	public bool MultiTouchEnded()
	{
		return Input.GetMouseButtonUp(0);
	}

	public bool MultiTouchMoved()
	{
		if (Input.touchCount == 2)
		{
			if (Input.GetTouch(0).phase != TouchPhase.Moved)
			{
				return Input.GetTouch(1).phase == TouchPhase.Moved;
			}
			return true;
		}
		return false;
	}

	public bool SingleTouchTouchStationary()
	{
		if (Input.GetMouseButton(0))
		{
			return (_previousMousePosition - Input.mousePosition).sqrMagnitude <= 0.9f;
		}
		return false;
	}

	public bool MultiTouchTouchStationary()
	{
		return false;
	}

	private void GetPointerDownPosition()
	{
		if (!JigSingleton<GameManager>.Instance.IsJigLoaded)
		{
			return;
		}
		Vector3 mousePosition = Input.mousePosition;
		Ray ray = _camera.ScreenPointToRay(mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 5000f, JigSingleton<GameManager>.Instance.LayerMask))
		{
			if (hitInfo.collider.gameObject != null && hitInfo.collider.gameObject.CompareTag("element"))
			{
				RaycastHit hitInfo2;
				if (Physics.Raycast(ray, out hitInfo2, 5000f, GizmoLayer) && hitInfo2.collider.gameObject != null)
				{
					_hitElement = null;
					return;
				}
				_hitElement = hitInfo.collider.gameObject;
				_isSelectionConfirmed = false;
				EventManager.TriggerEvent(Events.MouseDownOnElement, _hitElement);
				//GameObject innerParent = _hitElement.GetComponent<JigElement>().InnerParent;
				//GetPointerPositionForObject(ray, innerParent);
			}
		}
		else
		{
			_hitElement = null;
		}
	}

	private void GetPointerPositionForObject(Ray ray, GameObject selectedElement)
	{
		if (selectedElement == null)
		{
			Debug.LogError("Inner parent is null");
			return;
		}
		_holdDownTimer = 0f;
		_dragProjectionPlane.SetNormalAndPosition(_camera.transform.forward, selectedElement.transform.position);
		float enter;
		_dragProjectionPlane.Raycast(ray, out enter);
		_pointerDownPosition = ray.GetPoint(enter);
		_dragOffset = selectedElement.transform.position - _pointerDownPosition;
	}

	private void EditingControls()
	{
		if (JigSingleton<GameManager>.Instance.IsJigLoaded)
		{
			CheckDoubleClick();
		}
		if (SingleOrMultiTouchTouchStarted())
		{
			if (!IsPointerOverUIObject())
			{
				_previousMousePosition = Input.mousePosition;
				CanSelect = true;
				GetPointerDownPosition();
			}
		}
		else if ((SingleTouchTouchStationary() || MultiTouchTouchStationary()) && _hitElement != null && CanSelect)
		{
			if (IsPointerOverUIObject())
			{
				return;
			}
			GameObject selectedElement = null;//JigSingleton<SelectionManager>.Instance.SelectedElement;
			GameObject gameObject = null;
			if (_hitElement != null)
			{
				//gameObject = _hitElement.GetComponent<JigElement>().InnerParent;
			}
			bool flag = selectedElement == null || gameObject != selectedElement || _hitElement != selectedElement;
			if (_hitElement != null || flag)
			{
				_holdDownTimer += Time.deltaTime;
				if (_holdDownTimer > _selectionHoldDownTime && !_isSelectionConfirmed)
				{
					_isSelectionConfirmed = true;
					EventManager.TriggerEvent(Events.ElementLongPressed, _hitElement);
				}
			}
		}
		else if (SingleTouchEnded() || MultiTouchEnded())
		{
			if (IsPointerOverUIObject())
			{
				JigSingleton<GameManager>.Instance.SetIsParenting(false);
			}
			if (!JigSingleton<GameManager>.Instance.IsAR)
			{
				_cameraOrbit.Interact = true;
			}
			if (CanSelect && !_isSelectionConfirmed && _hitElement == null)
			{
				JigSingleton<GameManager>.Instance.SetIsParenting(false);
			}
			_pointerDownPosition = Vector3.zero;
			_hitElement = null;
			_holdDownTimer = 0f;
			_isSelectionConfirmed = false;
			CanSelect = false;
			if (IsMoving)
			{
				IsMoving = false;
			}
			if (IsScaling)
			{
				IsScaling = false;
			}
			if (IsRotating)
			{
				IsRotating = false;
			}
		}
		else if (SingleTouchMoved() || MultiTouchMoved())
		{
			CanSelect = false;
		}
		else
		{
			NoTouch();
		}
	}

	public void ViewingControls()
	{
		InputsCore.ProcessClick();
		ClickRotate();
		ScrollToZoom();
		_previousMousePosition = Input.mousePosition;
	}

	private void FlatControls()
	{
		if (SingleTouchStarted())
		{
			if (!IsPointerOverUIObject())
			{
				CanSelect = true;
			}
		}
		else if ((SingleTouchTouchStationary() || MultiTouchTouchStationary()) && CanSelect)
		{
			if (!IsPointerOverUIObject())
			{
				_holdDownTimer += Time.deltaTime;
				if (_holdDownTimer > _selectionHoldDownTime && !_isSelectionConfirmed)
				{
					CanSelect = false;
				}
			}
		}
		else if (SingleTouchEnded() || MultiTouchEnded())
		{
			if (!IsPointerOverUIObject())
			{
				RaycastHit hit;
				if (InputsCore.RayHitsElement(Camera.main.ScreenPointToRay(TouchPosition()), out hit))
				{
				}
				_holdDownTimer = 0f;
				CanSelect = false;
			}
		}
		else if (SingleTouchMoved() || MultiTouchMoved())
		{
			CanSelect = false;
		}
	}

	private void ScrollToZoom()
	{
		float y = Input.mouseScrollDelta.y;
		if (Mathf.Abs(y) > 0f)
		{
			Vector3 vector = new Vector3(y, y, y) * _scaleSensibility;
			Vector3 endValue = _centrelock.transform.localScale + vector;
			if (endValue.x < 0.001f)
			{
				endValue.x = 0.001f;
				endValue.y = 0.001f;
				endValue.z = 0.001f;
			}
			_centrelock.transform.DOScale(endValue, 0.3f);
		}
	}

	private void PinchZoom()
	{
		if (Input.touchCount != 2 || (Input.GetTouch(0).phase != TouchPhase.Moved && Input.GetTouch(1).phase != TouchPhase.Moved) || ClickedOverUI)
		{
			return;
		}
		Touch touch = Input.GetTouch(0);
		Touch touch2 = Input.GetTouch(1);
		Vector2 vector = touch.position - touch.deltaPosition;
		Vector2 vector2 = touch2.position - touch2.deltaPosition;
		float f = touch.position.x - vector.x;
		float f2 = touch.position.y - vector.y;
		float f3 = touch2.position.x - vector2.x;
		float f4 = touch2.position.y - vector2.y;
		if (Mathf.Sign(f) != Mathf.Sign(f3) || Mathf.Sign(f2) != Mathf.Sign(f4))
		{
			float magnitude = (vector - vector2).magnitude;
			float magnitude2 = (touch.position - touch2.position).magnitude;
			float num = (0f - (magnitude - magnitude2)) * PinchSensitivity;
			Vector3 vector3 = new Vector3(num, num, num);
			Vector3 endValue = _centrelock.transform.localScale + vector3;
			if (endValue.x < 0.001f)
			{
				endValue.x = 0.001f;
				endValue.y = 0.001f;
				endValue.z = 0.001f;
			}
			_centrelock.transform.DOScale(endValue, 0.3f);
		}
	}

	private float SnapScaleValue(float originalScale, float targetScale, float clampStrength)
	{
		int num = ((targetScale - originalScale > 0f) ? 1 : (-1));
		for (int i = 0; i < _zoomSnapValues.Length; i++)
		{
			
		}
		return targetScale;
	}

	private void ClickRotate()
	{
		
	}

	private void TouchRotate()
	{
		
	}

	public bool IsPointerOverUIObject()
	{
		Vector2 vector = default(Vector2);
		vector = Input.mousePosition;
		PointerEventData eventData = new PointerEventData(EventSystem.current)
		{
			position = vector
		};
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		for (int i = 0; i < list.Count; i++)
		{
			
		}
		return list.Count > 0;
	}

	private void DragObjects()
	{
		if (!(_hitElement == null) && !IsUsingGizmo)
		{
			if (!_isSelectionConfirmed)
			{
				CanSelect = false;
			}
			else if (!IsScaling && !IsRotating)
			{
				_cameraOrbit.Interact = false;
				IsMoving = true;
				Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
				float enter;
				_dragProjectionPlane.Raycast(ray, out enter);
				Vector3 point = ray.GetPoint(enter);
				//GameObject innerParent = _hitElement.GetComponent<JigElement>().InnerParent;
				//((innerParent == null) ? _hitElement.transform : innerParent.transform).position = point + _dragOffset;
			}
		}
	}

	public bool CtrlShift()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			if (Input.GetKey(KeyCode.RightMeta) || Input.GetKey(KeyCode.LeftMeta))
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					return Input.GetKey(KeyCode.RightShift);
				}
				return true;
			}
			return false;
		}
		if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		}
		return false;
	}

	public bool Ctrl()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			if (!Input.GetKey(KeyCode.RightMeta))
			{
				if (Input.GetKey(KeyCode.LeftMeta))
				{
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						return !Input.GetKey(KeyCode.RightShift);
					}
					return false;
				}
				return false;
			}
			return true;
		}
		if (!Input.GetKey(KeyCode.RightControl))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					return !Input.GetKey(KeyCode.RightShift);
				}
				return false;
			}
			return false;
		}
		return true;
	}

	public bool Shift()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			if (!Input.GetKey(KeyCode.RightMeta) && !Input.GetKey(KeyCode.LeftMeta))
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					return Input.GetKey(KeyCode.RightShift);
				}
				return true;
			}
			return false;
		}
		if (!Input.GetKey(KeyCode.RightControl) && !Input.GetKey(KeyCode.LeftControl))
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		}
		return false;
	}
}
