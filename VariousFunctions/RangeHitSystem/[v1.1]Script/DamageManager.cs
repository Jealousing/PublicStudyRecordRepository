using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageManager : Singleton<DamageManager>
{
    protected DamageManager() { }
    public GameObject[] prefabList;

    
    // ������Ʈ Ǯ�� �̿��� ����
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
    // ��ųʸ����� Ǯã�Ƽ� ��ȯ
    private Damage GetDictionaryPool(DamageRangeType type)
    {
        // Dictionary����  �ش� type���� �̷������ �ִ���Ȯ�� -> ������ ����
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
