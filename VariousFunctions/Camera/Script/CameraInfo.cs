using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 카메라에 필요한 정보를 가지고 있는 클래스
/// </summary>
public class CameraInfo : MonoBehaviour
{
    // 카메라 상태관리
    public CameraStateSystem CameraStateSystem;

    public Transform followObject;
    public float followSpeed = 10.0f;

    public Transform cameraArm;
    public Camera cameraObj;

    [NonSerialized] public Vector3 dirNormalized;
    [NonSerialized] public Vector3 finalDir;
    [NonSerialized] public float finalDistance;

    [NonSerialized] public float mouseAxisX;
    [NonSerialized] public float mouseAxisY;
    [NonSerialized] public float mouseScroll;
    public float wheelSensitivity = 2.0f;
    void Start()
    {
        dirNormalized = cameraObj.transform.localPosition.normalized;
        finalDistance = cameraObj.transform.localPosition.magnitude;
    }


    private void Update()
    {
        MouseInput();
    }

    void MouseInput()
    {
        mouseAxisX = Input.GetAxis("Mouse X");
        mouseAxisY = Input.GetAxis("Mouse Y");
        mouseScroll = Input.GetAxis("Mouse ScrollWheel") * wheelSensitivity;
    }
}
