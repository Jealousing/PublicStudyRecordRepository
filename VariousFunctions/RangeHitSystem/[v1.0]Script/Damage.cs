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

    // ���׸���� �޽� ����
    protected abstract void SetMeshAndMaterial();

    // �׸���
    public abstract void StartDraw();
    public abstract void StopDraw();
    protected abstract void Draw();
    
    // �ð��� ���� �׸��� ����
    protected abstract IEnumerator DrawShape();

    // ������ ����
    public abstract void StartDamage();
    public abstract List<GameObject> CheckObject();
    protected abstract IEnumerator CheckObjectDamage();

    // ���۰� Ȱ����Ȱ���� ���׸��� �缳��
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
