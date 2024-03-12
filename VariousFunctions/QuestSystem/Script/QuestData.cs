using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ʈ Ÿ��
public enum QuestType
{
    // ���� óġ
    KillMonster,
    // ������ ����
    CollectItem,
    // ������ġ �̵��ϱ�
    MoveToLocation,
    // ��ȭ�ϱ�
    TalkToNPC
}

public abstract class Quest
{
   public QuestType questType;

    // �Ϸ�Ȯ��
    public bool IsCompleted;

    // ����Ʈ ���� �� ����
    public string questTitle;
    public string questDescription;
    public int questNumber;

    // ������̸�, ������ġ
    public string questScene;
    public Vector3 questPos;

    // ����
    public List<Item> rewardItems;
    public int rewardEXP;

    // ��������Ʈ
    public bool IsLinkQuest;

    public abstract bool UpdateProgress(int amount = 1);

}

// ���͸� óġ�ϴ� ����Ʈ Ŭ����
public class KillMonsterQuest : Quest
{
    private int currentKillCount;
    private int targetKillCount;

    public KillMonsterQuest(string questTitle, int targetKillCount)
    {
        this.questTitle = questTitle;
        questType = QuestType.KillMonster;
        this.targetKillCount = targetKillCount;
    }
    public override bool UpdateProgress(int amount = 1)
    {
        // ����Ʈ ���� ��Ȳ ���� ���� �ۼ�
        currentKillCount+= amount; // ���͸� óġ�� ������ ȣ��Ǿ�� ��

        Debug.Log(questTitle + " ���� ��Ȳ: " + currentKillCount + "/" + targetKillCount);

        if (currentKillCount >= targetKillCount)
        {
            IsCompleted = true;
            Debug.Log(questTitle + " ����Ʈ �Ϸ�!");
            // �����߰��ؾߵ�

            return true;
        }
        return false;
    }
}

// �������� �����ϴ� ����Ʈ Ŭ����
public class CollectItemQuest : Quest
{
    private int currentItemCount;
    private int targetItemCount;
    private int itemID; // �����ؾ� �� �������� ID

    public CollectItemQuest(string questTitle, int targetItemCount, int itemID)
    {
        this.questTitle = questTitle;
        questType = QuestType.CollectItem;
        this.targetItemCount = targetItemCount;
        this.itemID = itemID;
    }

    public override bool UpdateProgress(int amount)
    {
        // ����Ʈ ���� ��Ȳ ���� ���� �ۼ�
        currentItemCount = amount; // �������� ������ ������ ȣ��Ǿ�� ��

        Debug.Log(questTitle + " ���� ��Ȳ: " + currentItemCount + "/" + targetItemCount);

        if (currentItemCount >= targetItemCount)
        {
            IsCompleted = true;
            Debug.Log(questTitle + " ����Ʈ �Ϸ�!");
            return true;
        }
        return false;
    }
}

// ��ġ�� �̵��ϴ� ����Ʈ Ŭ����
public class MoveToLocationQuest : Quest
{
    private Vector3 targetLocation; 
    private float completionDistance = 5f; // ��ǥ ��ġ�� �����ߴٰ� �Ǵ��� �Ÿ�

    public MoveToLocationQuest(string questTitle, Vector3 targetLocation)
    {
        this.questTitle = questTitle;
        questType = QuestType.MoveToLocation;
        this.targetLocation = targetLocation;
    }

    public override bool UpdateProgress(int amount = 1)
    {
        // �÷��̾�� ��ǥ ��ġ ������ �Ÿ� ���
        float distanceToTarget = Vector3.Distance(PlayerInfo.GetInstance.transform.position, targetLocation);

        if (distanceToTarget <= completionDistance)
        {
            IsCompleted = true;
            Debug.Log(questTitle + " ����Ʈ �Ϸ�!");
            return true;
        }
        return false;
    }
}

// NPC�� ��ȭ�ϴ� ����Ʈ Ŭ����
public class TalkToNPCQuest : Quest
{
    public string targetNpcNumber;
    public string targetNpcName;
    public int eventNumber;

    public TalkToNPCQuest(string questTitle, string questScene, Vector3 questPos, string targetNpcNumber,string targetNpcName,int eventNumber)
    {
        this.questTitle = questTitle;
        this.questScene = questScene;
        this.questPos = questPos;
        questType = QuestType.TalkToNPC;
        questDescription = "Go to " + targetNpcName;
        this.targetNpcNumber = targetNpcNumber;
        this.targetNpcName = targetNpcName;
        this.eventNumber = eventNumber;
    }

    public override bool UpdateProgress(int amount = 1)
    {
        IsCompleted = true;
        Debug.Log(questTitle + " ����Ʈ �Ϸ�!");
        return true;
    }
}

