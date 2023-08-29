using UnityEngine;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    protected DataManager() {}

    private const string saveFolderName = "Saves";
    private string saveFolderPath;

    private void Awake()
    {
        // ���� ���� ��� ����
        saveFolderPath = Path.Combine(Application.dataPath, saveFolderName);
        //Application.dataPath + "/Saves/"; -> Path.Combine(Application.dataPath, saveFolderName)���� ���� 
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    private string GetFilePath(string fileName, string addPath = "")
    {
        return Path.Combine(saveFolderPath, addPath, fileName + ".json");
    }

    // ���Ͽ� ������ ����
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

    // ���Ͽ��� ������ �ε�
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
