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

        // ��ư�� ũ�⸦ ������ �ٿ��� �� �ٿ� ǥ��
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("MenuCount : " + wheelMenu.menuCount.ToString(),
            GUILayout.Width(EditorGUIUtility.currentViewWidth/3f));

        // + ��ư ����
        if (GUILayout.Button("+", GUILayout.Width(EditorGUIUtility.currentViewWidth / 4f)))
        {
            wheelMenu.IncreaseItemCount();
        }

        // - ��ư ����
        if (GUILayout.Button("-", GUILayout.Width(EditorGUIUtility.currentViewWidth / 4f)))
        {
            wheelMenu.DecreaseItemCount();
        }

        EditorGUILayout.EndHorizontal();

        // Clear ��ư ����
        if (GUILayout.Button("Reset", GUILayout.Width(EditorGUIUtility.currentViewWidth)))
        {
            wheelMenu.Reset();
        }
    }
}
#endif