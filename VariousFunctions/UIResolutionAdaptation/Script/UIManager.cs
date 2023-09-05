using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    protected UIManager() { }
    public bool IsUILock = false;
    public SkillTree SkillTree;
    public QuickSlot QuickSlot;
    public Sprite SkillNodeLineImage;
    public PopUPInfo PopUPInfo;

    public RectTransform rectTransform;

    private CanvasScaler canvasScaler;
    float canvasWidth;
    float canvasHeight;
    float screenAspectRatio;
    float canvasAspectRatio;
    float offsetRatio;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        GetCanvasInfoAndRatio();
    }


    void Update()
    {
        if (IsUILock) return;

        if(Input.GetKeyUp(KeyCode.K))
        {
            SkillTree.gameObject.SetActive(true);
            IsUILock = true;
            PlayerInfo.GetInstance.movementInfo.inputLock = true;
            PlayerInfo.GetInstance.actionLock = true;
            Utils.CursorUnLock();
        }
    }

    public Transform GetPriority(int level)
    {
        return this.transform.GetChild(level);
    }


    void GetCanvasInfoAndRatio()
    {
        if (canvasScaler == null) return;

        // ĵ������ ����,����,���� ���ϱ�
        canvasWidth = canvasScaler.referenceResolution.x;
        canvasHeight = canvasScaler.referenceResolution.y;
        canvasAspectRatio = canvasWidth / canvasHeight;

        // ��ũ���� �ػ� ���� ���ϱ�
        screenAspectRatio = (float)Screen.width / (float)Screen.height;

        // ȭ���� �� ���� ���
        if (screenAspectRatio > canvasAspectRatio)
        {
            // ĵ������ ���̰� ȭ�� ���̿� ���� ���� ĵ������ �ʺ�
            float canvasWidthWithScreenHeight = canvasHeight * screenAspectRatio;
            //  �ʺ�(����) ���� �Ի�
            float offsetDiff = canvasWidthWithScreenHeight - canvasWidth;
            // �ʺ�(����) ���� ���� ���
            offsetRatio = offsetDiff / canvasWidth;

        }
        else // ȭ���� ���� ���
        {
            // ���� �ٸ� �κ��� �ʺ�-> ����
            float canvasHeightWithScreenWidth = canvasWidth / screenAspectRatio;
            float offsetDiff = canvasHeightWithScreenWidth - canvasHeight;
            offsetRatio = offsetDiff / canvasHeight;
        }
    }
    
    public Vector2  ChangeOffsetToMatchResolution (Vector2 oldOffset)
    {
        GetCanvasInfoAndRatio();

        // fullhd, height �� �������� offset ���
        Vector2 newOffset = oldOffset * canvasHeight / 1080f;

        // ȭ���� �� ���� ���
        if (screenAspectRatio > canvasAspectRatio)
        {
            newOffset.x += offsetRatio * oldOffset.x * canvasHeight / 1080f;
        }
        else // ȭ���� ���� ���
        {
            newOffset.y += offsetRatio * oldOffset.y * canvasHeight / 1080f;
        }

        return newOffset;
    }

}
