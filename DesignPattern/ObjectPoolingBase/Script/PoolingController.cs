using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example script to control object pooling
/// 오브젝트 풀링을 컨트롤하는 예시 스크립트 
/// </summary>
public class PoolingController : MonoBehaviour
{
    // Prefab for pooling
    // 풀링에 사용할 Prefab
    public GameObject ballPrefab;
    public GameObject cubePrefab;

    private void Update()
    {
        // Create Object
        // 오브젝트 생성
        if (Input.GetKey(KeyCode.Keypad1))
        {
            ObjectPooling.GetInstance.GetPool(ballPrefab);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            ObjectPooling.GetInstance.GetPool(cubePrefab);
        }

        // Disable Active Objects
        // 활성화된 오브젝트 비활성화
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ObjectPooling.GetInstance.ReleasePool(ballPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            ObjectPooling.GetInstance.ReleasePool(cubePrefab);
        }

        // Delete Pool
        // 해당 오브젝트의 풀 삭제
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            ObjectPooling.GetInstance.RemovePool(ballPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            ObjectPooling.GetInstance.RemovePool(cubePrefab);
        }

        // Reset all objects
        // 모든 오브젝트를 재설정
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            List<Projectile> activeList = ObjectPooling.GetInstance.GetActiveObject(ballPrefab);
            if (activeList != null)
                activeList.ForEach(x => x.Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1)));
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            List<Projectile> activeList = ObjectPooling.GetInstance.GetActiveObject(cubePrefab);
            if (activeList !=null)
                activeList.ForEach(x => x.Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1)));
        }
    }
}
