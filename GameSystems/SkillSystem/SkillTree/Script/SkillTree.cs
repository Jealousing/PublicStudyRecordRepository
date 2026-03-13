using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SkillTree : MonoBehaviour
{
    public RectTransform contentCollection;
    [SerializeField] private RectTransform background;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;

    private RectTransform rectTransform;

    private Vector2 collectionSize;
    private Vector2 collectionPos;
    private float collectionScale = 1f;

    [SerializeField] private TextMeshProUGUI textPlayerLV;
    private string defaultPlayerLV;
    [SerializeField] private TextMeshProUGUI textSkillPoint;
    private string defaultSkillPoint;
    [HideInInspector] public int skillTempPoint;
    private int preSkillPoint;

    public GameObject skillDragImage;
    public GameObject NodeLinePrefab;
    public Transform NodeLineList;
    public GameObject NodePrefab;

    [HideInInspector] public bool IsChanges=false;

    const int QuickSlotNum = 10;
    [HideInInspector] public SkillInfo[,] QuickSlotSkill = 
        new SkillInfo[System.Enum.GetValues(typeof(AttackBehaviorType)).Length, QuickSlotNum];


    public SkillTreeSlotPanel[] SkillTreeSlotPanels;

    public TMP_Dropdown dropDown;
    public PopUpSkillInfo PopUpSkillInfo;

    void DropDownSet()
    {
        dropDown.ClearOptions();
        // Enum 값들의 이름 배열을 가져옵니다.
        string[] enumNames = System.Enum.GetNames(typeof(AttackBehaviorType));
        // 맨 앞의 값을 제외한 이름 배열을 만듭니다.
        List<string> options = new List<string>(enumNames);
        options.RemoveAt(0);
        // Dropdown에 옵션을 추가합니다.
        dropDown.AddOptions(options);
    }

    public void SetUseSkill(int number, SkillInfo skillInfo)
    {
        QuickSlotSkill[dropDown.value, number] = skillInfo;
    }

    void UpdatePanel(int select)
    {
        for (int i=0;i<SkillTreeSlotPanels.Length;i++)
        {
            if(QuickSlotSkill[select, i] != null)
            {
                SkillTreeSlotPanels[i].SetSkillData(QuickSlotSkill[select, i]);
            }
            else
            {
                SkillTreeSlotPanels[i].ResetPanel();
            }
          
        }
    }

    void PanelNumbering()
    {
        for(int i=0; i<SkillTreeSlotPanels.Length; i++)
        {
            SkillTreeSlotPanels[i].number = i;
        }
    }

    public void OnDropDownValueChanged()
    {
        UpdatePanel(dropDown.value);
    }

    #region Event Function

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        collectionSize = contentCollection.rect.size;
        collectionPos = contentCollection.anchoredPosition;
        defaultPlayerLV = textPlayerLV.text;
        defaultSkillPoint = textSkillPoint.text;
    }
    private void Start()
    {
        skillTempPoint = PlayerInfo.GetInstance.playerStats.currentSkillPoints;
        UpdateInfo(); 
        DropDownSet();
        PanelNumbering();
        LoadQuickSlotSkill();
    }

    private void OnEnable()
    {
        if(PlayerInfo.GetInstance.skillTreeNodeInfo != null)
        {
            IsChanges = false;
            skillTempPoint = PlayerInfo.GetInstance.playerStats.currentSkillPoints;
            UpdateInfo();
        }
    }

    private void Update()
    {
        // 움직임
        HandleZoom();
        HandlePan();

        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.Escape))
        {
            StartCoroutine(ExitSkillTree());
        }
    }

    #endregion

    #region DataManagement

    public void SaveSkillTree()
    {
        for (int i = 0; i < PlayerInfo.GetInstance.skillTreeNodeInfo.Length; i++)
        {
            PlayerInfo.GetInstance.skillTreeNodeInfo[i].SaveNode();
        }
        PlayerInfo.GetInstance.playerStats.currentSkillPoints = skillTempPoint;
        DataManager.GetInstance.SaveData(PlayerInfo.GetInstance.playerStats, "playerStats");
        SaveQuickSlotSkill();
    }

    void SaveQuickSlotSkill()
    {
        for(int i=0; i< System.Enum.GetValues(typeof(AttackBehaviorType)).Length; i++)
        {
            for(int j=0; j < QuickSlotNum; j++)
            {
                string fileName = "skillQuickSlot_"+i+j;
                if (QuickSlotSkill[i, j] != null)
                {
                    QuickSlotSkill[i, j].OnBeforeSerialize();
                }
                DataManager.GetInstance.SaveData(QuickSlotSkill[i,j], fileName, "SkillData/QuickSlot");
            }      
        }
    }
    public void LoadQuickSlotSkill()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(AttackBehaviorType)).Length; i++)
        {
            for (int j = 0; j < QuickSlotNum; j++)
            {
                string fileName = "skillQuickSlot_" + i + j;
                QuickSlotSkill[i, j] = DataManager.GetInstance.LoadData<SkillInfo>(fileName, "SkillData/QuickSlot");
                if (QuickSlotSkill[i, j] != null)
                {
                    QuickSlotSkill[i, j] = DataManager.GetInstance.LoadData<SkillInfo>(QuickSlotSkill[i, j].skillName, "SkillData");
                    QuickSlotSkill[i, j].OnAfterDeserialize();
                }
            }
        }
        UpdatePanel(dropDown.value);
    }


    public void ResetSkillTree()
    {
        IsChanges = true;
        preSkillPoint = skillTempPoint;
        PlayerInfo.GetInstance.playerStats.currentSkillPoints = PlayerInfo.GetInstance.playerStats.maxSkillPoints;
        skillTempPoint = PlayerInfo.GetInstance.playerStats.maxSkillPoints;
        for (int i = 0; i < PlayerInfo.GetInstance.skillTreeNodeInfo.Length; i++)
        {
            PlayerInfo.GetInstance.skillTreeNodeInfo[i].NodeReset();
        }
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        textPlayerLV.text = defaultPlayerLV + PlayerInfo.GetInstance.LV;
        textSkillPoint.text = defaultSkillPoint + skillTempPoint;
    }
    public void ExitButton()
    {
        StartCoroutine(ExitSkillTree());
    }

    public void CancleButton()
    {
        if (!IsChanges) return;

        if(preSkillPoint !=0)
        {
            skillTempPoint = preSkillPoint;
            PlayerInfo.GetInstance.playerStats.currentSkillPoints = preSkillPoint;
            preSkillPoint = 0;
        }
        else
        {
            skillTempPoint = PlayerInfo.GetInstance.playerStats.currentSkillPoints;
        }
        
        for (int i = 0; i < PlayerInfo.GetInstance.skillTreeNodeInfo.Length; i++)
        {
            StartCoroutine(PlayerInfo.GetInstance.skillTreeNodeInfo[i].LoadNode());
        }
        IsChanges = false;
        UpdateInfo();
        DropDownSet();
        PanelNumbering();
    }

    public void SaveButton()
    {
        StartCoroutine(SaveSkill());
    }

    private IEnumerator SaveSkill()
    {
        bool result = false;

        if (IsChanges)
        {
            yield return UIManager.GetInstance.PopUPInfo.ShowPopup("Do you want to save?", (bool clickedYes) => { result = clickedYes; });

            if (result)
            {
                SaveSkillTree();
                IsChanges = false;
            }
        }
    }



    private IEnumerator ExitSkillTree()
    {
        bool result = false;

        if (IsChanges)
        {
            yield return UIManager.GetInstance.PopUPInfo.ShowPopup("Save and Quit?", (bool clickedYes) => { result = clickedYes; });

            if (result)
            {
                SaveSkillTree();
                IsChanges = false;
            }
            else
            {
            }
        }
        this.gameObject.SetActive(false);
        UIManager.GetInstance.IsUILock = false;
        PlayerInfo.GetInstance.movementInfo.inputLock = false;
        PlayerInfo.GetInstance.actionLock = false;
        Utils.CursorLock();
    }

    private void OnDisable()
    { 
        LoadQuickSlotSkill();
        UIManager.GetInstance.QuickSlot.SkillSlot.UpdateSlot();
    }
    #endregion

    #region Controller
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        collectionScale += scroll * zoomSpeed;
        collectionScale = Mathf.Clamp(collectionScale, minScale, maxScale);
        contentCollection.localScale = new Vector3(collectionScale, collectionScale, 1f);
    }

    private void HandlePan()
    {
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Vector2 delta = new Vector2(x, y) * panSpeed * collectionScale;
            collectionPos += delta;
            collectionPos.x = Mathf.Clamp(collectionPos.x, -collectionSize.x * collectionScale / 2f + rectTransform.rect.width / 2f, collectionSize.x * collectionScale / 2f - rectTransform.rect.width / 2f);
            collectionPos.y = Mathf.Clamp(collectionPos.y, -collectionSize.y * collectionScale / 2f + rectTransform.rect.height / 2f, collectionSize.y * collectionScale / 2f - rectTransform.rect.height / 2f);
            contentCollection.anchoredPosition = collectionPos;
        }
    }
    #endregion



}
