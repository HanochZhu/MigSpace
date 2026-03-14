using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AdsorptionScript : MonoBehaviour
{
    public float snapDistance = 1.0f; // 吸附的距离阈值
    public float snapSpeed = 5.0f; // 吸附速度

    private bool isDragging = false; // 是否正在拖动
    private Transform draggingObject = null; // 当前拖动的物体

    void Start()
    {
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.OnGizmosDragBegin, OnGizmosDragBegin); // 监听拖拽开始事件
        EventManager.StartListening(Events.OnGizmosDragEnd, OnGizmosDragEnd); // 监听拖拽结束事件
    }

    void Update()
    {
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.OnGizmosDragBegin, OnGizmosDragBegin); // 停止监听拖拽开始事件
        EventManager.StopListening(Events.OnGizmosDragEnd, OnGizmosDragEnd); // 停止监听拖拽结束事件
    }

    /**
     * 处理拖拽开始事件
     */
    private void OnGizmosDragBegin(object arg0, object arg1)
    {
        // Debug.Log($"OnGizmosDragBegin: arg0 = {arg0}, arg1 = {arg1}");
        draggingObject = transform;
        if (draggingObject != null)
        {
            isDragging = true; // 标记为正在拖动
        }
    }

    /**
     * 处理拖拽结束事件
     */
    private void OnGizmosDragEnd(object arg0, object arg1)
    {
        // Debug.Log($"OnGizmosDragEnd: arg0 = {arg0}, arg1 = {arg1}");
        if (isDragging)
        {
            isDragging = false; // 停止拖动
            SnapToClosestPoint(); // 尝试吸附到最近的吸附点
            draggingObject = null; // 清除拖动物体引用
        }
    }

    /**
     * 查找最近的吸附点并吸附
     */
    private void SnapToClosestPoint()
    {
        // // 获取所有吸附点   
        // SnapPoint[] snapPoints = FindObjectsOfType<SnapPoint>();

        // SnapPoint closestSnapPoint = null; // 最近的吸附点
        // float minDistance = Mathf.Infinity; // 初始化最小距离为无穷大

        // // 遍历所有吸附点，找到最近的一个
        // foreach (SnapPoint snapPoint in snapPoints)
        // {
        //     // 获取拖动物体的子物体
        //     foreach (Transform child in draggingObject)
        //     {
        //         // 检查吸附点的名字是否匹配
        //         if (child.name == snapPoint.snapName)
        //         {
        //             float distance = Vector3.Distance(draggingObject.position, snapPoint.transform.position);
        //             if (distance < snapDistance && distance < minDistance)
        //             {
        //                 minDistance = distance; // 更新最小距离
        //                 closestSnapPoint = snapPoint; // 更新最近的吸附点
        //             }
        //         }
        //     }
        // }

        float minDistance = Mathf.Infinity; // 初始化最小距离为无穷大
        GameObject closestSnapPoint = null; // 最近的吸附点
        GameObject selfSnapPoint = null; //自身的吸附点

        Debug.Log("child Count = " + draggingObject.childCount);
        // 获取拖动物体的子物体
        foreach (Transform child in draggingObject)
        {
            string[] nameSplit = child.name.Split("_");
            string otherPointName = "";
            for (int i = 0; i < nameSplit.Length - 1; i++)
            {
                if (otherPointName == "")
                {
                    otherPointName = nameSplit[i];
                }
                else
                {
                    otherPointName = otherPointName + "_" + nameSplit[i];
                }
            }
            if (string.Equals(nameSplit[nameSplit.Length - 1], "link001"))
            {
                otherPointName = otherPointName + "_" + "link002";
            }
            else if (string.Equals(nameSplit[nameSplit.Length - 1], "link002"))
            {
                otherPointName = otherPointName + "_" + "link001";
            }
            Debug.Log("Find point name = " + otherPointName);
            //查找和拖动物体名称相同的子物体(另一个吸附点
            GameObject point = GameObject.Find(otherPointName);
            if (point)
            {
                //需要对比的是 other点(point)和 自身的那个点的坐标
                // float distance = Vector3.Distance(draggingObject.position, point.transform.position);
                float distance = Vector3.Distance(child.position, point.transform.position);
                
                if (distance <= snapDistance && distance < minDistance)
                {
                    minDistance = distance; // 更新最小距离
                    closestSnapPoint = point; // 更新最近的吸附点
                    selfSnapPoint = child.gameObject;
                }
            }
            else
            {
                Debug.LogError("Not Find other point gameObject! other Point Name = " + otherPointName);
            }
        }

        // 如果找到最近的吸附点，开始吸附
        if (closestSnapPoint != null)
        {
            StartCoroutine(SnapToTarget(draggingObject, selfSnapPoint.transform, closestSnapPoint.transform));
            // StartCoroutine(SnapToTarget(selfSnapPoint.transform, closestSnapPoint.transform));
        }
    }

    /**
     * 吸附协程
     */
    private IEnumerator SnapToTarget(Transform obj, Transform selfSnapPoint, Transform target)
    {
        Debug.Log("start  obj = " + obj.name + " selfSnapPoint = " + selfSnapPoint.name + " target = " + target.name);
        Vector3 initialPosition = obj.position; // 记录初始位置
        Quaternion initialRotation = obj.rotation; // 记录初始旋转
        // 计算平移和旋转的差异
        Vector3 targetPosition = obj.position + (target.position - selfSnapPoint.position);
        // Quaternion targetRotation = obj.rotation * Quaternion.Inverse(selfSnapPoint.rotation) * target.rotation;

        // 平滑移动和旋转到吸附点
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * snapSpeed;
            obj.position = Vector3.Lerp(initialPosition, targetPosition, t);
            // obj.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        // 最终确保完全对齐吸附点
        obj.position = targetPosition;
        // obj.rotation = targetRotation;


        // // 平滑移动和旋转到吸附点
        // while (Vector3.Distance(obj.position, target.position) > 0.01f)
        // {
        //     obj.position = Vector3.Lerp(obj.position, target.position, snapSpeed * Time.deltaTime);
        //     obj.rotation = Quaternion.Lerp(obj.rotation, target.parent.rotation, snapSpeed * Time.deltaTime);
        //     yield return null;
        // }
        // // 最终确保完全对齐吸附点
        // obj.position = target.position;
        // obj.rotation = target.parent.rotation;
    }
}