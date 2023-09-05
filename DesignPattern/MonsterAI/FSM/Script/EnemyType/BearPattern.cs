using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearPattern : PatternInfo
{
    int aniHashBasicAttack;
    string aniName;
    string aniBasicAttackName = "BasicAttack";
    int basicAttackCnt = 0;
    int maxBasicAttackCnt = 3;
    EnemyInfo enemyInfo;

    private void Start()
    {
        enemyInfo = GetComponent<EnemyInfo>();
        aniHashBasicAttack = Animator.StringToHash("BasicAttackTrigger");
    }

    [SerializeField] bool viewRangeBasicAttack;
    Damage tempDamage;

    
    private void Update()
    {
    #if UNITY_EDITOR
        if (viewRangeBasicAttack && !tempDamage)
        {   
            viewRangeBasicAttack = false;
            StartCoroutine(BasicAttackCoroutine(true));

        }
#endif
    }

    public override bool BasicAttack()
    {
        if (IsPattern) return false;

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position +this.transform.forward*0.5f, enemyInfo.attackDistance );
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                StartCoroutine(BasicAttackCoroutine(false));
                IsPattern = true;
                return true;
            }
        }
        return false;
    }

    IEnumerator BasicAttackCoroutine(bool isDraw)
    {
        enemyInfo.animator.SetTrigger(aniHashBasicAttack);
        aniName = aniBasicAttackName + basicAttackCnt;
        // 실행대기
        while (!enemyInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniName))
        {
            yield return null;
        }
       
        while (enemyInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniName) &&
            enemyInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.55f)
        {
            yield return null;
        }
        basicAttackCnt++;
        if (basicAttackCnt >= maxBasicAttackCnt)
            basicAttackCnt = 0;

        tempDamage = DamageManager.GetInstance.CallDamageRange(
            DamageRangeType.LIMITEDANGLEDONUT, isDraw, 0, 30, LayerMask.GetMask("Player")
                    , this.transform.position + this.transform.forward * 0.5f,new Vector3(0,90,0), -0.5f, enemyInfo.attackDistance+1, 60);


        while (enemyInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniName) &&
            enemyInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <0.8f)
        {
            yield return null;
        }
        if(isDraw) tempDamage.StopDraw();
        tempDamage = null;

        if (enemyInfo.target && Vector3.Distance(this.transform.position, enemyInfo.target.transform.position) > enemyInfo.attackDistance)
        {
            basicAttackCnt = 0;
            IsPattern = false;
        }
        else if(isDraw && basicAttackCnt == maxBasicAttackCnt-1)
        {
            basicAttackCnt = 0;
        }
        else
        {
            StartCoroutine(BasicAttackCoroutine(isDraw));
        }
      
    }

    public override bool CooldownPattern()
    {
        if (IsPattern) return true;

        if (waitCooldownPattern.Count > 0)
        {
            var node = waitCooldownPattern.First;
            int randomIndex = Random.Range(0, waitCooldownPattern.Count);
            for (int i = 0; i < randomIndex; i++)
            {
                node = node.Next;
            }
            StartCoroutine(node.Value);
            waitCooldownPattern.Remove(node);
            IsPattern = true;
            return true;
        }
        return false;
    }

    public override bool HpPattern()
    {
        if (IsPattern) return true;

        if (waitHpPattern.Count > 0)
        {
            StartCoroutine(waitHpPattern.Dequeue());
            IsPattern =true; 
            return true;
        }
        return false;
    }

    public override void HpCheck()
    {

    }

}
