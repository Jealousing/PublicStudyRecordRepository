using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowZoomCamera : MonoBehaviour
{
    CameraInfo cameraInfo;

    public float smoothness = 3333;
    void Start()
    {
        cameraInfo = GetComponent<CameraInfo>();
    }
    public void BowZoomCamera_Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void BowZoomCamera_Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BowZoomCamera_Update()
    {
        // 이전값과 현재값의 차이
        Vector2 mouseDelta = new Vector2(cameraInfo.mouseAxisX, cameraInfo.mouseAxisY);
        // 카메라의 각도를 오일러 각도로 변환
        Vector3 camAngle = cameraInfo.cameraArm.transform.rotation.eulerAngles;

        float rotX = camAngle.x - mouseDelta.y;

        if (rotX < 180.0f) //위쪽 회전
        {
            // 수평 아래 회전안되는걸 방지하기위해 -1도
            rotX = Mathf.Clamp(rotX, -1.0f, 70.0f);
        }
        else // 180도 이상 아래회전
        {
            rotX = Mathf.Clamp(rotX, 305.0f, 361.0f);
        }
        Quaternion target = Quaternion.Euler(rotX, camAngle.y + mouseDelta.x, 0.0f);

        cameraInfo.cameraArm.transform.rotation = Quaternion.Lerp(cameraInfo.cameraArm.transform.rotation, target, Time.deltaTime * smoothness);
    }

    public void BowZoomCamera_LateUpdate()
    {

    }

    public void BowZoomCamera_FixedUpdate()
    {
        cameraInfo.cameraArm.transform.position = Vector3.Lerp(cameraInfo.cameraArm.transform.position, 
            cameraInfo.followObject.position , Time.deltaTime * cameraInfo.followSpeed);

        cameraInfo.cameraArm.transform.position += cameraInfo.cameraArm.transform.right*0.1f;


        cameraInfo.cameraObj.transform.localPosition = Vector3.Lerp(cameraInfo.cameraObj.transform.localPosition, 
            cameraInfo.dirNormalized , Time.deltaTime * smoothness);

    }

}
