using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> activeQuests = new List<Quest>();

    public GameObject marker;
    public Vector3 targetPos;
    public GameObject slotPrefab;
    public RectTransform content;
    private List<QuestSlot> slotObjects = new List<QuestSlot>();

    public void AddQuest(string questNumber)
    {
        Quest temp = DataManager.LoadQuestData(questNumber);
        activeQuests.Add(temp);
        AddSlot(temp);
        DistantQuestMarker(temp);
    }

    private void Update()
    {
        if(marker.activeSelf)
        {
            marker.transform.position = Camera.main.WorldToScreenPoint(targetPos + new Vector3(0, 3f, 0));

            float distance = Vector3.Distance(targetPos, PlayerInfo.GetInstance.transform.position);
            float normalizedDistance = Mathf.Clamp01((distance - 5f) / (10f - 2f));
            float scale = Mathf.Lerp(0.5f, 0.25f, normalizedDistance);

            marker.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

    private void DistantQuestMarker(Quest data)
    {
        // 데이터에 저장된 퀘스트의 씬 이름과 현재 씬 이름을 비교합니다.
        if (data.questScene == SceneManager.GetActiveScene())
        {
            // 현재 씬과 동일한 씬일 때만 마커를 생성합니다.
            marker.SetActive(true);
            targetPos = data.questPos;
        }
    }

    private void AddSlot(Quest data)
    {
        QuestSlot slotScript = Instantiate(slotPrefab, content).GetComponent<QuestSlot>();
        slotScript.Initialize(data, data.questTitle, data.questDescription);
        slotObjects.Add(slotScript);
    }
    private void RemoveSlot(Quest data)
    {
        List<QuestSlot> slotsToRemove = slotObjects.FindAll(slot => slot.data== data);
        foreach (QuestSlot slotToRemove in slotsToRemove)
        {
            slotObjects.Remove(slotToRemove);
            Destroy(slotToRemove.gameObject);
        }
    }

    public Quest CheckQuestList(QuestType questType, string value="")
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.questType == questType)
            {
                switch(questType)
                {
                    case QuestType.KillMonster:
                        if (quest is KillMonsterQuest)
                        {
                            KillMonsterQuest killQuest = (KillMonsterQuest)quest; // 업캐스팅
                            Debug.Log("Found KillMonsterQuest: " + killQuest.questTitle);
                            // 특정 작업 수행
                            return quest;
                        }
                        break;
                    case QuestType.CollectItem:
                        if (quest is CollectItemQuest)
                        {
                            CollectItemQuest collectQuest = (CollectItemQuest)quest; // 업캐스팅
                            Debug.Log("Found CollectItemQuest: " + collectQuest.questTitle);
                            // 특정 작업 수행
                            return quest;
                        }
                        break;
                    case QuestType.MoveToLocation:
                        if (quest is MoveToLocationQuest)
                        {
                            MoveToLocationQuest moveQuest = (MoveToLocationQuest)quest; // 업캐스팅
                            Debug.Log("Found MoveToLocationQuest: " + moveQuest.questTitle);
                            // 특정 작업 수행
                            return quest;
                        }
                        break;
                    case QuestType.TalkToNPC:
                        if (quest is TalkToNPCQuest)
                        {
                            TalkToNPCQuest talkQuest = (TalkToNPCQuest)quest; // 업캐스팅
                            Debug.Log("Found TalkToNPCQuest: " + talkQuest.questTitle);
                            // 특정 작업 수행.
                            //대화퀘스트로 퀘스트는 끝나기때문에 이때 대화를 시작하게 만들면된다.
                            if (talkQuest.targetNpcNumber == value && talkQuest.UpdateProgress())
                            {
                                RemoveSlot(quest);
                                activeQuests.Remove(quest);
                                return quest;
                            }
                        }
                        break;
                }
            }
        }
        return null;
    }

}
