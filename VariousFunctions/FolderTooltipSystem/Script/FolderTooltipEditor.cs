using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FolderTooltipEditor : EditorWindow
{
    private string folderPath;
    private string description;

    private static Dictionary<string, string> tooltips = new Dictionary<string, string>();
    private static string filePath = "Assets/Data/FolderTooltip/FolderTooltips.json";

    [MenuItem("Tools/Folder Tooltip")]
    public static void ShowWindow()
    {
        FolderTooltipEditor window = GetWindow<FolderTooltipEditor>("폴더 툴팁 편집");
        window.Show();
    }

    private void OnEnable()
    {
        LoadTooltips();
        UpdateSelectedFolder();
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        UpdateSelectedFolder();
        Repaint();
    }

    private void UpdateSelectedFolder()
    {
        if (Selection.activeObject != null)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path))
            {
                folderPath = path;
                description = GetTooltip(folderPath);
            }
        }
    }

    private void OnGUI()
    {
        if (string.IsNullOrEmpty(folderPath))
        { 
            return;
        }

        GUILayout.Label($"선택된 폴더: {folderPath}", EditorStyles.boldLabel);

        description = EditorGUILayout.TextArea(description, GUILayout.Height(80));

        if (GUILayout.Button(" 저장 "))
        {
            SetTooltip(folderPath, description);
            SaveTooltips();
        }
        GUILayout.Space(5);
         
        if (GUILayout.Button(" 정리 "))
        {
            RemoveInvalidTooltips();
            SaveTooltips(); 
        }
    }

    private static void LoadTooltips()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tooltips = JsonUtility.FromJson<SerializableDictionary>(json).ToDictionary();
        }
    }

    private static void SaveTooltips()
    {
        var json = JsonUtility.ToJson(new SerializableDictionary(tooltips));
        File.WriteAllText(filePath, json);
        AssetDatabase.Refresh();
    }
    private static void RemoveInvalidTooltips()
    {
        List<string> keysToRemove = new List<string>();

        foreach (var kvp in tooltips)
        {
            if (!Directory.Exists(kvp.Key) && !File.Exists(kvp.Key))
            {
                keysToRemove.Add(kvp.Key); 
            }
        } 

        foreach (string key in keysToRemove)
        {
            tooltips.Remove(key);
        }
    }
    private static string GetTooltip(string path)
    {
        return tooltips.ContainsKey(path) ? tooltips[path] : "";
    }

    private static void SetTooltip(string path, string tooltip)
    {
        tooltips[path] = tooltip;
    }

    [System.Serializable]
    public class SerializableDictionary
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();

        public SerializableDictionary() { }

        public SerializableDictionary(Dictionary<string, string> dictionary)
        {
            foreach (var kvp in dictionary)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < keys.Count; i++)
            {
                dictionary[keys[i]] = values[i];
            }
            return dictionary;
        }
    }
}
