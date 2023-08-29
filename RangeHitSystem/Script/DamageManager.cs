using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageManager : Singleton<DamageManager>
{
    protected DamageManager() { }
    public GameObject[] prefabList;

    
    // 오브젝트 풀을 이용한 관리
    private Dictionary<DamageRangeType, IObjectPool<Damage>> damageRangePools;
   
    private void Awake()
    {
        damageRangePools = new Dictionary<DamageRangeType, IObjectPool<Damage>>();
    }

    #region Pool

    private Damage CreateDamage(DamageRangeType type)
    {
        Damage damage = Instantiate(prefabList[(int)type].GetComponent<Damage>());
        damage.transform.parent = this.transform;
        damage.Set(damageRangePools[type]);
        return damage;
    }
    private void OnGetDamage(Damage damage)
    {
        damage.gameObject.SetActive(true);
    }

    private void OnReleaseDamage(Damage damage)
    {
        damage.gameObject.SetActive(false);
    }

    private void OnDestroyDamage(Damage damage)
    {
        Destroy(damage.gameObject);
    }
    public void AddDictionary(DamageRangeType type)
    {
        damageRangePools.Add(type, new ObjectPool<Damage>(() => CreateDamage(type),
                                                           OnGetDamage, OnReleaseDamage, OnDestroyDamage, maxSize: 5));
    }
    // 딕셔너리에서 풀찾아서 반환
    private Damage GetDictionaryPool(DamageRangeType type)
    {
        // Dictionary에서  해당 type으로 이루어진게 있는지확인 -> 없으면 생성
        if (!damageRangePools.ContainsKey(type))
        {
            AddDictionary(type);
        }

        return damageRangePools[type].Get();
    }

    #endregion

    // 받아야할 매개변수 <= 대미지 범위 타입 , 중심위치, 높이가 기본.
    // 원형(반지름), 도넛형(최소거리,최대거리) , 부채꼴형(도넛형에서 방향,각도추가), 직사각형 또는 정사각형(방향,가로세로)

    // 정해진대로 대미지와 범위를 표시하는 것도 있지만 
    // 그리기만을 통해 (fiil 여부도 중요 ->차징?) 스킬쓰기전(keyup) 범위표시 지원용 그리기도 필요

    // 공통적으로 들어가는 설정
    private void DefaultSetting(Damage damage, bool isDraw, float delayTime, float damageValue, int targetLayer, Vector3 centerPos)
    {
        damage.transform.position = centerPos;

        if (delayTime > 0)
        {
            damage.isFill = true;
            damage.fillTime = delayTime;
        }
        if (damageValue>0)
        {
            damage.isDamage = true;
            damage.damage = damageValue;
            damage.targetLayer = targetLayer;
        }
        if (isDraw)
        {
            damage.StartDraw();
        }
        else
        {
            damage.StartDamage();
        }
    }

    // 오버로딩을 이용
    // 기본 + 거리변수2개
    public Damage CallDamageRange(DamageRangeType type,bool isDraw, float delayTime, float damageValue, int targetLayer, Vector3 centerPos,
       float minDistance, float maxDistance)
    {
        if(type == DamageRangeType.DONUT)
        {
            Damage damage = GetDictionaryPool(type);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.limitAngle = 180.0f;
            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);

            return damage;
        }
        else if(type == DamageRangeType.CUBE)
        {
            Damage damage = GetDictionaryPool(type);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.width = 3.0f;
            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);
            return damage;
        }
        else
        {
            Debug.Log(" 설정된 값과 원하는 모양의 조건이 서로 다릅니다.");
            return null;
        }
    }

    // 기본+ 거리변수2개 + 각도제한
    public Damage CallDamageRange(DamageRangeType type, bool isDraw, float delayTime, float damageValue, int targetLayer, Vector3 centerPos, 
       float minDistance, float maxDistance, float limitAngle)
    {
        if (type == DamageRangeType.LIMITEDANGLEDONUT)
        {
            Damage damage = GetDictionaryPool(type);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.limitAngle = limitAngle;

            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);

            return damage;
        }
        else if (type == DamageRangeType.CUBE)
        {
            Damage damage = GetDictionaryPool(type);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.width = limitAngle;
            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);
            return damage;
        }
        else
        {
            Debug.Log(" 설정된 값과 원하는 모양의 조건이 서로 다릅니다.");
            return null;
        }
    }

    // 기본 + 회전각도 + 거리변수2개
    public Damage CallDamageRange(DamageRangeType type, bool isDraw, float delayTime, float damageValue, int targetLayer, Vector3 centerPos,
        Vector3 rotAngle, float minDistance, float maxDistance)
    {
        if (type == DamageRangeType.LIMITEDANGLEDONUT)
        {
            Damage damage = GetDictionaryPool(type);
            damage.transform.rotation = Quaternion.identity;
            damage.transform.Rotate(rotAngle);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;

            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);

            return damage;
        }
        else if(type == DamageRangeType.CUBE)
        {
            Damage damage = GetDictionaryPool(type); 
            damage.transform.rotation = Quaternion.identity;
            damage.transform.Rotate(rotAngle);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.width = 3.0f;
            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);
            return damage;
        }
        else
        {
            Debug.Log(" 설정된 값과 원하는 모양의 조건이 서로 다릅니다.");
            return null;
        }
    }

    // 기본 + 회전각도 + 거리변수2개 + 각도제한 or 넓이
    public Damage CallDamageRange(DamageRangeType type, bool isDraw, float delayTime, float damageValue, int targetLayer, Vector3 centerPos, 
       Vector3 rotAngle, float minDistance, float maxDistance, float limitAngle)
    {
        if (type == DamageRangeType.LIMITEDANGLEDONUT)
        {
            Damage damage = GetDictionaryPool(type);
            damage.transform.rotation = Quaternion.identity;
            damage.transform.Rotate (rotAngle);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.limitAngle = limitAngle;

            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);

            return damage;
        }
        else if (type == DamageRangeType.CUBE)
        {
            Damage damage = GetDictionaryPool(type);
            damage.transform.rotation = Quaternion.identity;
            damage.transform.Rotate(rotAngle);
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.width = limitAngle;
            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);
            return damage;
        }
        else
        {
            Debug.Log(" 설정된 값과 원하는 모양의 조건이 서로 다릅니다.");
            return null;
        }
    }

    public Damage CallDamageRange(DamageRangeType type, bool isDraw, float delayTime, float damageValue, int targetLayer, Vector3 centerPos,
      Quaternion rotAngle, float minDistance, float maxDistance, float limitAngle)
    {
        if (type == DamageRangeType.LIMITEDANGLEDONUT)
        {
            Damage damage = GetDictionaryPool(type);
            damage.transform.rotation = rotAngle;
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.limitAngle = limitAngle;

            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);

            return damage;
        }
        else if (type == DamageRangeType.CUBE)
        {
            Damage damage = GetDictionaryPool(type);
            damage.transform.rotation = rotAngle;
            damage.minDistance = minDistance;
            damage.maxDistance = maxDistance;
            damage.width = limitAngle;
            DefaultSetting(damage, isDraw, delayTime, damageValue, targetLayer, centerPos);
            return damage;
        }
        else
        {
            Debug.Log(" 설정된 값과 원하는 모양의 조건이 서로 다릅니다.");
            return null;
        }
    }
}
