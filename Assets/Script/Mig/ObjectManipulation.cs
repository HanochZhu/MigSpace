using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDrag()
    {
        float rotationSpeed = 10f;
        float scaleSpeed = 0.1f;

        // 旋转
        float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;
        transform.Rotate(Vector3.up, -rotX, Space.World);
        transform.Rotate(Vector3.right, rotY, Space.World);

        // 缩放
        float scale = Input.GetAxis("Mouse ScrollWheel");
        transform.localScale += Vector3.one * scale * scaleSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        // 获取物体的世界坐标
        Vector3 position = transform.position;

        // 绘制红色 X 轴箭头
        Gizmos.color = Color.red;
        Gizmos.DrawRay(position, transform.right * 0.5f);

        // 绘制绿色 Y 轴箭头
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, transform.up * 0.5f);

        // 绘制蓝色 Z 轴箭头
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(position, transform.forward * 0.5f);
    }

    public float axisLength = 1f;

    private void OnDrawGizmos()
    {
        DrawAxis(transform.position, transform.rotation);
    }

    private void DrawAxis(Vector3 position, Quaternion rotation)
    {
        // 绘制红色 X 轴箭头
        DrawArrow(position, rotation * Vector3.right, Color.red);

        // 绘制绿色 Y 轴箭头
        DrawArrow(position, rotation * Vector3.up, Color.green);

        // 绘制蓝色 Z 轴箭头
        DrawArrow(position, rotation * Vector3.forward, Color.blue);
    }

    private void DrawArrow(Vector3 position, Vector3 direction, Color color)
    {
        Gizmos.color = color;

        Vector3 end = position + direction * axisLength;
        Gizmos.DrawLine(position, end);

        Vector3 arrowPoint1 = end + Quaternion.Euler(0, 160, 0) * direction * 0.05f * axisLength;
        Vector3 arrowPoint2 = end + Quaternion.Euler(0, -160, 0) * direction * 0.05f * axisLength;
        Gizmos.DrawLine(end, arrowPoint1);
        Gizmos.DrawLine(end, arrowPoint2);
    }
}
