using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from the default class to create a Singleton
// 기본 클래스에서 상속하여 싱글톤을 만듭니다
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = null;
    static bool isClosing = false;
    static object lockObject = new object();

    public static T GetInstance
    {
        get
        {
            // Thread Lock
            // 쓰레드 잠금
            lock (lockObject) 
            {
                if (isClosing)
                {
                    Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    return null;
                }

                if (instance == null)
                {
                    // Verify that an instance exists
                    // 인스턴스 있는지 확인
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        isClosing = true;
    }

    private void OnDestroy()
    {
        isClosing = true;
    }

}
