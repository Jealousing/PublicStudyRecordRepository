using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from the default class to create a Singleton
// 기본 클래스에서 상속하여 싱글톤을 만듭니다
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static bool isClosing = false;
    private static readonly object lockObject = new object();

    public static T GetInstance
    {
        get
        {
            // 더블 체크로 여러스레드가 액세스 시도할 경우 발생할 수 있는 문제 방지
            if (instance == null)
            {
                // Thread Lock
                // 쓰레드 잠금 , 유령 객체 생성 방지
                lock (lockObject)
                {
                    // 인스턴스 있는지 확인 및 여러 스레드가 액세스를 시도하더라도 애플리케이션 종료 중에 인스턴스 생성이 방지.
                    if (instance == null && !isClosing)
                    {
                        instance = (T)FindObjectOfType(typeof(T));

                        if (instance == null)
                        {
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                        DontDestroyOnLoad(singletonObject);
                        }
                    }
                }
            }
             return instance;
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
