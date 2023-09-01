using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackViewCamera : MonoBehaviour
{
    CameraInfo cameraInfo;

    public float minDistance = 1;
    public float maxDistance = 6;
    public float finalDistance =  0;
    public float smoothness = 3333;
    float scroll;

    private void Start()
    {
        cameraInfo = GetComponent<CameraInfo>();
    }

    public void BackViewCamera_Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void BackViewCamera_Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void BackViewCamera_Update()
    {
        // �������� ���簪�� ����
        Vector2 mouseDelta = new Vector2(cameraInfo.mouseAxisX, cameraInfo.mouseAxisY);
        // ī�޶��� ������ ���Ϸ� ������ ��ȯ
        Vector3 camAngle = cameraInfo.cameraArm.transform.rotation.eulerAngles;

        float rotX = camAngle.x - mouseDelta.y;
        if (rotX < 180.0f) //���� ȸ��
        {
            // ���� �Ʒ� ȸ���ȵǴ°� �����ϱ����� -1��
            rotX = Mathf.Clamp(rotX, -1.0f, 70.0f);
        }
        else // 180�� �̻� �Ʒ�ȸ��
        {
            rotX = Mathf.Clamp(rotX, 335.0f, 361.0f);
        }
        Quaternion target = Quaternion.Euler(rotX, camAngle.y + mouseDelta.x, 0.0f);

        cameraInfo.cameraArm.transform.rotation = Quaternion.Lerp(cameraInfo.cameraArm.transform.rotation, target, Time.deltaTime * smoothness);

    }
    
    public void BackViewCamera_LateUpdate()
    {
    }

    public void BackViewCamera_FixedUpdate()
    {
        scroll = Mathf.Clamp(scroll + cameraInfo.mouseScroll, minDistance - maxDistance, 0);
        cameraInfo.cameraArm.transform.position = Vector3.Lerp(cameraInfo.cameraArm.transform.position, cameraInfo.followObject.position, Time.deltaTime * cameraInfo.followSpeed);
        cameraInfo.finalDir = cameraInfo.cameraArm.transform.TransformPoint(cameraInfo.dirNormalized * maxDistance);

        RaycastHit hit;
        if (Physics.Linecast(cameraInfo.followObject.transform.position, cameraInfo.finalDir, out hit, LayerMask.NameToLayer("Player")))
        {
            finalDistance = Mathf.Clamp(hit.distance + scroll -1.0f, minDistance, maxDistance);
        }
        else
        {
            finalDistance = Mathf.Clamp(maxDistance + scroll, minDistance, maxDistance); 
        }

        cameraInfo.cameraObj.transform.localPosition = Vector3.Lerp(cameraInfo.cameraObj.transform.localPosition, cameraInfo.dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

}
