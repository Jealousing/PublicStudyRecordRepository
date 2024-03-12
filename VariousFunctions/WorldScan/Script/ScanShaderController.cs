using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanShaderController : MonoBehaviour
{
    public Material material;
    MovementInfo playerInfo;

    private bool isShaderActive = false; // ���̴� Ȱ��ȭ ����
    private float progress = 0; // �μ� ��
    private float maxProgress = 1; // �ִ� �μ� ��

    public ScanCollider scanObj;
    private bool isCoroutineRunning = false;

    void Update()
    {
        if (scanObj.gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Y) && !isCoroutineRunning && PlayerInfo.GetInstance.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBlend"))
        {
            StartCoroutine(startScan());
        }
    }

    IEnumerator startScan()
    {
        playerInfo = PlayerInfo.GetInstance.movementInfo;
        isCoroutineRunning = true;
        playerInfo.moveState.ChangeState(MoveState.NotMovement);
        progress = 0.003f;
        
        yield return null;


        playerInfo.animator.SetTrigger(playerInfo.aniHashScan);

        while (!playerInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("ScanAni"))
        {
            yield return null;
        }
        scanObj.ScanStart();
        scanObj.transform.position = playerInfo.handIK.rightHand.transform.position + new Vector3(0, 0.05f, 0);
        scanObj.Scanning(progress);
        while (playerInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("ScanAni") &&
            playerInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
        {
            scanObj.transform.position = playerInfo.handIK.rightHand.transform.position + new Vector3(0, 0.05f, 0);
            yield return null;
        }

        while (playerInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("ScanAni") &&
           playerInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.95f)
        {
            progress -= 0.0001f;
            if(progress>0)
            {
                scanObj.transform.position = playerInfo.handIK.rightHand.transform.position + new Vector3(0, 0.05f, 0);
                scanObj.Scanning(progress);
            }
            yield return null;
        }


        scanObj.MeshOff();
        progress = 0.01f;
        material.SetFloat("_Progress", progress);
        isShaderActive = true;
        playerInfo.moveState.ChangeState(MoveState.DefaultMovement);
    }

    private void FixedUpdate()
    {
        if (isShaderActive)
        {
            // progress ���� ���̴��� ���������� ����
            material.SetFloat("_Progress", progress);

            // �۾��� �Ϸ�Ǹ� ��� ��Ȱ��ȭ
            if (progress >= maxProgress)
            {
                progress = 0.01f;
                isShaderActive = false;
                isCoroutineRunning = false;
                StartCoroutine(scanObj.ScanEnd());
            }

            // progress ����
            progress+=0.01f;
            scanObj.Scanning(progress);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isShaderActive)
        {
            // ���̴��� ����Ͽ� ������
            Graphics.Blit(source, destination, material);
        }
        else
        {
            // ����� ��Ȱ��ȭ�� ��� ���� �̹����� ����
            Graphics.Blit(source, destination);
        }
    }

    void LateUpdate()
    {
        if(isCoroutineRunning || isShaderActive) 
            material.SetVector("_Position", PlayerInfo.GetInstance.movementInfo.handIK.rightHand.transform.position);
    }
}

