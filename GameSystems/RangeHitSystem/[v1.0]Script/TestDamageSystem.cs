using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 정해진 범위를 보여주고 일정시간 이후 대미지

/*
 -> 오브젝트풀체크해서 오브젝트를 가져온다
 -> 가져온 오브젝트를 범위든 속도든 설정을하고 활성화한다
 -> 활성화되면 시간에맞게 범위를 표시해주고 
 -> 시간이 되었으면 범위를 확인해 특정 타겟이 그 범위에 있으면 대미지를 주도록 설정한다.

이 방식을 코드로 구현하기 위해 필요한것.
1. 오브젝트 풀을 관리할 매니저 하나 생성
2. 매니저에서 대미지를 주는 방식에 따른 풀 관리
    대미지를 주는방식:
    2.1. 원형
    2.2 도넛형(내부안전)
    2.3 부채꼴형 (도넛형태에서 각도만 제한한것)
    2.4 직사각형 또는 정사각형
 */
public class TestDamageSystem : MonoBehaviour
{
    Damage temp;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            float x = Random.Range(-40.0f, 55.0f);
            float z = Random.Range(40.0f, 140.0f);
            float minDistance = Random.Range(0.0f, 5.0f);
            float maxDistance = Random.Range(6.0f, 20.0f);
            float rot = Random.Range(0.0f, 360.0f);
            float angle = Random.Range(10.0f, 180.0f);

            // 임시저장해서 원하는시점에 지우기가능
            //temp = 
            DamageManager.GetInstance.CallDamageRange(DamageRangeType.CUBE, true, 2.0f, 200f, 
                LayerMask.GetMask("Enemy"), new Vector3(x, 0.001f, z), new Vector3(0,rot,0), minDistance , maxDistance);
        }

        if(Input.GetKeyUp(KeyCode.Keypad7) && temp !=null)
        {
            temp.StopDraw();            
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Vector3 pos = PlayerInfo.GetInstance.transform.position + new Vector3(0, 0.1f, 0);
            float minDistance = Random.Range(0.0f, 5.0f);
            float maxDistance = Random.Range(6.0f, 20.0f);
            Vector3 rot = Vector3.up;

            // 도넛형
            DamageManager.GetInstance.CallDamageRange(DamageRangeType.CUBE, true, 2.0f, 200f,
                LayerMask.GetMask("Enemy"), pos, rot, minDistance, maxDistance);
        }
    }
}
