using Mig.Model;
using Mig.UI.ModelCanvas;
using RTG;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GizmoState { Move, Rotate, Scale, NULL }

public class TranslateGizmosManager : MonoBehaviour
{
    public GameObject annotationPrefab;
    public Transform annotationPrefabParent;
    public ToolkitCanvas toolkitCanvas;

    public float moveSpeed = 1f;
    public float rotationSpeed = 50f;
    public float scaleSpeed = 1f;


    public GizmoState activeFunction = GizmoState.NULL;

    private ObjectTransformGizmo _moveGizmo;
    private ObjectTransformGizmo _rotationGizmo;
    private ObjectTransformGizmo _scaleGizmo;
    private GameObject selectedGameobject;

    private void OnEnable()
    {
        EventManager.StartListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);

        EventManager.StartListening(MigEventCommon.ChangePivotToGroupGizmoMode, ChangePivotToGroupGizmoMode);
        EventManager.StartListening(MigEventCommon.UpdateGizmo, UpdateGizmo);
        EventManager.StartListening(MigEventCommon.SetCurrentGizmoState, SetCurrentGizmoState);

        EventManager.StartListening(MigEventCommon.OnDeleteModelUIClick, OnDeleteModelUIClick);
        EventManager.StartListening(MigEventCommon.AnnotateModel, OnAnnotateModel);
        EventManager.StartListening(MigEventCommon.OnPaintModel, OnPaintModel);
        EventManager.StartListening(MigEventCommon.QueryCurrentGizmoState, OnQueryCurrentGizmoState);

