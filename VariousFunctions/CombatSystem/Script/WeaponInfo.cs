using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 무기정보
/// </summary>
public class WeaponInfo : MonoBehaviour
{
    public WeaponType weaponType;
    [NonSerialized] public bool wearDirLeft = false;

    [Serializable]
    public struct SetTr
    {
        public Vector3 pos;
        public Vector3 rot;
    }

    // 전투상태 위치
    public SetTr combatTr;

    // 비전투상태 위치
    public SetTr wearTr;

    float damage = 100.0f;
    public bool IsPickupState = false;

    public GameObject lightObj;
    public GameObject particleObj;
    public TrailRenderer trilRenderer;

    [NonSerialized] public CapsuleCollider capsuleCollider;
    public Quaternion defaultrot;

    LayerMask weapon;
    private Vector3 originalPosition;
    public GameObject effectPrefab;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        defaultrot = this.transform.rotation;
        weapon = LayerMask.NameToLayer("Weapon");
        originalPosition = transform.position;
        switch (weaponType)
        {
            case WeaponType.ONEHAND_SWORD:
            case WeaponType.ONEHAND_AXE:
            case WeaponType.ONEHAND_WAND:
            case WeaponType.ONEHAND_ETC:
            case WeaponType.TWOHAND_SWORD:
            case WeaponType.TWOHAND_AXE:
            case WeaponType.TWOHAND_STAFF:
            case WeaponType.TWOHAND_ETC:
                wearDirLeft = false;
                break;
            case WeaponType.SHIELD:
            case WeaponType.BOW:
                wearDirLeft = true;
                break;
        }
    }

    private void Update()
    {
        // 드랍 상태일때 위아래 진동과 좌우회전
        if(transform.gameObject.layer == weapon)
        {
            float shakeAmount = Mathf.Sin(Time.time * 5.0f) * 0.1f;
            transform.position = originalPosition + new Vector3(0f, shakeAmount, 0f);
            transform.Rotate(Vector3.up * 30.0f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 아이템 줍기(무기)
    /// </summary>
    public void PickUp(PlayerInfo playerInfo)
    {
        if (IsPickupState) return;

        IsPickupState = true;
        bool IsLeftWeapon;
        bool IsTwoHand;
        (IsLeftWeapon, IsTwoHand) = CheckType(this);
        StartCoroutine(CheckWeapon(playerInfo, IsLeftWeapon, IsTwoHand));

    }

    public 

    /// <summary>
    /// 무기 타입 구하기 (왼손에 착용하는지, 양손무기인지)
    /// </summary>
    (bool,bool) CheckType(WeaponInfo test)
    {
        WeaponType weaponWearType = test.weaponType;
        switch (weaponWearType)
        {
            case WeaponType.ONEHAND_SWORD:
            case WeaponType.ONEHAND_AXE:
            case WeaponType.ONEHAND_WAND:
            case WeaponType.ONEHAND_ETC:
                return (false, false);
            case WeaponType.TWOHAND_SWORD:
            case WeaponType.TWOHAND_AXE:
            case WeaponType.TWOHAND_STAFF:
            case WeaponType.TWOHAND_ETC:
                return (false, true);
            case WeaponType.SHIELD:
                return (true, false);
            case WeaponType.BOW:
                return (true, true);
            default:
                return (false, false);
        }
    }

    /// <summary>
    /// 무기를 착용할 수 있는 상태인지 확인 및 버리기 착용 처리
    /// </summary>
    IEnumerator CheckWeapon(PlayerInfo playerInfo, bool IsLeftWear, bool isTwoHand)
    {
        while (playerInfo.actionLock) yield return null;
        playerInfo.actionLock = true;

        // 두손으로 착용하는 아이템
        if (isTwoHand)
        {
            // 두손 전부 아이템 존재
            if(playerInfo.leftWeapon !=null && playerInfo.rightWeapon!=null)
            {
                // 두손이 참조하고 있는 오브젝트가 같은 인스턴스인지 확인
                if(object.ReferenceEquals(playerInfo.leftWeapon,playerInfo.rightWeapon))
                {
                    StartCoroutine(DropWeapon(playerInfo, playerInfo.leftWeapon.GetComponent<WeaponInfo>().wearDirLeft));
                    yield break;
                }
                // 다른 아이템이면 둘다 버리기
                StartCoroutine(DropWeapon(playerInfo, false));
                StartCoroutine(DropWeapon(playerInfo, true));
                yield break; ;
            }
            // 왼손에만 아이템 존재
            else if (playerInfo.leftWeapon != null)
            {
                StartCoroutine(DropWeapon(playerInfo, true));
                yield break; ;
            }
            // 오른손에만 아이템 존재
            else if(playerInfo.rightWeapon != null)
            {
                StartCoroutine(DropWeapon(playerInfo, false));
                yield break; ;
            }
            else
            {
                StartCoroutine(WearWeapon(playerInfo, IsLeftWear, isTwoHand));
                yield break; ;
            }
        }
        // 왼손 착용무기
        else if(IsLeftWear)
        {
            // 한손에 장착 해야되지만 양손에 무기가 있고 만약 그 양손 무기가 같은무기를 참조하는 양손으로 다루는 무기일 경우
            if (playerInfo.leftWeapon != null && playerInfo.rightWeapon != null)
            {
                if (object.ReferenceEquals(playerInfo.leftWeapon, playerInfo.rightWeapon))
                {
                    StartCoroutine(DropWeapon(playerInfo, playerInfo.leftWeapon.GetComponent<WeaponInfo>().wearDirLeft));
                    yield break; ;
                }
            }

            // 왼손에 무기가 착용되어있는지 확인
            if (playerInfo.leftWeapon !=null)
            {
                // 버리기
                StartCoroutine(DropWeapon(playerInfo, true));
                yield break; ;
            }
            // 착용하기
            StartCoroutine(WearWeapon(playerInfo, IsLeftWear, isTwoHand));
            yield break; ;
        }
        else
        {
            // 한손에 장착 해야되지만 양손에 무기가 있고 만약 그 양손 무기가 같은무기를 참조하는 양손으로 다루는 무기일 경우
            if (playerInfo.leftWeapon != null && playerInfo.rightWeapon != null)
            {
                if (object.ReferenceEquals(playerInfo.leftWeapon, playerInfo.rightWeapon))
                {
                    StartCoroutine(DropWeapon(playerInfo, playerInfo.leftWeapon.GetComponent<WeaponInfo>().wearDirLeft));
                    yield break; ;
                }
            }

            // 오른손에 무기가 착용되어있는지 확인
            if (playerInfo.rightWeapon !=null)
            {
                // 버리기
                StartCoroutine(DropWeapon(playerInfo,false));
                yield break; ;
            }
            // 착용하기
            StartCoroutine(WearWeapon(playerInfo, IsLeftWear, isTwoHand));
            yield break; ;
        }



    }

    /// <summary>
    /// 무기 버리기
    /// </summary>
    IEnumerator DropWeapon(PlayerInfo playerInfo, bool isLeftDrop)
    {
        string animationname;
        Vector3 forceDir;
        float dropTimer = 0.0f;
        float power = 4f;
        WeaponInfo dropWeapon;

        playerInfo.movementInfo.inputLock = true;
        GameObject weapon;
        if(isLeftDrop)
        {
            weapon = playerInfo.leftWeapon;
        }
        else
        {
            weapon = playerInfo.rightWeapon;
        }

        bool isTwoHand = false;

        switch (weapon.GetComponent<WeaponInfo>().weaponType)
        {
            case WeaponType.TWOHAND_SWORD:
            case WeaponType.TWOHAND_AXE:
            case WeaponType.TWOHAND_STAFF:
            case WeaponType.TWOHAND_ETC:
                isTwoHand = true;
                break;
            case WeaponType.BOW:
                isTwoHand = true;
                break;

        }


        if (isLeftDrop)
        {
            dropWeapon =  playerInfo.leftWeapon.GetComponent<WeaponInfo>();
            playerInfo.movementInfo.animator.SetTrigger(playerInfo.movementInfo.aniHashDropItemL);
            playerInfo.leftWeapon = null;
            if (isTwoHand)
                playerInfo.rightWeapon = null;
            animationname = "DropItemL";
            forceDir = playerInfo.movementInfo.handIK.leftHand.transform.forward + Vector3.up* 2.0f;
        }
        else
        {
            dropWeapon =  playerInfo.rightWeapon.GetComponent<WeaponInfo>();
            playerInfo.movementInfo.animator.SetTrigger(playerInfo.movementInfo.aniHashDropItemR);
            playerInfo.rightWeapon = null;
            if (isTwoHand)
                playerInfo.leftWeapon = null;
            animationname = "DropItemR";
            forceDir = playerInfo.movementInfo.handIK.rightHand.transform.forward + Vector3.up*2.0f;
        }

        playerInfo.combatInfo.checkWeaponScript();

        while (!playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(animationname))
        {
            yield return null;
        }

        while (playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(animationname) &&
            playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.45f)
        {
            yield return null;
        }
        Rigidbody rb= dropWeapon.gameObject.AddComponent<Rigidbody>();
        dropWeapon.transform.parent = null;
        dropWeapon.capsuleCollider.enabled = true;
        dropWeapon.capsuleCollider.isTrigger = false;
        yield return null;
        rb.useGravity = true;
        rb.AddForce(forceDir.normalized* power, ForceMode.Impulse);

        playerInfo.actionLock = false;

        while (playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(animationname) &&
           playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }
       
        playerInfo.movementInfo.inputLock = false;
        
        int ignoreLayer = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player") |
            1 << LayerMask.NameToLayer("Weapon")) ;
        float radius = ( dropWeapon.capsuleCollider.radius )*2;

        while (!Physics.CheckSphere(dropWeapon.transform.position, radius + 0.1f, ignoreLayer)
        && dropTimer<5.0f )
        {
            dropTimer += Time.deltaTime;
            yield return null;
        }

        dropWeapon.IsPickupState = false;
        IsPickupState = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        dropWeapon.capsuleCollider.isTrigger = true;
        dropWeapon.transform.gameObject.layer = LayerMask.NameToLayer("Weapon");
        dropWeapon.transform.rotation = dropWeapon.defaultrot;
        dropWeapon.originalPosition = dropWeapon.transform.position+new Vector3(0,0.1f,0);

        if (lightObj != null)
            dropWeapon.lightObj.SetActive(true);
        if (particleObj != null)
            dropWeapon.particleObj.SetActive(true);

        yield return null;


        // 5초 낙하가 발생했을 경우 예외처리 레이가 닿는 땅으로 이동
        RaycastHit raycastHit;
        if(dropTimer>=5.0f && Physics.Raycast(dropWeapon.transform.position,Vector3.down, out raycastHit,15f, ignoreLayer,QueryTriggerInteraction.Ignore))
        {
            dropWeapon.transform.position = raycastHit.point;
        }
    }

    /// <summary>
    /// 착용 무기
    /// </summary>
    IEnumerator WearWeapon(PlayerInfo playerInfo, bool isLeftWear, bool isTwoHand)
    {

        Vector3 lookDir = this.transform.position - playerInfo.transform.position;
        lookDir.y = playerInfo.transform.position.y;
        playerInfo.transform.rotation = Quaternion.LookRotation(lookDir).normalized;

        playerInfo.movementInfo.inputLock = true;

        // 줍기 애니메이션 실행
        playerInfo.movementInfo.animator.SetTrigger(playerInfo.movementInfo.aniHashTakeItem);

        // 실행될때까지 대기
        while(!playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("TakeItem"))
        {
            yield return null;
        }

        while (playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("TakeItem") &&
            playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.25f)
        {
            yield return null;
        }

        this.transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    
        if (lightObj != null)
            lightObj.SetActive(false);
        if (particleObj != null)
            particleObj.SetActive(false);

        // 종료시점
        while (playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("TakeItem") &&
            playerInfo.movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f)
        {
            yield return null;
        }

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        Destroy(rb);

        if (isLeftWear)
        {
            
            // 전투상태
            if (playerInfo.combatInfo.IsCombat)
            {
                this.transform.SetParent(playerInfo.movementInfo.handIK.leftHand.transform);
                this.transform.localPosition = combatTr.pos;
                this.transform.localRotation = Quaternion.Euler(combatTr.rot);
            }
            // 비전투상태
            else
            {
                this.transform.SetParent(playerInfo.wearPoint.transform);
                this.transform.localPosition = wearTr.pos;
                this.transform.localRotation = Quaternion.Euler(wearTr.rot);
            }
            playerInfo.leftWeapon = this.gameObject;
            if(isTwoHand)
                playerInfo.rightWeapon = this.gameObject;
        }
        else
        {
            // 전투상태
            if (playerInfo.combatInfo.IsCombat)
            {
                this.transform.SetParent(playerInfo.movementInfo.handIK.rightHand.transform);
                this.transform.localPosition = combatTr.pos;
                this.transform.localRotation = Quaternion.Euler(combatTr.rot);
            }
            // 비전투상태
            else
            {
                this.transform.SetParent(playerInfo.wearPoint.transform);
                this.transform.localPosition = wearTr.pos;
                this.transform.localRotation = Quaternion.Euler(wearTr.rot);
            }
           
            playerInfo.rightWeapon = this.gameObject;
            if (isTwoHand)
                playerInfo.leftWeapon = this.gameObject;
        }
        this.capsuleCollider.enabled = false;
        playerInfo.movementInfo.inputLock = false;
        playerInfo.actionLock = false;
        playerInfo.combatInfo.checkWeaponScript();

        yield return null;
    }


    void OnTriggerEnter(Collider other)
    {
        // 적일 경우 대미지 적용
        if (other.gameObject.tag == "Enemy")
        {
            RaycastHit hit;
            Vector3 raycastStart = transform.position;
            Vector3 raycastDirection = other.transform.position - transform.position;
            float raycastDistance = raycastDirection.magnitude;
            raycastDirection.Normalize();

            if (Physics.Raycast(raycastStart, raycastDirection, out hit, raycastDistance))
            {
                Vector3 collisionPoint = Utils.GetRandomPointInCube(hit.point, 0.5f, 2f, 0.5f);
                EffectManager.GetInstance.poolGet(effectPrefab, collisionPoint);
            }
            other.gameObject.GetComponent<EnemyInfo>().TakeDamage(damage);
        }
    }


}
