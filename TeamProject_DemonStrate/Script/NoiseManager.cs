using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: ������
public class NoiseManager : MonoBehaviour
{
    // ������ ������Ʈ
    public GameObject CopyObj;
    // ������ ������Ʈ ��͵Ѱ�
    public GameObject TempStorage;

    // ���� ����
    RaycastHit hit;
    // ��� �ִ� ���� ����
    GroundNoiseSet m_Ground;

    //����������Ʈ ����(����)
    public void CreateNoise(int p_NoiseLv, Vector3 p_vec3)
    {
        // ���� ���� ����
        CopyObj.GetComponent<NoiseData>().m_NoiseLv=p_NoiseLv;
        // ������Ʈ ����
        GameObject Result = Instantiate(CopyObj, p_vec3, Quaternion.identity);

        // ��͵δ� ������ �ű�
        Result.transform.parent = TempStorage.transform;
        // Active On
        Result.SetActive(true);
        // ���� �ð��� ������Ʈ �ı�
        StartCoroutine(DestroyNoise(Result));
    }

    // �÷��̾� �̵��� ����
    public void CreateNoise_Ani(string p_aniName)
    {
        int NoiseLv = 0;
        // �ٴ� ������ ���� �������� ��ȭ�� �������� ����
        NoiseLv=CopyObj.GetComponent<NoiseData>().m_NoiseLv = NoiseLvSet(p_aniName);
        // ������Ʈ ����
        GameObject Result = Instantiate(CopyObj, this.transform.position , Quaternion.identity);
        // �̸�����
        Result.name = "Noise_Player_Ani_Lv"+ NoiseLv;

        // ��͵δ� ������ �ű�
        Result.transform.parent = TempStorage.transform;
        // Active On
        Result.SetActive(true);
        // ���� �ð��� ������Ʈ �ı�
        StartCoroutine(DestroyNoise(Result));
    }

    // ���� ������Ʈ �ı�
    IEnumerator DestroyNoise(GameObject p_obj)
    {
        // 4.5����
        yield return new WaitForSeconds(4.5f);
        // �ı�
        Destroy(p_obj);
        // ���ܹ��� �߻� ó�� ( �ı� �Ǿ�� �νĵǴ� ���� )
        p_obj = null;

    }

    // �ٴ� ������ ���� ���� ���� ��ȭ
    int NoiseLvSet(string p_aniName)
    {
        int NoiseLv = 0;
        // ���̷� �ٴ�Ȯ��
        if( Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y+0.2f, 
            this.transform.position.z), -transform.up, out hit, 1f))
        {
            // �ٴ��� ������ ���� ������ �⺻������ return
            if(hit.transform.gameObject.GetComponent<GroundNoiseSet>()==null)
            {
                switch(p_aniName)
                {
                    case "Walk":
                        NoiseLv = 2;
                        break;
                    case "Run":
                        NoiseLv = 3;
                        break;
                    case "Jump":
                        NoiseLv = 3;
                        break;
                    case "Crouch":
                        NoiseLv = 1;
                        break;
                }
                return NoiseLv;
            }
            else
            {
                // ������ �ٴ��� ���� ���
                m_Ground = hit.transform.gameObject.GetComponent<GroundNoiseSet>();
                // Ÿ�Կ� ���� ����
                switch (m_Ground.m_GroundType)
                {
                    case E_GroundType.Wood:
                        switch (p_aniName)
                        {
                            case "Walk":
                                NoiseLv = 2;
                                break;
                            case "Run":
                                NoiseLv = 3;
                                break;
                            case "Jump":
                                NoiseLv = 3;
                                break;
                            case "Crouch":
                                NoiseLv = 1;
                                break;
                        }
                        break;

                    case E_GroundType.Bandage:
                        NoiseLv = 1;
                        break;

                    case E_GroundType.StickOutWood:
                        NoiseLv = 3;
                        break;

                    case E_GroundType.OnTopOfObj:
                        switch (p_aniName)
                        {
                            case "Walk":
                                NoiseLv = 2;
                                break;
                            case "Run":
                                NoiseLv = 3;
                                break;
                            case "Jump":
                                NoiseLv = 3;
                                break;
                            case "Crouch":
                                NoiseLv = 1;
                                break;
                        }
                        break;
                }
                return NoiseLv;
            }
        }
        else
        {
            NoiseLv = 0;
            return NoiseLv;
        }
    }
}
