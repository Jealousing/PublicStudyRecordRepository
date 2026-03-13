using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject imageToActivate;

    public void OnPointerEnter(PointerEventData eventData)
    {
        imageToActivate.SetActive(true); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imageToActivate.SetActive(false); 
    }
}
