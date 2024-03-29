using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 적이 플레이어를 인식하는 시스템
/// </summary>
public class RecognitionSystem : MonoBehaviour
{
    /*
    *인식이 걸리는 조건
    1) 시야 범위에 들어온다 -> 기본조건
    2) 인식범위에 들어온다 -> 인식범위 안에 들어온다고 바로 인식하면 부자연스러움 
                                     -> 대신 보스형 몬스터는 오히려 자연스러울수 있음

    시야 범위는 인식 범위보다 크고  인식범위에 들어온다고 바로 인식하지않고 
    한번 확인후 그때 시야범위안에 들어오면 추적
     
    *인식이 풀리는 조건?
    시야에 한번 없어졌다고 인식이 풀리는건 이상함.
    -> 시야에 없어졌어도 인식 범위안에 들어있으면 계속 추격 및 전투
    -> 시야에 없어지고 인식범위안에 벗어났을 경우 마지막 시야에서 발견되었던 위치로 이동하고 수색 후 다시 원래자리로 돌아간다.
    -> 추격을 해도 일정거리이상 벌어지면 다시 돌아간다 (어그로 해제)

    인식관련 스크립트는 
    실시간으로 작동해야되는 함수
    1) 타겟이 설정이 안되어있으면 시야를 계속 탐색한다. -> 시야 범위내 적이 들어오면 
    2) 타겟이 설정되었으면 지속적으로 타겟을 확인 후에 계속 타겟이 지속적으로 유지될건지 판단한다.

    조건부 작동해야되는 함수
    1) 인식 범위안에 들어오면 그쪽으로 회전하도록 도와준다
    2) 인식 범위 밖으로 이동했을 경우 타겟을 마지막 시야에서 발견되어있던 곳으로 수정한다.


    ---- 추가 생각 ----
    1) 가까운 근접범위 : 근접하면 그쪽으로 시야확인 필요없이 타겟설정
    2) 중간 근접범위 : 시야를 돌려서 지속적으로 확인
    3) 먼 범위 : 이 범위를 벗어나면 타겟을 해제

    문제사항 :
    가깝게 뛰어다니면 인식이 풀리고 그 방향을 쳐다볼려고 회전함
    -> 한번 적을 인식했다면 모든 타겟이 사라질때까지는 타겟을 바라본다?
     */


    // 적탐색 스크립트를 도와준다
    EnemySearch enemy;
    // 관측자
    Transform watcher;

    // 설정된 타겟의 정보
    public Transform target;
    // 콜라이더 정보
    CapsuleCollider capsuleCollider;
    // 플레이어 string
    string playerName = "Player";

    // 시야각, 시야거리, 따라가는 거리 배율, 플레이어 마스크와 플레이어 도와주는 오브젝트의 마스크
    [SerializeField] float viewAngle;
    [SerializeField] float viewDistance;
    [SerializeField] [Range(1.0f, 2.0f)] float followDistanceScale;
    [SerializeField] LayerMask playerMask;
    private LayerMask exceptionLayer;

    // 이동 최대 반경 설정
    [SerializeField] float moveLimitDistance;
    //[SerializeField] LayerMask notPlayerHelpful;

    // 기즈모 여부(에디터)
    public bool IsGizmos = true;

    // 시야 그려주는 스크립트
    bool isDrawViewScript;
    DonutShapeMesh drawViewShape;

    // 컬러설정 255/255
    Color greenColor = new Color(0, 1, 0, 0.05f);
    Color redColor = new Color(1, 0, 0, 0.05f);

    // flag
    bool isCheckTarget = false;
    bool isStart = false;
    bool returnPosFlag = false;

    // 암살허용
    /*
     적용할 경우 변경사항
     중간 범위 진입시 시야확인 -> 적용 x
     시야에 들었던적이 없었던 상태로 가까운 근접 범위 진입시 바로 타겟 -> 타겟설정x
     만약에 시야에 들었을 경우 변수가 true였을 경우 false로 바꿔줌 -> 근접타겟설정가능

     멀어져서 시야 인식이 풀리면  true 였을 경우 다시 암살가능상태로변경
     */
    public bool allowsAssassinates = false;
    public bool curAllowsAssassinates = false;

    /* 유니티 기본 이벤트 함수 */
    private void Start()
    {
        enemy = GetComponentInParent<EnemySearch>();
        
        capsuleCollider = GetComponent<CapsuleCollider>();
        curAllowsAssassinates = allowsAssassinates;
        // 만약에 따로 그려주려고 스크립트를 추가 했을경우에만 적용
        if (isDrawViewScript = TryGetComponent<DonutShapeMesh>(out drawViewShape))
        {
            drawViewShape.minDistance = 2.0f;
            drawViewShape.maxDistance = viewDistance;
            drawViewShape.shapeAngle = viewAngle/2;
        }

        isStart = true;
        // 레이어 두개를 제외한 레이어를 설정하는법
        //int layerMask = ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerHelpful")));
        // 해당 레이어 제외 전부
        //notPlayerHelpful = ~notPlayerHelpful;
        exceptionLayer = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        if (enemy.Head != null)
            watcher = enemy.Head;
        else
            watcher = this.transform;

    }

    // fill용 변수
    float tickTimer = 0.01f;
    float timer = 0.0f;

