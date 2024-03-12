using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : Interactable
{
    // npc ���� ��ȣ�� ��ȭ �̺�Ʈ ��ȣ�� ��ȭ�� �� �� �ֵ��� ����.
    public string npcNumber;
    public int eventNumber;
       
    public override void StartInteractableCoroutine()
    {
        isCheck = false;
        StartCoroutine(StartInteractable());
    }

    // ��ȣ�ۿ� �����κ�
    IEnumerator StartInteractable()
    {
        UIManager temp = UIManager.GetInstance;

        // �����ϱ����� Ȱ��ȭ �Ǿ��ִ� �ٸ� ui ��Ȱ��ȭ
        temp.interactableSystem.ClearAllSlots();
        temp.lowPriority.gameObject.SetActive(false);
        temp.mediumPriority.gameObject.SetActive(false);

        // ���� �������� ����Ʈ ��Ͽ��� �ش� npc��ȣ�� ���� ��ȭ ����Ʈ�� �ִ��� Ȯ���ϴ� �Լ�
        Quest quest = QuestManager.GetInstance.CheckQuestList(QuestType.TalkToNPC, npcNumber);
        if (quest != null )
        {
            TalkToNPCQuest upCasting = (TalkToNPCQuest)quest;
            yield return StartCoroutine(temp.dialogSystem.DialogSet(upCasting.targetNpcNumber, upCasting.eventNumber));
        }
        // �ٸ� �̺�Ʈ�� ���ٸ� �⺻ �̺�Ʈ ����
        else
        {
            yield return StartCoroutine(temp.dialogSystem.DialogSet(npcNumber, eventNumber));
        }
        
        // ������ UI �ǵ�����
        temp.lowPriority.gameObject.SetActive(true);
        temp.mediumPriority.gameObject.SetActive(true);
        temp.interactableSystem.ReloadSlots();
    }
}
