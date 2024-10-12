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
        OnDataChanged(key, value); // 데이터가 변경되었을 때 이벤트 발생
    }

    // 데이터 변경 이벤트
    public event Action<string, object> DataChanged;

    // 이벤트를 발생시키는 메서드
    protected virtual void OnDataChanged(string key, object value)
    {
        DataChanged?.Invoke(key, value);
    }

    // 디버그용 
    public void DebugPrint()
    {
        Console.WriteLine("Blackboard Data:");
        foreach (var item in data)
        {
            Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
        }
    }
}
