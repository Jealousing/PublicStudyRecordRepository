using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowController : MonoBehaviour
{
    public GameObject objectPoolParent;
    public GameObject effectPrefab;
    public Rigidbody arrowRigidbody;
    public TrailRenderer trailRenderer;
    float timer;
    public float releaseTime = 5.0f;
    public bool IsTrigger = false;
    IObjectPool<ArrowController> arrowPool;

    Vector3 previousPos;
    public GameObject Target;


    public AutomaticTrackingArrows automaticTrackingInfo;
    public int automaticTrackingNumber;
    public float count = 1;

    private void Awake()
    {
        trailRenderer=GetComponentInChildren<TrailRenderer>();
        arrowRigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        previousPos = transform.position;
        if (arrowRigidbody ==null )
        {
        }
        else if (arrowRigidbody.velocity.magnitude > 0.01f)
        {
            dirSet();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsTrigger) return;

        if(other.CompareTag("Enemy"))
        {
            RaycastHit hit;
            Vector3 raycastStart = transform.position;
            Vector3 raycastDirection = other.transform.position - transform.position;
            float raycastDistance = raycastDirection.magnitude;
            raycastDirection.Normalize();

            if (Physics.Raycast(raycastStart, raycastDirection, out hit, raycastDistance))
            {
                Vector3 collisionPoint = hit.point;
                EffectManager.GetInstance.poolGet(effectPrefab, collisionPoint);
            }
            else
            {
                Vector3 test = this.transform.position - arrowRigidbody.velocity.normalized * 1.5f;
                EffectManager.GetInstance.poolGet(effectPrefab, test);
            }
            other.GetComponent<EnemyInfo>().TakeDamage(100, previousPos);
            Reset();
            arrowPool.Release(this);
        }
    }


    private void Reset()
    {
        if(automaticTrackingInfo!=null)
        {
            count = 1;
            automaticTrackingInfo.arrow[automaticTrackingNumber] = null;
            automaticTrackingInfo = null;
        }

        arrowRigidbody.isKinematic = true;
        arrowRigidbody.useGravity = false;
        releaseTime = 5.0f;
        Target = null;
        timer = 0;
        trailRenderer.Clear();
        IsTrigger = false;
        trailRenderer.enabled = false;
        this.transform.parent = objectPoolParent.transform;
    }

    public void Remove()
    {
        Reset(); 
        arrowPool.Release(this);
    }


    public void dirSet()
    {
        timer += Time.deltaTime;
        if(timer> releaseTime)
        {
            timer = 0;
            Reset();
            arrowPool.Release(this);
            return;
        }
        transform.forward = arrowRigidbody.velocity.normalized;
        transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0));
    }


    public void Set(IObjectPool<ArrowController> pool)
    {
        arrowPool = pool;
    }
}
