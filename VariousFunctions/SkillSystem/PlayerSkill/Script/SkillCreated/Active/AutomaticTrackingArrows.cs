using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ȱ ��ų�� �� ����, ȭ���� �ڵ����� ���͸� �����ϴ� ��Ƽ�� ��ų.
/// </summary>
public class AutomaticTrackingArrows : ActiveSkill
{
    public ArrowController[] arrow = new ArrowController[5];
    BowCombat bowinfo;

    int maxArrowNumber = 5;
    public int addMaxArrowNumber = 0;
    int waitingArrowCount = 0;
    float Range = 10.0f;
    IEnumerator Repetition;
    Vector3 playerRightDir;
    float chargingTimer;
    float chargingTime =2.0f;
    public int chargingArrowNumber;

    Vector3[] arrowStartPos = new Vector3[5];
    private float[] journeyLength = new float[5];
    private float[] randArcHeight = new float[5];

    private Vector3[] localPosData = new Vector3[]
        {
            new Vector3( 0,0.35f,-0.2f),
            new Vector3( -0.1f,0.25f,-0.2f),
            new Vector3( 0.1f,0.25f,-0.2f),
            new Vector3( -0.2f,0.15f,-0.2f),
            new Vector3( 0.2f,0.15f,-0.2f),
            new Vector3( -0.3f,0.05f,-0.2f),
            new Vector3( 0.3f,0.05f,-0.2f),

            new Vector3( 0, 0.25f,-0.25f),
            new Vector3( -0.1f,0.15f,-0.25f),
            new Vector3( 0.1f,0.15f,-0.25f),
            new Vector3( -0.2f,0.05f,-0.25f),
            new Vector3( 0.2f,0.05f,-0.25f),
        };
    private float[] horizontalOffset = new float[]
       {
            0,-1,1,-3,3,-5,5, -2,2,-4,4,-6,6
       };

    public override void Setting()
    {
        bowinfo = (BowCombat)PlayerInfo.GetInstance.combatInfo.curScript;
        Repetition = RepetitionArrow();

        if(assistSkills !=null)
        {
            for(int i=0;i<assistSkills.Length;i++)
            {
                assistSkills[i].skillInfo.skillScript.GetComponent<AssistSkill>().Assist(this);
            }
        }
    }

    public void ResetArraySizeArrow()
    {
        for (int i = 0; i < arrow.Length; i++)
        {
            if (arrow[i] != null)
            {
                arrow[i].Remove();
            }
        }
        arrow = new ArrowController[maxArrowNumber + addMaxArrowNumber];
        arrowStartPos = new Vector3[maxArrowNumber + addMaxArrowNumber];
        journeyLength = new float[maxArrowNumber + addMaxArrowNumber];
        randArcHeight = new float[maxArrowNumber + addMaxArrowNumber];
}

    public override void CheckRange()
    {
    }

    public override void ResetRange()
    {
    }

    public override void Activate()
    {
        if (!isToggle)
        {
            StartCoroutine(SkillCooldownTimer());
            StartCoroutine(UseActiveSkill());
            StartCoroutine(Repetition);
            SetLinkSkill(isToggle);
        }
        else
        {
            StartCoroutine(SkillCooldownTimer());
            StopCoroutine(Repetition);
            SetLinkSkill(isToggle);

            Deactivate();
        }
        isToggle = !isToggle;
    }

    void SetLinkSkill(bool toggle)
    {
        SkillQuickSlot[] skillQuickSlots = UIManager.GetInstance.QuickSlot.SkillSlot.skillSlot;
        int maxCount = skillQuickSlots.Length;

        for (int i=0;i<maxCount;i++)
        {
            if (skillQuickSlots[i].quickSlotSkillInfo.skillScript !=null &&
                skillQuickSlots[i].activeSkill.skillData.skillInfo.skillName != 
                this.skillData.skillInfo.skillName)
            {
                if (toggle) skillQuickSlots[i].activeSkill.LinkedSkillEvent -= AddWaitingArrow;
                else skillQuickSlots[i].activeSkill.LinkedSkillEvent += AddWaitingArrow;
            }
        }
    }

    void AddWaitingArrow(int count)
    {
        waitingArrowCount += count;
        if(waitingArrowCount>maxArrowNumber+addMaxArrowNumber)
        {
            waitingArrowCount = maxArrowNumber + addMaxArrowNumber;
        }
    }

    public override void Deactivate()
    {
        for(int i=0;i< maxArrowNumber+ addMaxArrowNumber; i++)
        {
            if (arrow[i] !=null)
            {
                arrow[i].Remove();
            }
        }
    }

