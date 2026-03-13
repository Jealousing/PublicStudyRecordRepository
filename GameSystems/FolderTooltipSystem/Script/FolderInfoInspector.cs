using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(DefaultAsset))]
public class FolderInfoInspector  : Editor
{
    private static string filePath = "Assets/Data/FolderTooltip/FolderTooltips.json";
    private static Dictionary<string, string> tooltips = new Dictionary<string, string>();
    private static Dictionary<string, Texture2D> previewCache = new Dictionary<string, Texture2D>();
    private static Dictionary<string, string[]> directoryCache = new Dictionary<string, string[]>();
    private static Dictionary<string, string[]> fileCache = new Dictionary<string, string[]>();
    private static double cacheTimestamp;
    private const double cacheDuration = 10.0; // 캐시 유효 시간 (초)

    private string folderPath;
    private string tooltip;

    static FolderInfoInspector ()
    {
        LoadTooltips(); 
    }

    public override void OnInspectorGUI()
    {
        folderPath = AssetDatabase.GetAssetPath(target); 
         
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            tooltip = tooltips.ContainsKey(folderPath) ? tooltips[folderPath] : ""; 
            EditorGUILayout.TextArea(tooltip, GUILayout.Height(40));


            EditorGUILayout.Space(10); 
            EditorGUILayout.LabelField("하위 폴더 및 에셋 목록", EditorStyles.boldLabel);
            DisplaySubfoldersAndAssets(folderPath);
        }
        else
        {
            base.OnInspectorGUI();
        }
    }

    private void DisplaySubfoldersAndAssets(string path)
    { 
        if (EditorApplication.timeSinceStartup - cacheTimestamp > cacheDuration)
        {
            ClearCache();
            cacheTimestamp = EditorApplication.timeSinceStartup;
        } 
         
        // 하위 폴더 표시
        string[] subfolders = Directory.GetDirectories(path); 
        if (!directoryCache.ContainsKey(path))
        {
            directoryCache[path] = Directory.GetDirectories(path);
        }
        foreach (string subfolder in subfolders)
        {
            // 상대 경로를 AssetDatabase 형식으로 변환
            string relativePath = AssetDatabase.GetAssetPath(AssetDatabase.LoadAssetAtPath<DefaultAsset>(subfolder));
            string subfolderName = Path.GetFileName(subfolder);
            string subfolderTooltip = tooltips.ContainsKey(relativePath) ? tooltips[relativePath] : "";

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"[Folder]  {subfolderName}", GUILayout.Width(200));
            EditorGUILayout.LabelField($": {subfolderTooltip}");
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);

        
        // 폴더 내 파일 표시 및 미리보기 추가
        string[] files = Directory.GetFiles(path);
        if (!directoryCache.ContainsKey(path))
        {
            directoryCache[path] = Directory.GetDirectories(path);
        }
        int displayedCount = 0;
        foreach (string file in files)
        {
            if (displayedCount >= 10) break;
            string relativeFilePath = AssetDatabase.GetAssetPath(AssetDatabase.LoadAssetAtPath<Object>(file));
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(relativeFilePath);

            if (asset != null)
            {  
                // 미리보기 이미지 캐싱 및 출력
                if (!previewCache.TryGetValue(relativeFilePath, out Texture2D preview))
                {
                    preview = AssetPreview.GetAssetPreview(asset) ?? AssetPreview.GetMiniThumbnail(asset);
                    previewCache[relativeFilePath] = preview;
                } 
                 
                if (preview != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(Path.GetFileName(file), GUILayout.Width(200));
                    GUILayout.Label(preview, GUILayout.Width(50), GUILayout.Height(50));
                    EditorGUILayout.EndHorizontal();

                    displayedCount++;
                }
                else
                {
                    //GUILayout.Label("(미리보기 없음)", GUILayout.Width(50), GUILayout.Height(50));
                } 
            }
        } 
    }
     
    private static void ClearCache()
    {
        directoryCache.Clear();
        fileCache.Clear();
        previewCache.Clear();
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
