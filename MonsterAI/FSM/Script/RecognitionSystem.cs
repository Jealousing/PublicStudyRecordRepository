using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// ���� �÷��̾ �ν��ϴ� �ý���
/// </summary>
public class RecognitionSystem : MonoBehaviour
{
    /*
    *�ν��� �ɸ��� ����
    1) �þ� ������ ���´� -> �⺻����
    2) �νĹ����� ���´� -> �νĹ��� �ȿ� ���´ٰ� �ٷ� �ν��ϸ� ���ڿ������� 
                                     -> ��� ������ ���ʹ� ������ �ڿ�������� ����

    �þ� ������ �ν� �������� ũ��  �νĹ����� ���´ٰ� �ٷ� �ν������ʰ� 
    �ѹ� Ȯ���� �׶� �þ߹����ȿ� ������ ����
     
    *�ν��� Ǯ���� ����?
    �þ߿� �ѹ� �������ٰ� �ν��� Ǯ���°� �̻���.
    -> �þ߿� ������� �ν� �����ȿ� ��������� ��� �߰� �� ����
    -> �þ߿� �������� �νĹ����ȿ� ����� ��� ������ �þ߿��� �߰ߵǾ��� ��ġ�� �̵��ϰ� ���� �� �ٽ� �����ڸ��� ���ư���.
    -> �߰��� �ص� �����Ÿ��̻� �������� �ٽ� ���ư��� (��׷� ����)

    �νİ��� ��ũ��Ʈ�� 
    �ǽð����� �۵��ؾߵǴ� �Լ�
    1) Ÿ���� ������ �ȵǾ������� �þ߸� ��� Ž���Ѵ�. -> �þ� ������ ���� ������ 
    2) Ÿ���� �����Ǿ����� ���������� Ÿ���� Ȯ�� �Ŀ� ��� Ÿ���� ���������� �����ɰ��� �Ǵ��Ѵ�.

    ���Ǻ� �۵��ؾߵǴ� �Լ�
    1) �ν� �����ȿ� ������ �������� ȸ���ϵ��� �����ش�
    2) �ν� ���� ������ �̵����� ��� Ÿ���� ������ �þ߿��� �߰ߵǾ��ִ� ������ �����Ѵ�.


    ---- �߰� ���� ----
    1) ����� �������� : �����ϸ� �������� �þ�Ȯ�� �ʿ���� Ÿ�ټ���
    2) �߰� �������� : �þ߸� ������ ���������� Ȯ��
    3) �� ���� : �� ������ ����� Ÿ���� ����

    �������� :
    ������ �پ�ٴϸ� �ν��� Ǯ���� �� ������ �Ĵٺ����� ȸ����
    -> �ѹ� ���� �ν��ߴٸ� ��� Ÿ���� ������������� Ÿ���� �ٶ󺻴�?
     */


    // ��Ž�� ��ũ��Ʈ�� �����ش�
    EnemySearch enemy;
    // ������
    Transform watcher;

    // ������ Ÿ���� ����
    public Transform target;
    // �ݶ��̴� ����
    CapsuleCollider capsuleCollider;
    // �÷��̾� string
    string playerName = "Player";

    // �þ߰�, �þ߰Ÿ�, ���󰡴� �Ÿ� ����, �÷��̾� ����ũ�� �÷��̾� �����ִ� ������Ʈ�� ����ũ
    [SerializeField] float viewAngle;
    [SerializeField] float viewDistance;
    [SerializeField] [Range(1.0f, 2.0f)] float followDistanceScale;
    [SerializeField] LayerMask playerMask;
    private LayerMask exceptionLayer;

    // �̵� �ִ� �ݰ� ����
    [SerializeField] float moveLimitDistance;
    //[SerializeField] LayerMask notPlayerHelpful;

    // ����� ����(������)
    public bool IsGizmos = true;

    // �þ� �׷��ִ� ��ũ��Ʈ
    bool isDrawViewScript;
    DonutShapeMesh drawViewShape;

    // �÷����� 255/255
    Color greenColor = new Color(0, 1, 0, 0.05f);
    Color redColor = new Color(1, 0, 0, 0.05f);

    // flag
    bool isCheckTarget = false;
    bool isStart = false;
    bool returnPosFlag = false;

    // �ϻ����
    /*
     ������ ��� �������
     �߰� ���� ���Խ� �þ�Ȯ�� -> ���� x
     �þ߿� ��������� ������ ���·� ����� ���� ���� ���Խ� �ٷ� Ÿ�� -> Ÿ�ټ���x
     ���࿡ �þ߿� ����� ��� ������ true���� ��� false�� �ٲ��� -> ����Ÿ�ټ�������

     �־����� �þ� �ν��� Ǯ����  true ���� ��� �ٽ� �ϻ찡�ɻ��·κ���
     */
    public bool allowsAssassinates = false;
    public bool curAllowsAssassinates = false;

