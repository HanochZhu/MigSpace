using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 用于检测鼠标事件
/// </summary>
public class MouseEventHandle : MonoBehaviour
{

    #region Public Field

    public Camera RayCamera;

    public static MouseEventHandle Instance;

    public UnityAction OnMouseEnterUI;
    public UnityAction OnMouseExitUI;
    public UnityAction OnMouseLeftClick;
    public UnityAction OnMouseRightClick;
    public UnityAction OnShowContext;
    public UnityAction OnTouchClick;

    public int SelectAbleLayer = 1 << 9;

    #endregion

    #region Private
    #region TouchParameters

    private readonly float m_showContextThreshold = 0.5f;

    private readonly float m_rightClickThreshold = 0.2f;

    private float m_rightClickTime = 0f;
    private float m_leftClickTime = 0f;
    
    private float m_singleTouchTime = 0f;

    private bool m_isStartTouch = false;

    private Vector2 m_touchStartPos;

    private Vector3 m_MouseStartPos;

    private float m_touchClickThreshold = 0.5f;
    
    #endregion
    #endregion


    #region Unity Event

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // mouse over ui
            UpdateMouseEventInUI();
        }   
        else if (Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            UpdateMouseEventInUI();
            return;
        }
        else
        {
            // mouse over 3d 
            UpdateMouseEventInScene();
            UpdateInputEventInScene();
        }
    }
    #endregion

    #region User Event

    #region Mouse Event

    private void UpdateMouseEventInUI()
    {

    }

    private void UpdateMouseEventInScene()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_leftClickTime += Time.deltaTime;
            m_MouseStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            var endMousePos = Vector3.Distance(m_MouseStartPos, Input.mousePosition);
            if (m_leftClickTime < m_rightClickThreshold && endMousePos <= 0.005)
            {
                OnMouseLeftClick?.Invoke();
            }
            m_leftClickTime = 0f;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            m_rightClickTime += Time.deltaTime;
            m_MouseStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            var endMousePos = Vector3.Distance(m_MouseStartPos, Input.mousePosition);
            if (m_rightClickTime < m_rightClickThreshold && endMousePos <= 0.005)
            {
                OnMouseRightClick?.Invoke();
                OnShowContext?.Invoke();
            }
            
            m_rightClickTime = 0f;
        }
    }

    #endregion

    #region Touch Event
    private void UpdateInputEventInScene()
    {
        if (Input.touchCount == 1)
        {
            var singleTouch = Input.GetTouch(0);

            if (singleTouch.phase == TouchPhase.Began)
            {
                this.m_isStartTouch = true;
            }

            if (singleTouch.phase == TouchPhase.Moved)
            {
                this.m_isStartTouch = false;
            }
            
            if (singleTouch.phase == TouchPhase.Ended && m_isStartTouch)// 离开
            {
                if (m_singleTouchTime < m_rightClickThreshold)
                {
                    // touch
                    OnTouchClick?.Invoke();
                    // ProcessSingleTouchClick(singleTouch);
                }
                else if(m_singleTouchTime > m_showContextThreshold)
                {
                    OnShowContext?.Invoke();
                }
                this.m_isStartTouch = false;
            }

            if (singleTouch.phase == TouchPhase.Stationary)
            {
                m_singleTouchTime += Time.deltaTime;
            }
            
        }
        else if (Input.touchCount == 2)// tow fingers
        {
            this.m_isStartTouch = false;
        }
        else if(Input.touchCount == 0)
        {
            this.m_isStartTouch = false;
        }
    }

    // private void ProcessSingleTouchClick(Touch touch)
    // {
    //     //
    //     var ray = RayCamera.ScreenPointToRay(touch.position);
    //
    //     if (Physics.Raycast(ray,100,SelectAbleLayer))
    //     {
    //         
    //     }
    //     else
    //     {
    //         OnShowContext
    //     }
    // }

    private void ProcessSingleTouchContext()
    {
        
    }
    #endregion
    #endregion
    
}
