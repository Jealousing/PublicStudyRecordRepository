using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI dialogText;
    public ChoiceSlot[] choiceSlots;

    private List<DialogueEvent> currentConversation;
    CameraStateSystem cameraStateSystem;
    LookTheTargetCamera lookTheTargetCamera;

    private bool isSkip = false;
    string previousNpcNumber;
    int previousEventNumber;

    private void Awake()
    {
        lookTheTargetCamera = PlayerInfo.GetInstance.GetComponent<LookTheTargetCamera>();
        cameraStateSystem = PlayerInfo.GetInstance.cameraInfo.CameraStateSystem;
    }

    private void Update()
    {
        DialogSkipKeyCheck();
    }
    
    void DialogSkipKeyCheck()
    {
        if (isSkip) return;

        if(Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            isSkip = true;
        }
    }

    public IEnumerator DialogSet(string npcNumber, int eventNumber)
    {
        gameObject.SetActive(true);
        isSkip = false;

        // 데이터 찾아오기
        currentConversation = DataManager.LoadNPCDialogueData(previousNpcNumber=npcNumber, previousEventNumber= eventNumber);

        yield return DisplayConversation();
    }

    IEnumerator DisplayConversation()
    {
        dialogText.text = "";

        cameraStateSystem.ChangeState(CameraState.LookTheTarget);

        // 이전에 검색한 NPC의 이름과 카메라위치 이름을 저장합니다.
        string previousTargetName = null;
        string previousCameraPoint = null;

        // 대화 이벤트를 하나씩 처리합니다.
        foreach (DialogueEvent dialogueEvent in currentConversation)
        {
            string text = dialogueEvent.dialogueText;
            Collider target = null;

            // 이전에 검색한 NPC와 현재 대상이 다른 경우에만 NPC를 검색합니다.
            if ( (previousTargetName != dialogueEvent.speaker) || 
                (previousTargetName == dialogueEvent.speaker && previousCameraPoint != dialogueEvent.cameraPoint) )
            {
                previousTargetName = dialogueEvent.speaker;
                previousCameraPoint = dialogueEvent.cameraPoint;

                // 플레이어 주변의 오브젝트를 검색합니다.
                Collider[] nearbyColliders = Physics.OverlapSphere(PlayerInfo.GetInstance.transform.position, 50f);
                foreach (Collider col in nearbyColliders)
                {
                    if (col.gameObject.name == dialogueEvent.speaker)
                    {
                        target = col;
                        if (dialogueEvent.cameraPoint == "")
                        {
                            Vector3 directionToTarget = target.transform.forward * 1.5f;
                            Vector3 heightOffset = new Vector3(0, 1.5f, 0);

                            Vector3 cameraPos = target.transform.position + directionToTarget + heightOffset;

                            lookTheTargetCamera.set(cameraPos, 
                               ((target.transform.position+heightOffset)- cameraPos).normalized, dialogueEvent.cameraType);
                            break;
                           
                        }
                        else
                        {
                            Transform temp = target.gameObject.transform.Find(dialogueEvent.cameraPoint);
                            if (temp != null)
                            {
                                // LookTheTargetCamera 클래스에 NPC의 위치와 카메라의 로컬 위치를 설정합니다.
                                lookTheTargetCamera.set(temp.position, temp.forward, dialogueEvent.cameraType);
                                break;
                            }
                        }
                    }
                }

            }

            // 대화 텍스트를 표시합니다.
            objectName.text = dialogueEvent.speaker;
            yield return TypeText(text);
            yield return DialogChoice(dialogueEvent, target);
            

            // 다음 진행을 기다립니다.
            while (dialogueEvent.dialogType == "" && !Input.anyKeyDown && !Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            isSkip = false;
        }

        // 대화가 끝난 후 카메라 상태를 변경합니다.
        cameraStateSystem.ChangeState(CameraState.BackView);
        gameObject.SetActive(false);
    }

    // 타이핑 효과 주는 텍스트 작성 코루틴
    IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        foreach (char letter in text)
        {
            dialogText.text += letter;
            if(isSkip)
            {
                isSkip = false;
                dialogText.text = text;
                break;
            }
            yield return new WaitForSeconds(0.025f); 
        }
    }

    IEnumerator DialogChoice(DialogueEvent temp , Collider col=null)
    {
        if (temp.dialogType == "") yield break;

        int result = -1; 

        switch (temp.dialogType)
        {
            case "ContinueOrEnd":
                SetupChoiceSlots((choice) => result = choice, "End", "Continue");
                break;

            case "QuestOrContinue":
                SetupChoiceSlots((choice) => result = choice+1, "Rejection", "Accept Quest");
                break;

            case "End":
                SetupChoiceSlots((choice) => result = choice, "End");
                break;

            default:
                yield break;
        }

        // 버튼 클릭할 때까지 또는 ESC 키가 눌릴 때까지 대기
        yield return new WaitUntil(() => result != -1 || Input.GetKeyDown(KeyCode.Escape));

        // ESC 키가 눌렸을 경우
        if (result == -1 && Input.GetKeyDown(KeyCode.Escape))
        {
            result = 0;
        }

        foreach (ChoiceSlot choiceSlot in choiceSlots)
        {
            choiceSlot.Reset();
        }

        // 선택된 결과에 따라 처리
        if (result >0)
        {
            switch (temp.dialogType)
            {
                case "ContinueOrEnd":
                    yield return DialogSet(previousNpcNumber, previousEventNumber + result);
                    break;

                case "QuestOrContinue":
                    if (result == 2)
                    {
                        QuestManager.GetInstance.AddQuest(col.gameObject.GetComponent<QuestGiver>().questNumber);
                    }
                    else yield return DialogSet(previousNpcNumber, previousEventNumber + 1);
                    break;

                default:
                    break;
            }
        }
    }

    void SetupChoiceSlots(Action<int> resultCallback,string option1Text, string option2Text="")
    {
        choiceSlots[0].gameObject.SetActive(true);
        choiceSlots[0].textUI.text = option1Text;
        choiceSlots[0].button.onClick.AddListener(() => resultCallback(0));

        if (!string.IsNullOrEmpty(option2Text))
        {
            choiceSlots[1].gameObject.SetActive(true);
            choiceSlots[1].textUI.text = option2Text;
            choiceSlots[1].button.onClick.AddListener(() => resultCallback(1));
        }
    }
}
