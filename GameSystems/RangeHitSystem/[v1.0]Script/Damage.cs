using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public abstract class Damage : MonoBehaviour
{
    [NonSerialized] public IObjectPool<Damage> pool;
    public void Set(IObjectPool<Damage> pool)
    {
        this.pool = pool;
    }

    [NonSerialized] public Mesh baseMesh;
    [NonSerialized] public Mesh fillMesh;
    [NonSerialized] public Material baseMaterial;
    [NonSerialized] public Material fillMaterial;

    [NonSerialized] public float fillProgress = 0;
    [NonSerialized] public float preFillProgress = -0.1f;

    [NonSerialized] public float fillTime = 0;
    [NonSerialized] public bool isDraw = false;
    [NonSerialized] public bool isFill = false;
    [NonSerialized] public bool isDamage = false;

    
    [NonSerialized] public float tick = 0.01f;
    [NonSerialized] public WaitForSeconds tickTimer = new WaitForSeconds(0.01f);

    [NonSerialized] public int targetLayer;
    [NonSerialized] public float minDistance = 5.0f;
    [NonSerialized] public float maxDistance = 15.0f;
    [NonSerialized] public float height = 0.0f;
    [NonSerialized] public float width =  0.0f;
    [NonSerialized] public float damage = 0.0f;
    [NonSerialized] public float limitAngle = 0.0f;

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
