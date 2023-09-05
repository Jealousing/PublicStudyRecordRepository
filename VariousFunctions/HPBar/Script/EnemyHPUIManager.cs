using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHPUIManager : MonoBehaviour
{
    /*
     ī�޶� �þ� ���� ���� ������ ���̴� ���� �Ӹ��� HP Bar�� ���̰� ���ִ� ��ũ��Ʈ �ۼ� ��ȹ.

     ��������:
     1) ī�޶�������� forward vector ����������� ���� ������ ���Ե� ������ HPBar�� 
         ������Ʈ Ǯ���� �̿��ؼ� HPBar�� �������ش�.
    
     ������ ����� HPBar�� �����Ǵ� ������ ���Ŀ� ����:
     1)  ������ Ÿ���� �ڽ��� �Ǿ��� ��� 
     2) �÷��̾ Ÿ���� ������Ų ���͸� ����
     
    ������� ����:
    1)  �þ� ������ ���
    2) ���Ϳ� �÷��̾� ���̿� ��ֹ��� ������
    3) ���Ͱ� ������ų� ������� ��� 
     */

    [SerializeField] GameObject prefabHPBar = null;
    private IObjectPool<EnemyHPUI> HPBarPool;
    [SerializeField] Camera playerCamera = null;
    public Transform playerTr;
    [SerializeField] LayerMask enemyMask;
    public float createAngle =20.0f;
    private LayerMask exceptionLayer;
    public float setRadius;
    string enemyTag = "Enemy";

    private void Awake()
    {
        // ������Ʈ Ǯ ����
        HPBarPool = new ObjectPool<EnemyHPUI>(CreateHPUI, OnGetHPUI, OnReleaseHPUI, OnDestroyHPUI, maxSize: 30);
    }

    void Start()
    {
        // ���� ���̾� ����
        exceptionLayer = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player"));
    }

    private void Update()
    {
        // ���� ������ ������ �ݶ��̴��� Ž���Ѵ�
        Collider[] colliders = Physics.OverlapSphere(playerTr.position, setRadius, enemyMask,QueryTriggerInteraction.Ignore);

        for( int i=0; i<colliders.Length;i++)
        {
            if (colliders[i].CompareTag(enemyTag))
            {
                Vector3 temp = colliders[i].transform.position;
                EnemyInfo enemy;
                // �̹� HPBar�� �����ϰ��ִ��� Ȯ��
                if ((enemy = colliders[i].GetComponent<EnemyInfo>()).HPUI==null)
                {
                    // ���� �÷��̾� ���̿� ���ع��� �ִ��� Ȯ��
                    if (checkRaycast(temp,createAngle))
                    {
                        EnemyHPUI enemyHPUI = HPBarPool.Get();
                        enemyHPUI.Setting(enemy, playerCamera,this,createAngle+5.0f);
                        enemy.HPUI = enemyHPUI;
                    }
                }
            }
        }
    }

    public bool checkRaycast(Vector3 pos , float setAngle)
    {
        RaycastHit hit;
        
        Vector3 direction = (pos - playerCamera.transform.position).normalized;
        float angle = Vector3.Angle(direction, playerCamera.transform.forward);
        if (angle < setAngle && Physics.Raycast(playerCamera.transform.position, direction, out hit, setRadius, exceptionLayer
                        , QueryTriggerInteraction.Ignore) && hit.transform.CompareTag(enemyTag))
        {
            return true;
        }
        return false;
    }

    #region Object Pool 
    private EnemyHPUI CreateHPUI()
    {
        EnemyHPUI enemyHPUI = Instantiate(prefabHPBar).GetComponent<EnemyHPUI>();
        enemyHPUI.SetPool(HPBarPool);
        enemyHPUI.transform.SetParent(UIManager.GetInstance.GetPriority(0));
        //enemyHPUI.Setting();

        return enemyHPUI;
    }
    private void OnGetHPUI(EnemyHPUI HPUI)
    {
        HPUI.gameObject.SetActive(true);
    }
    private void OnReleaseHPUI(EnemyHPUI HPUI)
    {
        HPUI.gameObject.SetActive(false);
    }
    private void OnDestroyHPUI(EnemyHPUI HPUI)
    {
        Destroy(HPUI.gameObject);
    }
    #endregion
}
