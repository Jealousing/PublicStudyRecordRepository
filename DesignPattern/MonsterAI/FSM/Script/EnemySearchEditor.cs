using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/// <summary>
/// https://github.com/Ageia/Unity_SubToolsList/blob/master/AI_MovePath_Editor.cs
/// </summary>

[CustomEditor(typeof(EnemySearch))]
public class EnemySearchEditor : Editor
{
    public bool hideEditor;

    GUIStyle stringStyle = new();
    
    EnemySearch editScript;

    void OnSceneGUI()
    {
        editScript = (EnemySearch)target;

        if (editScript.wayPoint != null && editScript.wayPoint.Count > 0)
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


            for (int i = 0; i < editScript.wayPoint.Count; i++)
            {
                if (!hideEditor)
                {
                    editScript.wayPoint[i] = Handles.PositionHandle(editScript.wayPoint[i], Quaternion.identity); //기즈모 표시
                    Handles.Label(editScript.wayPoint[i] + new Vector3(0, -0.25f, 0), i.ToString(), stringStyle); //배열 숫자 표시


                    Handles.BeginGUI();
                    //위치 추가
                    GUI.backgroundColor = new Color(0.3f, 0.5f, 2);
                    if (GUI.Button(new Rect(HandleUtility.WorldToGUIPoint(editScript.wayPoint[i]).x, HandleUtility.WorldToGUIPoint(editScript.wayPoint[i]).y - 65, 80, 20), "Add", buttonStyle))
                    {
                        editScript.wayPoint.Insert(i, new Vector3(editScript.wayPoint[i].x, editScript.wayPoint[i].y, editScript.wayPoint[i].z));
                    }

                    //위치 제거
                    GUI.backgroundColor = new Color(2, 0.5f, 0.2f);
                    if (GUI.Button(new Rect(HandleUtility.WorldToGUIPoint(editScript.wayPoint[i]).x, HandleUtility.WorldToGUIPoint(editScript.wayPoint[i]).y - 40, 80, 20), "Remove", buttonStyle))
                    {
                        editScript.wayPoint.RemoveAt(i);
                    }
                    Handles.EndGUI();

                }
            }

            Handles.color = new Color(2, 0.5f, 0.2f, 5);
            Vector3[] Vector3Array = new Vector3[editScript.wayPoint.Count+1];
            for (int i = 0; i < editScript.wayPoint.Count; i++)
            {
                Vector3Array[i] = editScript.wayPoint[i];
            }
            Vector3Array[editScript.wayPoint.Count] = editScript.wayPoint[0];

            Handles.DrawAAPolyLine(10, Vector3Array.Length, Vector3Array);
        }
        else
        {
            editScript.wayPoint = new List<Vector3>();
        }
    }


    //인스펙터 GUI
    ReorderableList reorderableList;
    void OnEnable()
    {
        //타겟 컴포넌트
        editScript = (EnemySearch)target;

        //리스트 갯수 받아올 데이터
        reorderableList = new ReorderableList(serializedObject,
                                 serializedObject.FindProperty("wayPoint"));

        reorderableList.drawHeaderCallback = (Rect rect) => 
        {
            EditorGUI.LabelField(rect, "wayPoint");
        };

        //리스트 안에 GUI구성
        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => 
        {
            editScript.wayPoint[index] = EditorGUI.Vector3Field(rect, index.ToString(), editScript.wayPoint[index]);
        };

        //리스트에 +버튼 눌렀을 때 처리
        reorderableList.onAddCallback += (list) => 
        {
            if (editScript.wayPoint.Count > 0)
            {
                editScript.wayPoint.Add(editScript.wayPoint[editScript.wayPoint.Count - 1]);
            }
            else
            {
                editScript.wayPoint.Add(editScript.transform.position);
            }

        };

        //리스트에 -버튼 눌렀을 때 처리
        reorderableList.onRemoveCallback += (list) => 
        {
            editScript.wayPoint.RemoveAt(list.index);
        };
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        hideEditor = GUILayout.Toggle(hideEditor, "에디터 데이터 숨기기");

        //초기화 버튼
        if (GUILayout.Button("ResetPath"))
        {
            editScript.wayPoint.Clear();
        }
        reorderableList.DoLayoutList();
    }

}
