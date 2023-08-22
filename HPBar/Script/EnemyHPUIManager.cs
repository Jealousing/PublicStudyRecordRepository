using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHPUIManager : MonoBehaviour
{
    /*
     카메라 시야 전방 일정 각도에 보이는 적의 머리에 HP Bar을 보이게 해주는 스크립트 작성 계획.

     생성조건:
     1) 카메라기준으로 forward vector 방향기준으로 일정 각도에 포함된 몬스터의 HPBar를 
         오브젝트 풀링을 이용해서 HPBar를 생성해준다.
    
     각도에 벗어나도 HPBar가 고정되는 조건은 추후에 설정:
     1)  몬스터의 타겟이 자신이 되었을 경우 
     2) 플레이어가 타겟을 고정시킨 몬스터면 고정
     
    사라지는 조건:
    1)  시야 각도에 벗어남
    2) 몬스터와 플레이어 사이에 장애물이 있으면
    3) 몬스터가 사라지거나 사망했을 경우 
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
        // 오브젝트 풀 생성
        HPBarPool = new ObjectPool<EnemyHPUI>(CreateHPUI, OnGetHPUI, OnReleaseHPUI, OnDestroyHPUI, maxSize: 30);
    }

    void Start()
    {
        // 예외 레이어 설정
        exceptionLayer = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player"));
    }

    private void Update()
    {
        // 일정 범위의 몬스터의 콜라이더를 탐지한다
        Collider[] colliders = Physics.OverlapSphere(playerTr.position, setRadius, enemyMask,QueryTriggerInteraction.Ignore);

        for( int i=0; i<colliders.Length;i++)
        {
            if (colliders[i].CompareTag(enemyTag))
            {
                Vector3 temp = colliders[i].transform.position;
                EnemyInfo enemy;
                // 이미 HPBar를 보유하고있는지 확인
                if ((enemy = colliders[i].GetComponent<EnemyInfo>()).HPUI==null)
                {
                    // 적과 플레이어 사이에 방해물이 있는지 확인
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
