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
            //�⺻ ��Ʈ ��Ÿ��
            stringStyle.normal.textColor = Color.green;
            stringStyle.fontStyle = FontStyle.Bold;
            stringStyle.fontSize = 30;
            stringStyle.alignment = TextAnchor.MiddleCenter;

            //��ư ��Ÿ��

            GUIStyle buttonStyle = new(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.fontSize = 15;
            buttonStyle.alignment = TextAnchor.MiddleCenter;


            for (int i = 0; i < editScript.wayPoint.Count; i++)
            {
                if (!hideEditor)
                {
                    editScript.wayPoint[i] = Handles.PositionHandle(editScript.wayPoint[i], Quaternion.identity); //����� ǥ��
                    Handles.Label(editScript.wayPoint[i] + new Vector3(0, -0.25f, 0), i.ToString(), stringStyle); //�迭 ���� ǥ��


                    Handles.BeginGUI();
                    //��ġ �߰�
                    GUI.backgroundColor = new Color(0.3f, 0.5f, 2);
                    if (GUI.Button(new Rect(HandleUtility.WorldToGUIPoint(editScript.wayPoint[i]).x, HandleUtility.WorldToGUIPoint(editScript.wayPoint[i]).y - 65, 80, 20), "Add", buttonStyle))
                    {
                        editScript.wayPoint.Insert(i, new Vector3(editScript.wayPoint[i].x, editScript.wayPoint[i].y, editScript.wayPoint[i].z));
                    }

                    //��ġ ����
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


    //�ν����� GUI
    ReorderableList reorderableList;
    void OnEnable()
    {
        //Ÿ�� ������Ʈ
        editScript = (EnemySearch)target;

        //����Ʈ ���� �޾ƿ� ������
        reorderableList = new ReorderableList(serializedObject,
                                 serializedObject.FindProperty("wayPoint"));

        reorderableList.drawHeaderCallback = (Rect rect) => 
        {
            EditorGUI.LabelField(rect, "wayPoint");
        };

        //����Ʈ �ȿ� GUI����
        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => 
        {
            editScript.wayPoint[index] = EditorGUI.Vector3Field(rect, index.ToString(), editScript.wayPoint[index]);
        };

        //����Ʈ�� +��ư ������ �� ó��
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

        //����Ʈ�� -��ư ������ �� ó��
        reorderableList.onRemoveCallback += (list) => 
        {
            editScript.wayPoint.RemoveAt(list.index);
        };
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        hideEditor = GUILayout.Toggle(hideEditor, "������ ������ �����");

        //�ʱ�ȭ ��ư
        if (GUILayout.Button("ResetPath"))
        {
            editScript.wayPoint.Clear();
        }
        reorderableList.DoLayoutList();
    }

}
