using UnityEngine;
using UnityEngine.UI;

public class UIResolutionAdaptationSetter : MonoBehaviour
{
    [SerializeField] private Vector2 fullHdOffset; // FullHD 해상도에서 설정한 offset값
    [SerializeField] private Vector2 fullHdSizeDelta;

    private RectTransform rectTransform;

    [SerializeField] bool modifyAnchorPivot;
    [SerializeField] bool modifyPosition;

    [SerializeField] bool modifyTest;
    private void Awake()
    {
        // rectTransform 정보와 부모에 있을 CanvasScaler 참조
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if(modifyAnchorPivot)
        {
            rectTransform.anchorMin = new Vector2(1f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(1f, 1f);
        }

        Positioning();
    }

    // RectTransform가 변경될 때 (화면 크기가 조정될 때) 호출
    private void OnRectTransformDimensionsChange()
    {
        if (Application.isPlaying) Positioning();
    }

    private void OnValidate()
    {
        fullHdOffset = GetComponent<RectTransform>().anchoredPosition;
        fullHdSizeDelta = GetComponent<RectTransform>().sizeDelta;
    }


    public void Positioning()
    {

        if (rectTransform == null || UIManager.GetInstance == null)
            return;

        if (modifyPosition)
        {
            // 새로운 offset값 적용
            rectTransform.anchoredPosition = UIManager.GetInstance
                .ChangeOffsetToMatchResolution(fullHdOffset); ;
        }

        if(modifyTest)
        {
            rectTransform.sizeDelta = new Vector2(fullHdSizeDelta.x,fullHdSizeDelta.y * 
                UIManager.GetInstance.rectTransform.sizeDelta.y / 1080f);

        }
    }
}
