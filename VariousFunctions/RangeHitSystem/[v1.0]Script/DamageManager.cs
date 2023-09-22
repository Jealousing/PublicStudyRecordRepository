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

    // �޾ƾ��� �Ű����� <= ����� ���� Ÿ�� , �߽���ġ, ���̰� �⺻.
    // ����(������), ������(�ּҰŸ�,�ִ�Ÿ�) , ��ä����(���������� ����,�����߰�), ���簢�� �Ǵ� ���簢��(����,���μ���)

    // ��������� ������� ������ ǥ���ϴ� �͵� ������ 
    // �׸��⸸�� ���� (fiil ���ε� �߿� ->��¡?) ��ų������(keyup) ����ǥ�� ������ �׸��⵵ �ʿ�

    // ���������� ���� ����
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

    // �����ε��� �̿�
    // �⺻ + �Ÿ�����2��
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
            Debug.Log(" ������ ���� ���ϴ� ����� ������ ���� �ٸ��ϴ�.");
            return null;
        }
    }

    // �⺻+ �Ÿ�����2�� + ��������
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
            Debug.Log(" ������ ���� ���ϴ� ����� ������ ���� �ٸ��ϴ�.");
            return null;
        }
    }

    // �⺻ + ȸ������ + �Ÿ�����2��
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
            Debug.Log(" ������ ���� ���ϴ� ����� ������ ���� �ٸ��ϴ�.");
            return null;
        }
    }

    // �⺻ + ȸ������ + �Ÿ�����2�� + �������� or ����
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
            Debug.Log(" ������ ���� ���ϴ� ����� ������ ���� �ٸ��ϴ�.");
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
            Debug.Log(" ������ ���� ���ϴ� ����� ������ ���� �ٸ��ϴ�.");
            return null;
        }
    }
}
