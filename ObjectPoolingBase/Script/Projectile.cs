using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Script to use for object pool example
/// 오브젝트풀 예제에 이용할 스크립트
/// </summary>
public class Projectile : MonoBehaviour
{
    public Rigidbody rigid;
    public Renderer render;

    IObjectPool<Projectile> pool;
    public GameObject keyPrefab;
    public bool isActive=false;

    float speed = 15f;
    private float spawnRange = 5.0f;
    public bool IsRelease = false;

    public void Fire()
    {
        transform.position = Random.insideUnitSphere * spawnRange;

        Vector3 direction = Random.onUnitSphere; 
        direction.y = Mathf.Abs(direction.y); 
        rigid.velocity = direction * speed;


        render.material.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
        Invoke(nameof(Release), 5f);
    }

    public void Setup(Color color)
    {
        render.material.color = color;
    }

    public void Set(IObjectPool<Projectile> pool , GameObject prefab )
    {
        this.pool = pool;
        this.keyPrefab = prefab;
        IsRelease = false;
    }

    /// <summary>
    /// Function that calls for return
    /// 반환을 위해 호출하는 함수
    /// </summary>
    public void Release()
    {
        if(!IsRelease)
        {
            CancelInvoke();
            IsRelease = true;
            pool.Release(this);
        }
    }
}
