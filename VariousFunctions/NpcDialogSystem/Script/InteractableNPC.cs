using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : Interactable
{
    // npc 고유 변호와 대화 이벤트 번호로 대화를 할 수 있도록 설정.
    public string npcNumber;
    public int eventNumber;
       
    public override void StartInteractableCoroutine()
    {
        isCheck = false;
        StartCoroutine(StartInteractable());
    }

    // 상호작용 구현부분
    IEnumerator StartInteractable()
    {
        UIManager temp = UIManager.GetInstance;

        // 시작하기전에 활성화 되어있는 다른 ui 비활성화
        temp.interactableSystem.ClearAllSlots();
        temp.lowPriority.gameObject.SetActive(false);
        temp.mediumPriority.gameObject.SetActive(false);

        // 현재 진행중인 퀘스트 목록에서 해당 npc번호를 가진 대화 퀘스트가 있는지 확인하는 함수
        Quest quest = QuestManager.GetInstance.CheckQuestList(QuestType.TalkToNPC, npcNumber);
        if (quest != null )
        {
            TalkToNPCQuest upCasting = (TalkToNPCQuest)quest;
            yield return StartCoroutine(temp.dialogSystem.DialogSet(upCasting.targetNpcNumber, upCasting.eventNumber));
        }
        // 다른 이벤트가 없다면 기본 이벤트 진행
        else
        {
            yield return StartCoroutine(temp.dialogSystem.DialogSet(npcNumber, eventNumber));
        }
        
        // 끝나면 UI 되돌리기
        temp.lowPriority.gameObject.SetActive(true);
        temp.mediumPriority.gameObject.SetActive(true);
        temp.interactableSystem.ReloadSlots();
    }
}
