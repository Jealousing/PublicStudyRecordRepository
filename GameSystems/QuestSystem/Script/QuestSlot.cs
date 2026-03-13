using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlot : MonoBehaviour
{
    public Quest data;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;

    public void Initialize(Quest data, string questTitle,string questDescription)
    {
        this.data = data;
        this.questTitle.text = questTitle;
        this.questDescription.text = questDescription;
    }
}
