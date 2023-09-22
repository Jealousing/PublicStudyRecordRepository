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

    public Damage DamageBuilder(DamageRangeType type)
    {
        return GetDictionaryPool(type);
    }
    

}
