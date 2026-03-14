using RTG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// only use for mig camera eidtor
public class MigCameraController : MonoBehaviour
{
    public bl_CameraOrbit bl_CameraOrbit;


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        EventManager.StartListening(MigEventCommon.OnGizmosDragBegin, OnGizmosDragBegin);
        EventManager.StartListening(MigEventCommon.OnGizmosDragEnd, OnGizmosDragEnd);
    }

    private void OnGizmosDragEnd(object arg0, object arg1)
    {
        bl_CameraOrbit.enabled = true;
    }

    private void OnGizmosDragBegin(object arg0, object arg1)
    {
        bl_CameraOrbit.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.StopListening(MigEventCommon.OnGizmosDragBegin, OnGizmosDragBegin);
        EventManager.StopListening(MigEventCommon.OnGizmosDragEnd, OnGizmosDragEnd);
    }

}
