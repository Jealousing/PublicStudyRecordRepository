using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 타입
public enum QuestType
{
    // 몬스터 처치
    KillMonster,
    // 아이템 수집
    CollectItem,
    // 지정위치 이동하기
    MoveToLocation,
    // 대화하기
    TalkToNPC
}

public abstract class Quest
{
   public QuestType questType;

    // 완료확인
    public bool IsCompleted;

    // 퀘스트 제목 및 설명
    public string questTitle;
    public string questDescription;
    public int questNumber;

    // 진행씬이름, 진행위치
    public string questScene;
    public Vector3 questPos;

    // 보상
    public List<Item> rewardItems;
    public int rewardEXP;

    // 다음퀘스트
    public bool IsLinkQuest;

    public abstract bool UpdateProgress(int amount = 1);

}

// 몬스터를 처치하는 퀘스트 클래스
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
        // 퀘스트 진행 상황 갱신 로직 작성
        currentKillCount+= amount; // 몬스터를 처치할 때마다 호출되어야 함

        Debug.Log(questTitle + " 진행 상황: " + currentKillCount + "/" + targetKillCount);

        if (currentKillCount >= targetKillCount)
        {
            IsCompleted = true;
            Debug.Log(questTitle + " 퀘스트 완료!");
            // 보상추가해야됨

            return true;
        }
        return false;
    }
}

// 아이템을 수집하는 퀘스트 클래스
public class CollectItemQuest : Quest
{
    private int currentItemCount;
    private int targetItemCount;
    private int itemID; // 수집해야 할 아이템의 ID

    public CollectItemQuest(string questTitle, int targetItemCount, int itemID)
    {
        this.questTitle = questTitle;
        questType = QuestType.CollectItem;
        this.targetItemCount = targetItemCount;
        this.itemID = itemID;
    }

    public override bool UpdateProgress(int amount)
    {
        // 퀘스트 진행 상황 갱신 로직 작성
        currentItemCount = amount; // 아이템을 수집할 때마다 호출되어야 함

        Debug.Log(questTitle + " 진행 상황: " + currentItemCount + "/" + targetItemCount);

        if (currentItemCount >= targetItemCount)
        {
            IsCompleted = true;
            Debug.Log(questTitle + " 퀘스트 완료!");
            return true;
        }
        return false;
    }
}

// 위치로 이동하는 퀘스트 클래스
public class MoveToLocationQuest : Quest
{
    private Vector3 targetLocation; 
    private float completionDistance = 5f; // 목표 위치에 도달했다고 판단할 거리

    public MoveToLocationQuest(string questTitle, Vector3 targetLocation)
    {
        this.questTitle = questTitle;
        questType = QuestType.MoveToLocation;
        this.targetLocation = targetLocation;
    }

    public override bool UpdateProgress(int amount = 1)
    {
        // 플레이어와 목표 위치 사이의 거리 계산
        float distanceToTarget = Vector3.Distance(PlayerInfo.GetInstance.transform.position, targetLocation);

        if (distanceToTarget <= completionDistance)
        {
            IsCompleted = true;
            Debug.Log(questTitle + " 퀘스트 완료!");
            return true;
        }
        return false;
    }
}

// NPC와 대화하는 퀘스트 클래스
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
        Debug.Log(questTitle + " 퀘스트 완료!");
        return true;
    }
}

