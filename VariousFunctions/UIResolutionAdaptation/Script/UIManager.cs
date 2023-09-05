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

        // 캔버스의 가로,세로,비율 구하기
        canvasWidth = canvasScaler.referenceResolution.x;
        canvasHeight = canvasScaler.referenceResolution.y;
        canvasAspectRatio = canvasWidth / canvasHeight;

        // 스크린의 해상도 비율 구하기
        screenAspectRatio = (float)Screen.width / (float)Screen.height;

        // 화면이 더 넓을 경우
        if (screenAspectRatio > canvasAspectRatio)
        {
            // 캔버스의 높이가 화면 높이와 같을 때의 캔버스의 너비
            float canvasWidthWithScreenHeight = canvasHeight * screenAspectRatio;
            //  너비(가로) 차이 게산
            float offsetDiff = canvasWidthWithScreenHeight - canvasWidth;
            // 너비(가로) 간격 비율 계산
            offsetRatio = offsetDiff / canvasWidth;

        }
        else // 화면이 좁을 경우
        {
            // 위와 다른 부분은 너비-> 높이
            float canvasHeightWithScreenWidth = canvasWidth / screenAspectRatio;
            float offsetDiff = canvasHeightWithScreenWidth - canvasHeight;
            offsetRatio = offsetDiff / canvasHeight;
        }
    }
    
    public Vector2  ChangeOffsetToMatchResolution (Vector2 oldOffset)
    {
        GetCanvasInfoAndRatio();

        // fullhd, height 값 기준으로 offset 계산
        Vector2 newOffset = oldOffset * canvasHeight / 1080f;

        // 화면이 더 넓을 경우
        if (screenAspectRatio > canvasAspectRatio)
        {
            newOffset.x += offsetRatio * oldOffset.x * canvasHeight / 1080f;
        }
        else // 화면이 좁을 경우
        {
            newOffset.y += offsetRatio * oldOffset.y * canvasHeight / 1080f;
        }

        return newOffset;
    }

}
