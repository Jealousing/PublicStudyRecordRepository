using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public Slider progressBar;
    private float textUpdateInterval = 0.25f;
    private float lastTextUpdateTime = 0.0f;
    public TMP_Text loadText;
    int loadTextRepeat = 0;

    static string nextSceneName = "Main";
    static Vector3 targetPosition;
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }
    public static void LoadScene(string sceneName, Vector3 position)
    {
        nextSceneName = sceneName;
        targetPosition = position;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }

    public static string GetActiveScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        StartCoroutine(LoadLoadingScene());
    }


    // 씬 전환시 로딩화면을 띄움
    private IEnumerator LoadLoadingScene()
    {
        yield return null;
        PlayerInfo.GetInstance.transform.position = Vector3.zero;
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceneName);
        asyncOperation.allowSceneActivation = false;

        float startTime = 0;

        while (!asyncOperation.isDone)
        {
            yield return null;

            if (asyncOperation.progress >= 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            }
            else
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, asyncOperation.progress, Time.deltaTime);
            }

            // 1초마다 메시지 업데이트
            if (progressBar.value < 1f && Time.time - lastTextUpdateTime >= textUpdateInterval)
            {
                if (loadTextRepeat < 3)
                {
                    loadTextRepeat++;
                }
                else
                {
                    loadTextRepeat = 0;
                }

                loadText.text = "Loading".PadRight(7 + loadTextRepeat, '.');
                lastTextUpdateTime = Time.time;
            }

            if (progressBar.value >= 1f)
            {
                loadText.text = "Press SpaceBar";
            }

            if(progressBar.value>=1f &&asyncOperation.progress>=0.9f)
            {
                if(startTime ==0) startTime = Time.time;

                float progressPercentage = (Time.time - startTime) / 2f * 100f;
                loadText.text = $"Press SpaceBar or Auto Next...({progressPercentage:F0}%)";

                if (Input.GetKeyDown(KeyCode.Space) || Time.time - startTime >= 2f)
                {
                    if (targetPosition != Vector3.zero)
                    {
                        PlayerInfo.GetInstance.transform.position = targetPosition;
                    }
                    asyncOperation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

}
