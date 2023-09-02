using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class NoiseManager : MonoBehaviour
{
    // 복사할 오브젝트
    public GameObject CopyObj;
    // 복제된 오브젝트 모와둘곳
    public GameObject TempStorage;

    // 레이 변수
    RaycastHit hit;
    // 밞고 있는 땅의 세팅
    GroundNoiseSet m_Ground;

    //소음오브젝트 복제(생성)
    public void CreateNoise(int p_NoiseLv, Vector3 p_vec3)
    {
        // 소음 레벨 설정
        CopyObj.GetComponent<NoiseData>().m_NoiseLv=p_NoiseLv;
        // 오브젝트 복사
        GameObject Result = Instantiate(CopyObj, p_vec3, Quaternion.identity);

        // 모와두는 곳으로 옮김
        Result.transform.parent = TempStorage.transform;
        // Active On
        Result.SetActive(true);
        // 일정 시간뒤 오브젝트 파괴
        StartCoroutine(DestroyNoise(Result));
    }

    // 플레이어 이동용 소음
    public void CreateNoise_Ani(string p_aniName)
    {
        int NoiseLv = 0;
        // 바닥 재질에 따른 소음레벨 변화후 소음레벨 설정
        NoiseLv=CopyObj.GetComponent<NoiseData>().m_NoiseLv = NoiseLvSet(p_aniName);
        // 오브젝트 복사
        GameObject Result = Instantiate(CopyObj, this.transform.position , Quaternion.identity);
        // 이름설정
        Result.name = "Noise_Player_Ani_Lv"+ NoiseLv;

        // 모와두는 곳으로 옮김
        Result.transform.parent = TempStorage.transform;
        // Active On
        Result.SetActive(true);
        // 일정 시간뒤 오브젝트 파괴
        StartCoroutine(DestroyNoise(Result));
    }

    // 소음 오브젝트 파괴
    IEnumerator DestroyNoise(GameObject p_obj)
    {
        // 4.5초후
        yield return new WaitForSeconds(4.5f);
        // 파괴
        Destroy(p_obj);
        // 예외문제 발생 처리 ( 파괴 되었어도 인식되는 문제 )
        p_obj = null;

    }

    // 바닥 재질에 따른 소음 레벨 변화
    int NoiseLvSet(string p_aniName)
    {
        int NoiseLv = 0;
        // 레이로 바닥확인
        if( Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y+0.2f, 
            this.transform.position.z), -transform.up, out hit, 1f))
        {
            // 바닥이 설정된 값이 없으면 기본값으로 return
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
                // 설정된 바닥이 있을 경우
                m_Ground = hit.transform.gameObject.GetComponent<GroundNoiseSet>();
                // 타입에 따라 설정
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
