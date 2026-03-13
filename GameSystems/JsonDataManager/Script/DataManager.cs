using UnityEngine;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    protected DataManager() {}

    private const string saveFolderName = "Saves";
    private string saveFolderPath;

    private void Awake()
    {
        // 폴더 저장 경로 설정
        saveFolderPath = Path.Combine(Application.dataPath, saveFolderName);
        //Application.dataPath + "/Saves/"; -> Path.Combine(Application.dataPath, saveFolderName)으로 변경 
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    private string GetFilePath(string fileName, string addPath = "")
    {
        return Path.Combine(saveFolderPath, addPath, fileName + ".json");
    }

    // 파일에 데이터 저장
    public void SaveData<T>(T data, string fileName, string addPath = "")
    {
        string filePath = GetFilePath(fileName, addPath);
        string directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonString);
    }

    // 파일에서 데이터 로드
    public T LoadData<T>(string fileName, string addPath = "")
    {
        string filePath = GetFilePath(fileName, addPath);
        string directoryPath = Path.GetDirectoryName(filePath);
        if (Directory.Exists(directoryPath))
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(jsonString);
            }
        }
        return default;
    }

}
