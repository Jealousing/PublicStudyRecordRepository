using UnityEngine;
using UnityEngine.UI;

public class UIResolutionAdaptationSetter : MonoBehaviour
{
    [SerializeField] private Vector2 fullHdOffset; // FullHD �ػ󵵿��� ������ offset��
    [SerializeField] private Vector2 fullHdSizeDelta;

    private RectTransform rectTransform;

    [SerializeField] bool modifyAnchorPivot;
    [SerializeField] bool modifyPosition;

    [SerializeField] bool modifyTest;
    private void Awake()
    {
        // rectTransform ������ �θ� ���� CanvasScaler ����
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

    // RectTransform�� ����� �� (ȭ�� ũ�Ⱑ ������ ��) ȣ��
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
            // ���ο� offset�� ����
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
