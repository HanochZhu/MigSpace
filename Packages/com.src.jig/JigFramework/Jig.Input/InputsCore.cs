using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JigSpace
{
	public static class InputsCore
	{
		private static float _stationaryThreshold = 3f;

		private static float _maxClickDuration = 0.3f;

		private static float _maxTapDuration = 0.3f;

		private static float _minLongPressdDuration = 1f;

		private static float _touchStationaryTolerenceSqr = 0.5f;

		private static float _mouseStationaryTolerenceMagn = 0.03f;

		private static bool _startCounting;

		private static float _touchTimer;

		private static Vector3 _startClickPosition;

		public static bool IsPointerOverUIElement(List<GameObject> excludedObjects = null)
		{
			Vector2 position = default(Vector2);
			if (Input.touchCount > 0)
			{
				position = Input.GetTouch(0).position;
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = position;
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (excludedObjects.Contains(list[i].gameObject))
					{
						list.RemoveAt(i);
						i--;
					}
				}
			}
			return list.Count > 0;
		}

		public static void ProcessTouch()
		{
			if (Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase.Equals(TouchPhase.Began))
				{
					if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
					{
						return;
					}
					_startCounting = true;
					_touchTimer = 0f;
				}
				if (!TouchStationary(touch) && _startCounting)
				{
					_touchTimer += Time.deltaTime;
					if (_touchTimer > _minLongPressdDuration)
					{
						RaycastHit hitInfo = default(RaycastHit);
						if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hitInfo, 5000f, JigSingleton<GameManager>.Instance.LayerMask) && hitInfo.collider.gameObject.tag == "element")
						{
							_startCounting = false;
						}
					}
				}
				if (TouchMoved(touch))
				{
					_startCounting = false;
				}
				if (touch.phase.Equals(TouchPhase.Ended) && _startCounting)
				{
					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					RaycastHit hit;
					if (_touchTimer < _maxTapDuration && _touchTimer >= 0f && RayHitsElement(ray, out hit))
					{
					}
					_startCounting = false;
				}
			}
			else if (Input.touchCount > 1)
			{
				_startCounting = false;
			}
		}

		public static void ProcessClick()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (EventSystem.current.IsPointerOverGameObject())
				{
					return;
				}
				_startCounting = true;
				_touchTimer = 0f;
				_startClickPosition = Input.mousePosition;
			}
			if (Input.GetMouseButton(0))
			{
				if (EventSystem.current.IsPointerOverGameObject())
				{
					return;
				}
				if (!MouseMoved() && _startCounting)
				{
					_touchTimer += Time.deltaTime;
					RaycastHit hit;
					if (_touchTimer > _minLongPressdDuration && RayHitsElement(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
					{
						_startCounting = false;
					}
				}
				if (MouseMoved())
				{
					_startCounting = false;
				}
			}
			if (Input.GetMouseButtonUp(0) && _startCounting && !EventSystem.current.IsPointerOverGameObject())
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit2;
				if (_touchTimer < _maxClickDuration && _touchTimer > 0f && RayHitsElement(ray, out hit2))
				{
				}
				_startCounting = false;
			}
		}

		public static bool IsMouseMoving()
		{
			return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > _mouseStationaryTolerenceMagn;
		}

		public static bool MouseMoved()
		{
			return (Input.mousePosition - _startClickPosition).magnitude > _stationaryThreshold;
		}

		public static bool TouchMoved(Touch touch)
		{
			if (touch.phase.Equals(TouchPhase.Moved))
			{
				return touch.deltaPosition.sqrMagnitude > _touchStationaryTolerenceSqr;
			}
			return false;
		}

		public static bool TouchStationary(Touch touch)
		{
			if (!touch.phase.Equals(TouchPhase.Stationary))
			{
				if (touch.phase.Equals(TouchPhase.Moved))
				{
					return touch.deltaPosition.sqrMagnitude <= _touchStationaryTolerenceSqr;
				}
				return false;
			}
			return true;
		}

		public static bool RayHitsElement(Ray ray, out RaycastHit hit)
		{
			if (Physics.Raycast(ray, out hit, 5000f, JigSingleton<GameManager>.Instance.JigElementLayerMask))
			{
				return hit.collider.gameObject.CompareTag("element");
			}
			return false;
		}

		public static bool CtrlShift()
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

		public static bool Ctrl()
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

		public static bool Shift()
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

		public static bool Alt()
		{
			if (!Input.GetKey(KeyCode.LeftAlt))
			{
				return Input.GetKey(KeyCode.RightAlt);
			}
			return true;
		}
	}
}