    // 몸을 기준으로 머리를 각도를 참고하여 그리기
    void DrawViewField()
    {
        if(isDrawViewScript && enemy.enemyInfo.stateSystem.GetCurrentEnemyState() == EnemyState.Search)
        {
            drawViewShape.DrawShape(this.transform.position , watcher.transform.forward);

            if(enemy.enemyInfo.target != null)
            {
                drawViewShape.fillProgress = 1.0f;
            }
            else if (tickTimer > timer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0.0f;
                drawViewShape.fillProgress += 0.01f;
                if (drawViewShape.fillProgress >= 1.0f)
                {
                    drawViewShape.fillProgress = 0;
                }
            }
        }
    }

    private void Update()
    {
        // 시야체크
        viewSearch();
        DrawViewField();

        // 타겟확인
        if (CheckTarget())
        {
            angleCheck();
            enemy.enemyInfo.target = this.target.GetComponent<BasicInfo>();
            isCheckTarget = true;
        }
        else
        {
            isCheckTarget = false;
            enemy.enemyInfo.target = null;
            enemy.target = null;
            target = null;
        }

    }

    void angleCheck()
    {
        float angle = Vector3.Angle(enemy.transform.forward, target.position);
        if(angle > viewAngle*0.5f)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(target.position), 0.1f);
        }
    }

    /// <summary>
    /// 지속적인 감지를 위한 stay에서 감지
    /// 해당 오브젝트가 업데이트 안되면 감지 x
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerName) && !returnPosFlag)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < capsuleCollider.radius / 2 && !curAllowsAssassinates)
            {
                target = other.transform;
            }
            else if (enemy.enemyInfo.target == null && !enemy.isFeelThePresence && !curAllowsAssassinates)
            {
                StartCoroutine(enemy.FeelThePresence(other.transform.position));
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(IsGizmos)
        {
            // 시야가 닿을 수 있는 거리
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, viewDistance);

            // 인식 해제 거리
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, viewDistance * followDistanceScale);

            // 움직일 수 있는 최대거리
            Gizmos.color = Color.black;
            if(isStart)
                Gizmos.DrawWireSphere(enemy.returnPos, moveLimitDistance);

            //시야 범위 그려주기
            if (!isDrawViewScript)
            {
                Handles.color = isCheckTarget ? redColor : greenColor;
                Handles.DrawSolidArc(this.transform.position, this.transform.up,
                    AngleToDir(-viewAngle * 0.5f), viewAngle, viewDistance);
            }
        }
    }

    /// <summary>
    ///  각도를 방향벡터로 바꿔주는 함수
    /// </summary>
    Vector3 AngleToDir(float angle)
    {
        Transform standard;
        if (isStart)
        {
            standard = watcher;
        }
        else
        {
            standard = this.transform;
        }
        float radian = (angle + standard.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    // 시야 체크
    void viewSearch()
    {
        if (returnPosFlag)
        {
            if (Vector3.Distance(enemy.transform.position, enemy.returnPos) < 0.25f)
                returnPosFlag = false;
            return;
        }
        if (target != null) return;
        // 시야 방향벡터
        //Vector3 leftView = AngleToDir(-viewAngle * 0.5f);
        //Vector3 rightView = AngleToDir(viewAngle * 0.5f);

        // 시야 확인용
        if(IsGizmos)
        {
            Debug.DrawRay(watcher.position + watcher.up, transform.forward * viewDistance, Color.blue);
        }

        // 범위내에 있는 특정 레이어마스크를 가진 오브젝트 탐지
        Collider[] targetCollider = Physics.OverlapSphere(transform.position, viewDistance, playerMask);

        // 탐지된 콜라이더 만큼 반복
        for (int i= 0; i<targetCollider.Length; i++)
        {
            Transform temp = targetCollider[i].transform;
            if(temp.gameObject.name ==  playerName)
            {
                // 방향 및 각도
                Vector3 direction = (temp.position - watcher.position).normalized;
                float angle = Vector3.Angle(direction, watcher.forward);
                
                // 시야 각도 내에 있는지 확인
                if(angle<viewAngle*0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(watcher.position+transform.up,direction,out hit,viewDistance, exceptionLayer,QueryTriggerInteraction.Ignore)
                        &&hit.transform.CompareTag(playerName))
                    {
                        target = hit.transform;

                        if (curAllowsAssassinates)
                            curAllowsAssassinates = false;

                        if (IsGizmos)
                            Debug.DrawRay(watcher.position + transform.up, direction * hit.distance, Color.green);
                    }
                }    
            }
        }
    }

    // 타겟 확인 함수
    bool CheckTarget()
    {
        if(target)
        {
            if(moveLimitDistance<Vector3.Distance(enemy.transform.position,enemy.returnPos))
            {
                returnPosFlag = true;
               return false;
            }

            // 사이 거리
            float distance = Vector3.Distance(watcher.position, target.position);

            // 가까운 곳에 존재할 경우
            if (distance < capsuleCollider.radius / 2)
            {
                return true;
            }

            // 타겟의 거리가 따라갈수 있는 거리보다 멀어졌는지 확인
            if (viewDistance*followDistanceScale < distance)
            {
                if(allowsAssassinates)
                    curAllowsAssassinates = allowsAssassinates;

                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
