using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    bool rotating = false;
    Vector3 prerot=Vector3.zero;
    Vector3 prepos = Vector3.zero;
    void Update()
    {
        //Debug.Log(GetRotateState());
        //判断当前相机位置是否与上一帧相同
        if (prerot != transform.localEulerAngles|| prepos!=transform.localPosition)
        {
            prerot = transform.localEulerAngles;
            prepos = transform.localPosition;
            rotating = true;
            //Debug.Log("Rotating");
            //disable
        }
        else
            //enable
            rotating = false;
        
    }
    public bool GetRotateState()
    {
        //返回相机旋转状态
        return rotating;
    }
}
