using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added after Unity 21 version
// 유니티 21버전부터 추가
using UnityEngine.Pool;


/// <summary>
/// Example script created to study object pooling
/// 오브젝트 풀링을 공부하기 위해서 만들어본 예제 스크립트 
/// </summary>
public class ObjectPooling : Singleton<ObjectPooling>
{
    // Pooling Information Management Variables Using Dictionary and Tuple
    // 딕셔너리와 튜플을 이용한 풀링정보 관리 변수
    private Dictionary<GameObject,(IObjectPool<Projectile> ,List<Projectile>)> poolInfo;
    private GameObject tempPrefab;

    private void Awake()
    {
        poolInfo = new Dictionary<GameObject, (IObjectPool<Projectile>, List<Projectile>)>();
    }

    /// <summary>
    /// Create a Tuple(Value) for GameObject(Key)
    /// 해당 오브젝트(Key)에 맞는  Tuple(Value) 만들기 
    /// </summary>
    public void CreatePool(GameObject prefab)
    {
        poolInfo.Add(prefab, (new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, 
            OnDestroyProjectile, maxSize: 300), new List<Projectile>()));
    }

    /// <summary>
    /// Get an instance from a pool that fits GameObject(Key)
    /// 해당 오브젝트(Key)에 맞는 풀에서 인스턴스를 가져온다 
    /// </summary>
    public void GetPool(GameObject prefab)
    {
        // Verify that the key has the correct Dictionary -> Create if it does not
        // key에 맞는 Dictionary가 있는지 확인 -> 없으면 생성
        tempPrefab = prefab;
        if (!poolInfo.ContainsKey(prefab))
        {
            CreatePool(prefab);
        }

        // Get an instance from the pool that fits the key and fire it
        // key에 맞는 pool에서 instance를 가져와 발사
        poolInfo[prefab].Item1.Get().Fire();
    }

    /// <summary>
    /// Returns all active objects that match that GameObject(Key) to the pool.
    /// 해당 오브젝트(Key)에 맞는 활성화된 오브젝트 전부를 풀에 반환합니다.
    /// </summary>
    public void ReleasePool(GameObject prefab)
    {
        // Check Dictionary and verify that there are activity objects.
        // Dictionary 존재 확인 및 활동 객체가 있는지 확인.
        if (poolInfo.ContainsKey(prefab) && poolInfo[prefab].Item2.Count !=0)
        {
            for (int i = poolInfo[prefab].Item2.Count - 1; i >= 0; i--)
            {
                poolInfo[prefab].Item2[i].Release();
            }
        }
    }

    /// <summary>
    /// Removes the information that matches the GameObject(Key).
    /// 해당 오브젝트(Key)에 맞는 정보를 제거합니다.
    /// </summary>
    public void RemovePool(GameObject prefab)
    {
        if (poolInfo.ContainsKey(prefab))
        {
            ReleasePool(prefab);
            poolInfo[prefab].Item1.Clear();
            poolInfo[prefab].Item2.Clear();
            poolInfo.Remove(prefab);
        }
    }

    /// <summary>
    /// Gets the List of enabled objects for that GameObject(Key).
    /// 해당 오브젝트(Key)에 맞는 활성화된 오브젝트의 리스트를 가져옵니다.
    /// </summary>
    public List<Projectile> GetActiveObject(GameObject prefab)
    {
        if (poolInfo.ContainsKey(prefab))
            return poolInfo[prefab].Item2;
        else
            return null;
    }


    #region IObjectPool 
    // Functions used by ObjectPool.
    // ObjectPool에서 사용하는 함수들입니다.

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(tempPrefab).GetComponent<Projectile>();
        projectile.transform.parent = this.transform;
        projectile.Set(poolInfo[tempPrefab].Item1, tempPrefab);
        return projectile;
    }
    private void OnGetProjectile(Projectile projectile)
    {
        if (poolInfo.ContainsKey(projectile.keyPrefab))
        {
            poolInfo[projectile.keyPrefab].Item2.Add(projectile);
        }
        projectile.gameObject.SetActive(true);
        projectile.IsRelease = false;

    }
    private void OnReleaseProjectile(Projectile projectile)
    {
        if (poolInfo.ContainsKey(projectile.keyPrefab))
        {
            poolInfo[projectile.keyPrefab].Item2.Remove(projectile);
        }
        projectile.gameObject.SetActive(false);
    }
    private void OnDestroyProjectile(Projectile projectile)
    {
        if (poolInfo.ContainsKey(projectile.keyPrefab))
        {
            poolInfo[projectile.keyPrefab].Item2.Remove(projectile);
        }
        Destroy(projectile.gameObject);
    }
    #endregion

}
