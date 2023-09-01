using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // �⺻ OnInspectorGUI�� ���� ȣ��
        base.OnInspectorGUI();

        // ���� ����
        GUILayout.Space(10);

        // ��ų�����͸� json���� �����ϱ����� ��ư ���� �� ������ ����
        if (GUILayout.Button("Save Data"))
        {
            // ��ų������ �ޱ�
            SkillData skillData = (SkillData)target;

            skillData.skillInfo.OnBeforeSerialize();

            // ������ json
            string json = JsonUtility.ToJson(skillData.skillInfo);
            string path = Application.dataPath + "/Saves/"+ "/SkillData/"+ skillData.skillInfo.skillName + ".json";

            if (!string.IsNullOrEmpty(path))
            {
                System.IO.File.WriteAllText(path, json);
            }
        }

        GUILayout.Space(1);
        if (GUILayout.Button("Load Data"))
        {
            SkillData skillData = (SkillData)target;
            string path = Application.dataPath + "/Saves/" + "/SkillData/" + skillData.name+ ".json";
            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                skillData.skillInfo = JsonUtility.FromJson<SkillInfo>(json);

                skillData.skillInfo.OnAfterDeserialize();
            }
        }


    }
}
