using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(RadialMenuSlotBuilder))]
public class RadialMenuSlotBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RadialMenuSlotBuilder wheelMenu = (RadialMenuSlotBuilder)target;

        EditorGUILayout.Space();

        // 버튼의 크기를 반으로 줄여서 한 줄에 표시
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("MenuCount : " + wheelMenu.menuCount.ToString(),
            GUILayout.Width(EditorGUIUtility.currentViewWidth/3f));

        // + 버튼 생성
        if (GUILayout.Button("+", GUILayout.Width(EditorGUIUtility.currentViewWidth / 4f)))
        {
            wheelMenu.IncreaseItemCount();
        }

        // - 버튼 생성
        if (GUILayout.Button("-", GUILayout.Width(EditorGUIUtility.currentViewWidth / 4f)))
        {
            wheelMenu.DecreaseItemCount();
        }

        EditorGUILayout.EndHorizontal();

        // Clear 버튼 생성
        if (GUILayout.Button("Reset", GUILayout.Width(EditorGUIUtility.currentViewWidth)))
        {
            wheelMenu.Reset();
        }
    }
}
#endif