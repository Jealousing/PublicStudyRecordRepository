using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(NodeInfo))]
public class NodeInfoEditor : Editor
{
    public bool hideEditor;
    GUIStyle stringStyle = new();
    NodeInfo editScript;
    
    void OnSceneGUI()
    {
        editScript = (NodeInfo)target;

        if(editScript != null )
        {
            //기본 폰트 스타일
            stringStyle.normal.textColor = Color.green;
            stringStyle.fontStyle = FontStyle.Bold;
            stringStyle.fontSize = 30;
            stringStyle.alignment = TextAnchor.MiddleCenter;

            //버튼 스타일

            GUIStyle buttonStyle = new(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.fontSize = 15;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            editScript.transform.position = Handles.PositionHandle(editScript.transform.position, Quaternion.identity); //기즈모 표시
            Handles.Label(editScript.transform.position + new Vector3(0, -0.25f, 0), this.name , stringStyle); //배열 숫자 표시

            Handles.BeginGUI();
            //위치 추가
            GUI.backgroundColor = new Color(0.3f, 0.5f, 2);
            if (GUI.Button(new Rect(HandleUtility.WorldToGUIPoint(editScript.transform.position).x, 
                HandleUtility.WorldToGUIPoint(editScript.transform.position).y - 65, 80, 20), "Add", buttonStyle))
            {
                NodeInfo newObj = Instantiate(editScript.gameObject, editScript.transform.parent).GetComponent<NodeInfo>();
                newObj.name = editScript.name + "_copy";
                Vector3 offset = new Vector3(5f, 5f, 0);
                newObj.transform.position += offset;
                newObj.childrenNodeList.Clear();
            }

            //위치 제거
            GUI.backgroundColor = new Color(2, 0.5f, 0.2f);
            if (GUI.Button(new Rect(HandleUtility.WorldToGUIPoint(editScript.transform.position).x,
                HandleUtility.WorldToGUIPoint(editScript.transform.position).y - 40, 80, 20), "Remove", buttonStyle))
            {
                DestroyImmediate(editScript.gameObject);
            }
            Handles.EndGUI();

            Handles.color = new Color(2, 0.5f, 0.2f, 5);
            Vector3[] Vector3Array = new Vector3[editScript.childrenNodeList.Count];
            for (int i = 0; i < Vector3Array.Length; i++)
            {
                Vector3Array[i] = editScript.childrenNodeList[i].transform.position;
            }

            // Draw lines one by one
            for (int i = 0; i < Vector3Array.Length; i++)
            {
                Handles.DrawLine(editScript.transform.position, Vector3Array[i]);
            }
        }
    }

}