    /* ����Ƽ �⺻ �̺�Ʈ �Լ� */
    private void Start()
    {
        enemy = GetComponentInParent<EnemySearch>();
        
        capsuleCollider = GetComponent<CapsuleCollider>();
        curAllowsAssassinates = allowsAssassinates;
        // ���࿡ ���� �׷��ַ��� ��ũ��Ʈ�� �߰� ������쿡�� ����
        if (isDrawViewScript = TryGetComponent<DonutShapeMesh>(out drawViewShape))
        {
            drawViewShape.minDistance = 2.0f;
            drawViewShape.maxDistance = viewDistance;
            drawViewShape.shapeAngle = viewAngle/2;
        }

        isStart = true;
        // ���̾� �ΰ��� ������ ���̾ �����ϴ¹�
        //int layerMask = ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerHelpful")));
        // �ش� ���̾� ���� ����
        //notPlayerHelpful = ~notPlayerHelpful;
        exceptionLayer = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        if (enemy.Head != null)
            watcher = enemy.Head;
        else
            watcher = this.transform;

    }

    // fill�� ����
    float tickTimer = 0.01f;
    float timer = 0.0f;

    // ���� �������� �Ӹ��� ������ �����Ͽ� �׸���
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
        // �þ�üũ
        viewSearch();
        DrawViewField();

        // Ÿ��Ȯ��
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
    /// �������� ������ ���� stay���� ����
    /// �ش� ������Ʈ�� ������Ʈ �ȵǸ� ���� x
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
            // �þ߰� ���� �� �ִ� �Ÿ�
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, viewDistance);

            // �ν� ���� �Ÿ�
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, viewDistance * followDistanceScale);

            // ������ �� �ִ� �ִ�Ÿ�
            Gizmos.color = Color.black;
            if(isStart)
                Gizmos.DrawWireSphere(enemy.returnPos, moveLimitDistance);

            //�þ� ���� �׷��ֱ�
            if (!isDrawViewScript)
            {
                Handles.color = isCheckTarget ? redColor : greenColor;
                Handles.DrawSolidArc(this.transform.position, this.transform.up,
                    AngleToDir(-viewAngle * 0.5f), viewAngle, viewDistance);
            }
        }
    }

    /// <summary>
    ///  ������ ���⺤�ͷ� �ٲ��ִ� �Լ�
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

    // �þ� üũ
    void viewSearch()
    {
        if (returnPosFlag)
        {
            if (Vector3.Distance(enemy.transform.position, enemy.returnPos) < 0.25f)
                returnPosFlag = false;
            return;
        }
        if (target != null) return;
        // �þ� ���⺤��
        //Vector3 leftView = AngleToDir(-viewAngle * 0.5f);
        //Vector3 rightView = AngleToDir(viewAngle * 0.5f);

        // �þ� Ȯ�ο�
        if(IsGizmos)
        {
            Debug.DrawRay(watcher.position + watcher.up, transform.forward * viewDistance, Color.blue);
        }

        // �������� �ִ� Ư�� ���̾��ũ�� ���� ������Ʈ Ž��
        Collider[] targetCollider = Physics.OverlapSphere(transform.position, viewDistance, playerMask);

        // Ž���� �ݶ��̴� ��ŭ �ݺ�
        for (int i= 0; i<targetCollider.Length; i++)
        {
            Transform temp = targetCollider[i].transform;
            if(temp.gameObject.name ==  playerName)
            {
                // ���� �� ����
                Vector3 direction = (temp.position - watcher.position).normalized;
                float angle = Vector3.Angle(direction, watcher.forward);
                
                // �þ� ���� ���� �ִ��� Ȯ��
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

    // Ÿ�� Ȯ�� �Լ�
    bool CheckTarget()
    {
        if(target)
        {
            if(moveLimitDistance<Vector3.Distance(enemy.transform.position,enemy.returnPos))
            {
                returnPosFlag = true;
               return false;
            }

            // ���� �Ÿ�
            float distance = Vector3.Distance(watcher.position, target.position);

            // ����� ���� ������ ���
            if (distance < capsuleCollider.radius / 2)
            {
                return true;
            }

            // Ÿ���� �Ÿ��� ���󰥼� �ִ� �Ÿ����� �־������� Ȯ��
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