        InitGizmo();
    }

    private void SetCurrentGizmoState(object arg0, object arg1)
    {
        GizmoState state = (GizmoState)arg0;
        UpdateGizmoState(state);
    }

    private void OnQueryCurrentGizmoState(object arg0, object arg1)
    {
        EventManager.TriggerEvent(MigEventCommon.ResponseCurrentGizmoState, activeFunction);
    }

    private void OnPaintModel(object arg0, object arg1)
    {
        DisableAllGizmo();
    }

    private void OnAnnotateModel(object arg0, object arg1)
    {
        AnnotateModel();
    }

    private void OnDeleteModelUIClick(object arg0, object arg1)
    {
        DeleteModel();
    }

    private void UpdateGizmo(object arg0, object arg1)
    {
        UpdateGizmos();
    }

    private void ChangePivotToGroupGizmoMode(object arg0, object arg1)
    {
        bool isChangeToGroupCenter = (bool)arg0;

        if (isChangeToGroupCenter)
        {
            SetTransformPivot(RTG.GizmoObjectTransformPivot.ObjectGroupCenter);
        }
        else
        {
            SetTransformPivot(RTG.GizmoObjectTransformPivot.ObjectCenterPivot);
        }
    }

    private void OnDisable()
    {

        EventManager.StopListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
        EventManager.StopListening(MigEventCommon.ChangePivotToGroupGizmoMode, ChangePivotToGroupGizmoMode);
        EventManager.StopListening(MigEventCommon.UpdateGizmo, UpdateGizmo);
        EventManager.StopListening(MigEventCommon.SetCurrentGizmoState, SetCurrentGizmoState);

        EventManager.StopListening(MigEventCommon.AnnotateModel, OnAnnotateModel);
        EventManager.StopListening(MigEventCommon.OnPaintModel, OnPaintModel);
        EventManager.StopListening(MigEventCommon.OnDeleteModelUIClick, OnDeleteModelUIClick);
        EventManager.StopListening(MigEventCommon.QueryCurrentGizmoState, OnQueryCurrentGizmoState);
    }

    private void UpdateGizmoState(GizmoState state)
    {
        if (selectedGameobject == null)
        {
            DisableAllGizmo();
            return;
        }

        switch (state)
        {
            case GizmoState.Move:
                ShowMoveGizmo();
                break;
            case GizmoState.Scale:
                ShowScaleGizmo();
                break;
            case GizmoState.Rotate:
                ShowRotateGizmo();
                break;
        }
        EventManager.TriggerEvent(MigEventCommon.OnGizmoStateChanged, state);
        UpdateGizmos();
    }

    private void OnSelectedChanged(object arg0, object arg1)
    {
        if(arg0 == null)
        {
            return;
        }
        selectedGameobject = (GameObject)arg0;

        if (activeFunction == GizmoState.NULL)
        {
            activeFunction = GizmoState.Move;
        }
        UpdateGizmoState(activeFunction);
    }

    public void ShowMoveGizmo()
    {
        _moveGizmo.Gizmo.SetEnabled(true);

        activeFunction = GizmoState.Move;
        _moveGizmo.SetTransformSpace(GizmoSpace.Local);
        _moveGizmo.SetTargetObject(selectedGameobject);
        DisableGizmo(_rotationGizmo);
        DisableGizmo(_scaleGizmo);
    }

    public void ShowRotateGizmo()
    {
        _rotationGizmo.Gizmo.SetEnabled(true);
        _rotationGizmo.SetTargetObject(selectedGameobject);

        activeFunction = GizmoState.Rotate;
        DisableGizmo(_moveGizmo);
        DisableGizmo(_scaleGizmo);
    }

    public void ShowScaleGizmo()
    {
        _scaleGizmo.Gizmo.SetEnabled(true);
        _scaleGizmo.SetTargetObject(selectedGameobject);

        activeFunction = GizmoState.Scale;
        DisableGizmo(_moveGizmo);
        DisableGizmo(_rotationGizmo);

        Debug.Log("Scale Gizmo created");
    }

    public void DisableAllGizmo()
    {
        DisableGizmo(_moveGizmo);
        DisableGizmo(_scaleGizmo);
        DisableGizmo(_rotationGizmo);
        Debug.Log("Rotate, Scale, and Move functionalities disabled");
    }

    private void DisableGizmo(ObjectTransformGizmo gizmo)
    {
        if (gizmo == null)
        {
            Debug.LogWarning("No move gizmo found!");
            return;
        }
        gizmo.Gizmo.SetEnabled(false);
        Debug.Log("Move functionality disabled");
    }

    public void DeleteModel()
    {
        if (ModelManager.Instance.CurrentGameObjectRoot == null)
        {
            Debug.LogWarning("No model loaded!");
            return;
        }

        DisableAllGizmo();
        activeFunction = GizmoState.NULL;
        _moveGizmo = null;
        _rotationGizmo = null;
        _scaleGizmo = null;

        EventManager.TriggerEvent(Events.OnDeleteModel);
        Debug.Log("Model deleted");
    }

    public Vector3 annotationPosition;
    public void AnnotateModel()
    {
        GameObject annotation = Instantiate(annotationPrefab, annotationPosition, Quaternion.identity, annotationPrefabParent);
        annotation.GetComponent<InputField>().text = "Annotation";
        Debug.Log("Annotation created" );
    }

    public void UpdateGizmos()
    {
        if (_moveGizmo != null) _moveGizmo.RefreshPosition();
        if (_rotationGizmo != null) _rotationGizmo.RefreshRotation();
        if (_scaleGizmo != null) _scaleGizmo.RefreshRotation();
    }

    public void SetTransformPivot(GizmoObjectTransformPivot transformPivot)
    {
        if (_moveGizmo != null)
            _moveGizmo.SetTransformPivot(transformPivot);
        if (_rotationGizmo != null)
            _rotationGizmo.SetTransformPivot(transformPivot);
        if (_scaleGizmo != null)
            _scaleGizmo.SetTransformPivot(transformPivot);
    }

    public void InitGizmo()
    {
        _scaleGizmo = RTGizmosEngine.Get.CreateObjectScaleGizmo();
        _moveGizmo = RTGizmosEngine.Get.CreateObjectMoveGizmo();
        _rotationGizmo = RTGizmosEngine.Get.CreateObjectRotationGizmo();

        _rotationGizmo.Gizmo.SetEnabled(false);
        _moveGizmo.Gizmo.SetEnabled(false);
        _scaleGizmo.Gizmo.SetEnabled(false);

    }
}
