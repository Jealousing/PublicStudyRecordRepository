using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ű�� ������ ������ ������ �����ְ� �����ð� ���� �����

/*
 -> ������ƮǮüũ�ؼ� ������Ʈ�� �����´�
 -> ������ ������Ʈ�� ������ �ӵ��� �������ϰ� Ȱ��ȭ�Ѵ�
 -> Ȱ��ȭ�Ǹ� �ð����°� ������ ǥ�����ְ� 
 -> �ð��� �Ǿ����� ������ Ȯ���� Ư�� Ÿ���� �� ������ ������ ������� �ֵ��� �����Ѵ�.

�� ����� �ڵ�� �����ϱ� ���� �ʿ��Ѱ�.
1. ������Ʈ Ǯ�� ������ �Ŵ��� �ϳ� ����
2. �Ŵ������� ������� �ִ� ��Ŀ� ���� Ǯ ����
    ������� �ִ¹��:
    2.1. ����
    2.2 ������(���ξ���)
    2.3 ��ä���� (�������¿��� ������ �����Ѱ�)
    2.4 ���簢�� �Ǵ� ���簢��
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

            // �ӽ������ؼ� ���ϴ½����� ����Ⱑ��
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

            // ������
            DamageManager.GetInstance.CallDamageRange(DamageRangeType.CUBE, true, 2.0f, 200f,
                LayerMask.GetMask("Enemy"), pos, rot, minDistance, maxDistance);
        }
    }
}
