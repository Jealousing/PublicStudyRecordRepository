using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // 스킬을 배울 수 있는 조건 설정
    [SerializeField] int skillGetLv = 0;
    public int skillGetSkillLv;

    // 스킬 정보
    [SerializeField] public SkillData skillData;
    public int skillCurLV;
    public bool isActivation = false;
    // 노드
    Image nodeImage;
    [HideInInspector] public List<NodeInfo> parentNodeList;
    public List<NodeInfo> childrenNodeList;
    Vector3 preVec3;
    GameObject[] lineList;
    public TextMeshProUGUI SkillText;
    public RectTransform rectTransform;
    SkillTree skillTree;
    IEnumerator popupSkillinfoCoroutine;
    private float holdTime = 2f;
    private float pointerEnterTime = 0f;
    private GameObject clickedGameObject;
    private bool isPressed = false;
    private float pressTime = 0f;

    private void Awake()
    {
        nodeImage = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    private void Start()
    {
        lineList = new GameObject[childrenNodeList.Count];
        preVec3 = this.transform.position;

        // 자식노드에게 자기가 부모인것을 알림.
        if (childrenNodeList.Count > 0)
        {
            for (int i = 0; i < childrenNodeList.Count; i++)
            {
                if (childrenNodeList[i] != null)
                    childrenNodeList[i].parentNodeList.Add(this);
            }
        }
        skillTree = UIManager.GetInstance.SkillTree;
    }

    private void OnEnable()
    {
        StartCoroutine(LoadNode());
    }

    // 노드(스킬정보) 저장
    public void SaveNode()
    {
        skillData.skillInfo.skillLV = skillCurLV;
        skillData.skillInfo.OnBeforeSerialize();
        DataManager.GetInstance.SaveData(skillData.skillInfo, skillData.name, "SkillData");
    }

    // 노드(스킬정보) 불러오기
    public IEnumerator LoadNode()
    {
        isActivation = false;
        LoadSkillInfo();
       
        skillCurLV = skillData.skillInfo.skillLV;
        yield return null;
        UpdateNode();
    }

    public void LoadSkillInfo()
    {
        skillData.skillInfo = DataManager.GetInstance.LoadData<SkillInfo>(skillData.name, "SkillData");
        skillData.skillInfo.OnAfterDeserialize();
    }


    private void LateUpdate()
    {
        if (Vector3.Distance(this.transform.position, preVec3) > 0.1f)
        {
            DrawLinesToChildren();
            preVec3 = this.transform.position;
        }
        else
        {
            preVec3 = this.transform.position;
        }
    }

    #region DrawNodeLine
    void DrawLinesToChildren()
    {
        for (int i = 0; i < childrenNodeList.Count; i++)
        {
            if (childrenNodeList[i] != null)
            {
                UpdateLine(childrenNodeList[i].rectTransform, i);
            }
        }
    }

    private void UpdateLine(RectTransform target, int num)
    {
        if (lineList[num] == null)
        {
            lineList[num] = Instantiate(skillTree.NodeLinePrefab);
        }
    
        Vector2 difference = rectTransform.transform.position - target.transform.position;
        float sign = (rectTransform.transform.position.y < target.transform.position.y) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.right, difference) * sign;
        lineList[num].transform.rotation = Quaternion.Euler(0, 0, angle);

        float height = 80;
        float width = Vector2.Distance(target.transform.position, rectTransform.transform.position);
        lineList[num].GetComponent<RectTransform>().sizeDelta = new Vector2(width, height) * UIManager.GetInstance.rectTransform.sizeDelta.y / 1080f;

        float newposX = target.transform.position.x + (this.transform.position.x - target.transform.position.x) / 2;
        float newposY = target.transform.position.y + (this.transform.position.y - target.transform.position.y) / 2;
        lineList[num].transform.position = new Vector3(newposX, newposY, 0);

        lineList[num].transform.SetParent(skillTree.NodeLineList, true);
        lineList[num].transform.localScale = Vector3.one;
    }
    #endregion

    public void NodeReset()
    {
        // 초기화시 정보리셋
        isActivation = false;
        skillCurLV = skillData.skillInfo.skillDefaultLV;
        UpdateNode();
    }

    public void UpdateNode()
    {
        if (!isActivation)
        {
            if (skillCurLV > 0)
            {
                isActivation = true;
            }
            else
            {
                for (int i = 0; i < parentNodeList.Count; i++)
                {
                    if (parentNodeList[i] != null && parentNodeList[i].skillCurLV >= skillGetSkillLv)
                    {
                        isActivation = true;
                    }
                }
            }
        }
        if (isActivation)
        {
            if (skillCurLV > 0)
            {
                nodeImage.sprite = skillData.skillInfo.skillIcon;
                SkillText.transform.localPosition = new Vector3(40, -35, 0);
                SkillText.color = Color.white;
                nodeImage.color = Color.white;
                SkillText.fontSize = 36;
                SkillText.text = skillCurLV + "";
            }
            else if (skillCurLV == 0)
            {
                nodeImage.sprite = skillData.skillInfo.skillIcon;
                SkillText.text = "";
            }
        }
                
        else
        {
            nodeImage.sprite = skillData.skillInfo.skillIcon;
            SkillText.transform.localPosition = Vector3.zero;
            SkillText.color = Color.red;
            nodeImage.color = Color.gray;
            SkillText.fontSize = 145;
            SkillText.text = "X";
        }
    }

    bool CheckParent()
    {
        if (parentNodeList.Count > 0)
        {
            for (int i = 0; i < parentNodeList.Count; i++)
            {
                // 부모노드에서 조건을 확인해서 비교
                if (skillGetSkillLv <= parentNodeList[i].skillCurLV &&
                    skillGetLv <= PlayerInfo.GetInstance.LV)
                {
                    return true;
                }
            }
        }
        else if (skillGetLv <= PlayerInfo.GetInstance.LV)
        {
            return true;
        }
        return false;
    }



    void SetSkillLv()
    {
        // 비활성화 = 안찍혀있음

        if (PlayerInfo.GetInstance.playerStats.currentSkillPoints > 0 &&
            skillData.skillInfo.skillMaxLV > skillCurLV )
        {
            if (!isActivation) isActivation = true;

            skillCurLV++;
            UpdateNode();
            skillTree.skillTempPoint--;
            skillTree.UpdateInfo();
            skillTree.IsChanges = true;

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressTime = Time.deltaTime;
        clickedGameObject = eventData.pointerCurrentRaycast.gameObject;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (isPressed && eventData.pointerCurrentRaycast.gameObject == clickedGameObject)
            {
                isPressed = false;
                pressTime = 0;
                clickedGameObject = null;

                if (CheckParent())
                {
                    SetSkillLv();

                    for (int i = 0; i < childrenNodeList.Count; i++)
                    {
                        if (childrenNodeList[i] != null)
                            childrenNodeList[i].UpdateNode();
                    }
                    return;
                }
            }
            else if(pressTime> 0.25f && eventData.pointerCurrentRaycast.gameObject != null)
            {
                skillTree.skillDragImage.SetActive(false);
                // 검사할 스크립트가 있는지 확인
                SkillTreeSlotPanel skillPanel = eventData.pointerCurrentRaycast.gameObject.GetComponent<SkillTreeSlotPanel>();
                if (skillPanel != null)
                {
                    // 스크립트에서 정의한 함수 호출
                    skillPanel.AddSkillData(skillData.skillInfo);
                    return;
                }
            }
        }
        skillTree.skillDragImage.SetActive(false);
        isPressed = false;
        pressTime = 0;
        clickedGameObject = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPressed && skillData.skillInfo.skillType >= SkillType.Active &&
            skillCurLV >0
            )
        {
            pressTime += Time.deltaTime;
            if (pressTime > 0.1f && !skillTree.skillDragImage.activeSelf)
            {
                skillTree.skillDragImage.transform.position = eventData.position;
                skillTree.skillDragImage.SetActive(true);
                skillTree.skillDragImage.GetComponent<Image>().sprite = skillData.skillInfo.skillIcon;
                skillTree.dropDown.value = (int)skillData.skillInfo.attackBehavior - 1;
            }
            else if (pressTime > 0.25f)
            {
                skillTree.skillDragImage.transform.position = eventData.position;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEnterTime = Time.time;
        StartCoroutine(popupSkillinfoCoroutine = PopupSkillinfo());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEnterTime = 0f;
        StopCoroutine(popupSkillinfoCoroutine);
        skillTree.PopUpSkillInfo.gameObject.SetActive(false);
    }
    
    IEnumerator PopupSkillinfo()
    {
        while(true)
        {
            if (pointerEnterTime != 0 && Time.time - pointerEnterTime >= holdTime)
            {
                skillTree.PopUpSkillInfo.Setting(skillData.skillInfo);
                skillTree.PopUpSkillInfo.gameObject.SetActive(true);
                pointerEnterTime = 0f;
                break;
            }
            yield return null;
        }
    }

}
