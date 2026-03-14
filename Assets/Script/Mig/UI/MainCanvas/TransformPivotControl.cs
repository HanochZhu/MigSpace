using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformPivotControl : MonoBehaviour
{
    public Button ObjectGroupCenter;
    public Button ObjectCenterPivot;
    public Transform PivotList;

    protected void Awake()
    {
        ObjectGroupCenter.onClick.AddListener(() => { EventManager.TriggerEvent(MigEventCommon.ChangePivotToGroupGizmoMode, true); });
        ObjectCenterPivot.onClick.AddListener(() => { EventManager.TriggerEvent(MigEventCommon.ChangePivotToGroupGizmoMode, false); });
    }

    private bool _isActive; 
    public void SetPivotListActive() 
    {
        _isActive = !_isActive;
        PivotList.gameObject.SetActive(_isActive);
    }
}
