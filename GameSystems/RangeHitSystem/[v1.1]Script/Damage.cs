using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public abstract class Damage : MonoBehaviour
{
    [NonSerialized] protected IObjectPool<Damage> pool;
    public void Set(IObjectPool<Damage> pool)
    {
        this.pool = pool;
    }

    [NonSerialized] protected Mesh baseMesh;
    [NonSerialized] protected Mesh fillMesh;
    [NonSerialized] protected Material baseMaterial;
    [NonSerialized] protected Material fillMaterial;

    [NonSerialized] protected float fillProgress = 0;
    [NonSerialized] protected float preFillProgress = -0.1f;

    [NonSerialized] protected float fillTime = 0;
    [NonSerialized] protected bool isDraw = false;
    [NonSerialized] protected bool isFill = false;
    [NonSerialized] protected bool isDamage = false;

    [NonSerialized] protected float tick = 0.01f;
    [NonSerialized] protected WaitForSeconds tickTimer = new WaitForSeconds(0.01f);

    [NonSerialized] protected int targetLayer;
    [NonSerialized] protected float minDistance = 5.0f;
    [NonSerialized] protected float maxDistance = 15.0f;
    [NonSerialized] protected float height = 0.0f;
    [NonSerialized] protected float width =  0.0f;
    [NonSerialized] protected float damage = 0.0f;
    [NonSerialized] protected float limitAngle = 0.0f;

    #region Method Chaining

    public Damage SetBaseMesh(Mesh mesh)
    {
        baseMesh = mesh;
        return this;
    }
    public Damage SetFillMesh(Mesh mesh)
    {
        fillMesh = mesh;
        return this;
    }
    public Damage SetBaseMaterial(Material material)
    {
        baseMaterial = material;
        return this;
    }
    public Damage SetFillMaterial(Material material)
    {
        fillMaterial = material;
        return this;
    }
    public Damage SetFillProgress(float progress)
    {
        fillProgress = progress;
        return this;
    }
    public Damage SetPreFillProgress(float progress)
    {
        preFillProgress = progress;
        return this;
    }
    public Damage SetFillTime(float time)
    {
        fillTime = time;
        return this;
    }
    public Damage SetIsDraw(bool draw)
    {
        isDraw = draw;
        return this;
    }
    public Damage SetIsFill(bool fill)
    {
        isFill = fill;
        return this;
    }
    public Damage SetIsDamage(bool damage)
    {
        isDamage = damage;
        return this;
    }
    public Damage SetTick(float tickValue)
    {
        tick = tickValue;
        tickTimer = new WaitForSeconds(tickValue);
        return this;
    }
    public Damage SetTargetLayer(int layer)
    {
        targetLayer = layer;
        return this;
    }
    public Damage SetMinDistance(float distance)
    {
        minDistance = distance;
        return this;
    }
    public Damage SetMaxDistance(float distance)
    {
        maxDistance = distance;
        return this;
    }
    public Damage SetHeight(float heightValue)
    {
        height = heightValue;
        return this;
    }
    public Damage SetWidth(float widthValue)
    {
        width = widthValue;
        return this;
    }
    public Damage SetDamage(float damageValue)
    {
        damage = damageValue;
        return this;
    }
    public Damage SetLimitAngle(float angle)
    {
        limitAngle = angle;
        return this;
    }
    public Damage SetPos(Vector3 pos)
    {
        this.transform.position = pos;
        return this;
    }
    public Damage SetRot(Vector3 rot)
    {
        this.transform.rotation = Quaternion.identity;
        this.transform.Rotate(rot);
        return this;
    }
    public Damage SetRot(Quaternion rot)
    {
        this.transform.rotation = rot;
        return this;
    }
    public Damage Build()
    {
        // 필요한 검증 또는 추가 로직을 수행할 수 있다.

        if(isDraw)
        {
            this.StartDraw();
        }
        else
        {
            this.StartDamage();
        }
        return this;
    }

    #endregion

    // 메테리얼과 메쉬 정리
    protected abstract void SetMeshAndMaterial();

    // 그리기
    public abstract void StartDraw();
    public abstract void StopDraw();
    protected abstract void Draw();
    
    // 시간에 따른 그리기 관리
    protected abstract IEnumerator DrawShape();

    // 데미지 관련
    public abstract void StartDamage();
    public abstract List<GameObject> CheckObject();
    protected abstract IEnumerator CheckObjectDamage();

    // 시작과 활성비활성시 메테리얼 재설정
    private void Start()
    {
        SetMeshAndMaterial();
    }

    private void OnEnable()
    {
        SetMeshAndMaterial();
    }
    
    private void Update()
    {
        if (isDraw)
        { 
            Draw();
        }
    }
}
