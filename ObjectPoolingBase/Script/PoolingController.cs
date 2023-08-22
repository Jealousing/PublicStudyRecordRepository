using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example script to control object pooling
/// ������Ʈ Ǯ���� ��Ʈ���ϴ� ���� ��ũ��Ʈ 
/// </summary>
public class PoolingController : MonoBehaviour
{
    // Prefab for pooling
    // Ǯ���� ����� Prefab
    public GameObject ballPrefab;
    public GameObject cubePrefab;

    private void Update()
    {
        // Create Object
        // ������Ʈ ����
        if (Input.GetKey(KeyCode.Keypad1))
        {
            ObjectPooling.GetInstance.GetPool(ballPrefab);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            ObjectPooling.GetInstance.GetPool(cubePrefab);
        }

        // Disable Active Objects
        // Ȱ��ȭ�� ������Ʈ ��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ObjectPooling.GetInstance.ReleasePool(ballPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            ObjectPooling.GetInstance.ReleasePool(cubePrefab);
        }

        // Delete Pool
        // �ش� ������Ʈ�� Ǯ ����
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            ObjectPooling.GetInstance.RemovePool(ballPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            ObjectPooling.GetInstance.RemovePool(cubePrefab);
        }

        // Reset all objects
        // ��� ������Ʈ�� �缳��
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
