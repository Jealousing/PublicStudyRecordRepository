using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 OnInspectorGUI을 먼저 호출
        base.OnInspectorGUI();

        // 간격 조절
        GUILayout.Space(10);

        // 스킬데이터를 json으로 저장하기위한 버튼 생성 및 눌리면 저장
        if (GUILayout.Button("Save Data"))
        {
            // 스킬데이터 받기
            SkillData skillData = (SkillData)target;

            skillData.skillInfo.OnBeforeSerialize();

            // 저장방식 json
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
