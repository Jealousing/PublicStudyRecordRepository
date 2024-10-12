using System;
using System.Collections.Generic;

public class Blackboard 
{
    private Dictionary<string, object> data = new Dictionary<string, object>();
     
    public T Get<T>(string key)
    {
        if (data.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        return default(T);
    }
     
    public void Set<T>(string key, T value)
    {
        data[key] = value;
        OnDataChanged(key, value); // �����Ͱ� ����Ǿ��� �� �̺�Ʈ �߻�
    }

    // ������ ���� �̺�Ʈ
    public event Action<string, object> DataChanged;

    // �̺�Ʈ�� �߻���Ű�� �޼���
    protected virtual void OnDataChanged(string key, object value)
    {
        DataChanged?.Invoke(key, value);
    }

    // ����׿� 
    public void DebugPrint()
    {
        Console.WriteLine("Blackboard Data:");
        foreach (var item in data)
        {
            Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
        }
    }
}
