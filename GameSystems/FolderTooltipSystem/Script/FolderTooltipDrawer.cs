using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[InitializeOnLoad]
public class FolderTooltipDrawer
{
    private static string filePath = "Assets/Data/FolderTooltip/FolderTooltips.json";
    private static Dictionary<string, string> tooltips = new Dictionary<string, string>();

    static FolderTooltipDrawer()
    {
        LoadTooltips();
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }
     
    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
         
        if (AssetDatabase.IsValidFolder(path) && tooltips.ContainsKey(path))
        {
            string tooltip = tooltips[path];
             
            if (selectionRect.Contains(Event.current.mousePosition))
            {
                GUI.Label(new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height), new GUIContent("", tooltip));
            }
        }
    }

    private static void LoadTooltips()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tooltips = JsonUtility.FromJson<FolderTooltipEditor.SerializableDictionary>(json).ToDictionary();
        }
    }
}