    IEnumerator RepetitionArrow()
    {
        while(true)
        {
            yield return null;

            // �ð��� ������ ���� �ڵ�����
            chargingTimer+=Time.deltaTime;
            if(chargingTimer >= chargingTime)
            {
                chargingTimer = 0;
                AddWaitingArrow(chargingArrowNumber);
            }

            // �ٸ���ų�� ����ؼ� ������ ������ �ִ��� Ȯ�� ������ ����
            if (waitingArrowCount>0)
            {
                int maxCount = waitingArrowCount;
                for(int i=0;i< maxCount; i++)
                {
                    if (i >= arrow.Length) break;

                    for(int j=0; j< arrow.Length; j++)
                    {
                        if (arrow[j] == null)
                        {
                            arrow[j] = bowinfo.arrowPool.Get();
                            arrow[j].automaticTrackingInfo = this;
                            arrow[j].automaticTrackingNumber = j;
                            arrow[j].IsTrigger = false;
                            arrow[j].transform.parent = PlayerInfo.GetInstance.wearPoint.transform;
                            arrow[j].transform.localPosition = localPosData[j];
                            arrow[j].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

                            arrow[j].trailRenderer.enabled = false;
                            arrow[j].arrowRigidbody.isKinematic = true;
                            arrow[j].arrowRigidbody.freezeRotation = true;

                            waitingArrowCount--;
                            break;
                        }
                    }

                   
                }
            }


            // ȭ���� �ִ��� Ȯ��
            bool IsArrow = false;
            for(int i = 0; i < arrow.Length; i++)
            {
                if (arrow[i] != null && arrow[i].gameObject.activeSelf)
                {
                    IsArrow = true;

                    // ������ Ÿ���� �ִ��� Ȯ��
                    if(arrow[i].Target ==null)
                    {
                        // �÷��̾� ��ó�� Ÿ���� �ɸ��Ѱ� �ִ��� Ȯ��
                        Collider[] colliders = Physics.OverlapSphere(PlayerInfo.GetInstance.transform.position, Range);

                        // ���� Ÿ���� ���� ���
                        float[] priorities = new float[colliders.Length];
                        for (int j = 0; j < colliders.Length; j++)
                        {
                            priorities[j] = Random.Range(0f, 1f);
                        }
                        System.Array.Sort(priorities, colliders);

                        // Ÿ�� ����
                        foreach (Collider collider in colliders)
                        {
                            if (collider.CompareTag("Enemy"))
                            {
                                arrow[i].Target = collider.gameObject;
                                arrowStartPos[i] = arrow[i].transform.position;
                                journeyLength[i] = Vector3.Distance(arrowStartPos[i], arrow[i].Target.transform.position);
                                randArcHeight[i] = Random.Range(3.0f, 4.0f);
                                playerRightDir = PlayerInfo.GetInstance.transform.right;
                                arrow[i].trailRenderer.enabled = true;
                                arrow[i].arrowRigidbody.isKinematic = false;
                                arrow[i].arrowRigidbody.freezeRotation = true;
                                arrow[i].transform.parent = null;
                                break;
                            }
                        }
                    }
                }
            }

            if (IsArrow)
            {
                for (int i = 0; i < arrow.Length; i++)
                {
                    // Ÿ���� �ִ� ȭ���̸� Ÿ�� ����
                    if (arrow[i] != null && arrow[i].enabled && arrow[i].Target != null)
                    {
                        float fracJourney = arrow[i].count /20.0f;
                        Vector3 currentPos = Vector3.Slerp(arrowStartPos[i], arrow[i].Target.transform.position,
                            fracJourney);
                        float Offset = Mathf.Sin(fracJourney * Mathf.PI) * horizontalOffset[i];
                        currentPos += playerRightDir * Offset;
                        currentPos.y += Mathf.Sin(fracJourney * Mathf.PI) * randArcHeight[i];

                        arrow[i].count += 0.1f;
                        if (arrow[i].count >10.0f)
                        {
                            arrow[i].IsTrigger = true;
                        }

                        arrow[i].arrowRigidbody.MovePosition(currentPos);
                        arrow[i].arrowRigidbody.MoveRotation(Quaternion.LookRotation(arrow[i].transform.position 
                            - currentPos) * Quaternion.Euler(new Vector3(0, 90, 0)));
                    }
                }
            }
        }
    }

    IEnumerator UseActiveSkill()
    {
        // �Է� ����
        PlayerInfo.GetInstance.movementInfo.inputLock = true;
     
        // ��ų ������ ����
        skillDamage = skillData.skillInfo.skillLV * PlayerInfo.GetInstance.LV * 250.0f;

        yield return new WaitForSeconds(0.05f);

        for(int i=0;i< arrow.Length; i++)
        {
            // ����ְų� ��Ȱ��ȭ�ϰ�� ȭ�� ����
            if (arrow[i] ==null || !arrow[i].gameObject.activeSelf)
            {
                // ȭ�� ���� �� ���
                arrow[i] = bowinfo.arrowPool.Get();
                arrow[i].automaticTrackingInfo = this;
                arrow[i].automaticTrackingNumber= i;
                arrow[i].IsTrigger = false;
                arrow[i].transform.parent = PlayerInfo.GetInstance.wearPoint.transform;
                arrow[i].transform.localPosition = localPosData[i];
                arrow[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

                arrow[i].trailRenderer.enabled = false;
                arrow[i].arrowRigidbody.isKinematic = true;
                arrow[i].arrowRigidbody.freezeRotation = true;
            }
        }

        yield return new WaitForSeconds(0.05f);
        PlayerInfo.GetInstance.movementInfo.inputLock = false;
    }

}
