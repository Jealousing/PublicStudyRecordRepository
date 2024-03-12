using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class WorldChunk : MonoBehaviour
{
    #region 오브젝트풀링
    private IObjectPool<WorldChunk> pool;
    public void Set(IObjectPool<WorldChunk> pool) => this.pool = pool;

    protected bool released = false;
    public void Release()
    {
        if (!released)
        {
            released = true; 
            pool.Release(this);
        }
    }
    #endregion

    public ChunkPos chunkPos;
    public GameObject ground;
    public GameObject ocean;

    protected Renderer groundRenderer;
    protected Material groundMaterial;


    public abstract void Init();
    public abstract void Set(ChunkPos pos);

}
