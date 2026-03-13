# npc 대화 시스템   


## 기존에 사용하던 DataManager에 함수추가해서 데이터 불러오도록 변경    

 ``` 
    public static List<DialogueEvent> LoadNPCDialogueData(string npcNumber, int eventNumber)    
    {   
        string filePath = GetFilePath(npcNumber,"NpcDialog",".csv");    
        if (File.Exists(filePath))    
        {    
            List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();   
            string[] lines = File.ReadAllLines(filePath);    
     
            for (int i = 1; i < lines.Length; i++)    
            {    
                string[] values = lines[i].Split(',');    
                int eventNum = int.Parse(values[0].Trim());    
                if (eventNum == eventNumber)    
                {    
                    string speaker = values[1].Trim();     
                    string cameraPoint = values[2].Trim();    
                    string text = values[3].Trim();    
                    string cameraType = values[4].Trim();    
                    string dialogType = values[5].Trim();   
     
                    DialogueEvent dialogueEvent = new DialogueEvent(eventNum, speaker, cameraPoint, text, cameraType, dialogType);     
                    dialogueEvents.Add(dialogueEvent);    
     
                    if (dialogType == "END") break;     
                }    
            }     
            return dialogueEvents;     
        }    
        else    
        {    
            Debug.LogWarning("file not found: " + filePath);     
            return null;     
        }     
    }    

 ```    
   
## 기능 설명   
 이 스크립트는 대화 시스템을 구현하는 데 사용됩니다. NPC와의 대화를 시작하고 대화 이벤트를 처리합니다.    
또한 텍스트 타이핑 효과를 제공하고 선택지를 표시하여 사용자의 선택에 따라 대화를 진행합니다.   
 
 ### 장점   
1) 다양한 대화 이벤트를 처리할 수 있습니다.   
2) 텍스트 타이핑 효과를 통해 대화를 더 생생하게 보여줍니다.   
3) 선택지를 통해 사용자에게 대화 옵션을 제공합니다.     

 ### 단점    
1) 대화 이벤트가 많아질수록 코드가 복잡해질 수 있습니다.   
2) 대화 텍스트의 길이가 길 경우 사용자 경험이 저하될 수 있습니다.   
 
## 어려움과 해결책   
복잡한 대화 로직: 대화 이벤트를 처리하고 선택지를 표시하는 로직은 복잡할 수 있습니다. 이를 해결하기 위해 각 대화 이벤트에 대한 코루틴을 사용하여 각각의 이벤트를 순차적으로 처리했습니다.   

## 유튜브   
 [![Video Label](http://img.youtube.com/vi/aDysJcz79WQ/0.jpg)](https://youtu.be/aDysJcz79WQ)
 
