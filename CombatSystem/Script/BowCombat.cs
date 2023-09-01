using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BowCombat : CombatAbstract
{
    // �� ��忡 �ʿ��� �ð� , �߻���ġ ����
    public float zoomTime = 0.5f;
    public float minShotTime = 1.15f;

    // Ȱ����
    public Animator bowAni;

    // ȭ�쿡 ���� ����
    public ArrowController arrowObject; // ���� ������� ȭ��
    public GameObject arrowPrefab;
    public IObjectPool<ArrowController> arrowPool;

    [HideInInspector] public Vector3 defaultArrowPos;
    [HideInInspector] public Vector3 defaultArrowRot;

    public Camera playerCamera;

    private bool isAiming = false;
    bool isShot = false;

    // Ÿ�̸�
    float deltaTime;
    private float timer = 0f;
    float delayTime = 0.25f;
    float delayTimer;
    

    private GameObject target = null;
   
    // aim ������ ���� ��
    private Transform followAim;
    private Transform basicAim;

    // �ִϸ��̼� ������ ���� �÷��̾� ĳ������ �� ����
    private Transform spine; // �ƹ�Ÿ�� �㸮
    private Transform head; // �ƹ�Ÿ�� �Ӹ�
    private Transform leftUpperArm;

    // �ִϸ��̼� hash��
    int aniHashBow;
    int aniHashLeftTurn;
    int aniHashRightTurn;
    [HideInInspector] public int aniHashBowString;
    [HideInInspector] public int aniHashBowZoom;
    

    // ȸ���ؾߵǴ� ��
    float rotatedAngle = 70.0f;

    private void Awake()
    {
        arrowPool = new ObjectPool<ArrowController>(CreateArrow, OnGetArrow, OnReleaseArrow, OnDestroyArrow, maxSize: 5);
    }
    private void Start()
    {
        aniHashBow = Animator.StringToHash("IsBow");
        aniHashLeftTurn = Animator.StringToHash("LowerBodyLeftTurn");
        aniHashRightTurn = Animator.StringToHash("LowerBodyRightTurn");
        aniHashBowString = Animator.StringToHash("BowTrigger");
        aniHashBowZoom = Animator.StringToHash("Zoom");
        StartCoroutine(Setting());
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isAiming && timer > minShotTime)
        {
            // ������ ��ġ ����
            combatInfo.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            combatInfo.animator.SetIKPosition(AvatarIKGoal.RightHand, arrowObject.GetComponent<ArrowController>().trailRenderer.transform.position);
        }
    }

    private void OnDestroy()
    {
        combatInfo.animator.SetBool(aniHashBow, false);
    }

    private void LateUpdate()
    {
        BasicAttackSystem();
    }

    IEnumerator TurnAni(string dir , float angle)
    {
        // ȸ�� Ʈ����
        if (dir == "Left")
        {
            angle -= 15.0f;
            combatInfo.animator.SetTrigger(aniHashLeftTurn);
        }
        else
        {
            angle += 35.0f;
            combatInfo.animator.SetTrigger(aniHashRightTurn);
        }

        aniName = dir + "Turn";

        // ����ɶ����� ���
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(2).IsName(aniName))
        {
            yield return null;
        }
        Quaternion initialRotation = this.transform.rotation; ; 
        Quaternion targetRotation= initialRotation *Quaternion.Euler(0f, angle, 0f);

        while (combatInfo.animator.GetCurrentAnimatorStateInfo(2).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(2).normalizedTime <= 0.1f)
        {
            yield return null;
        }
        while (combatInfo.animator.GetCurrentAnimatorStateInfo(2).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(2).normalizedTime <= 1f)
        {
            this.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, 
                combatInfo.animator.GetCurrentAnimatorStateInfo(2).normalizedTime-0.1f);
            yield return null;
        }
        combatInfo.playerInfo.actionLock = false;
        combatInfo.playerInfo.movementInfo.inputLock = false;
    }

    IEnumerator bowAniTrigger(float timer)
    {
        yield return new WaitForSeconds(timer);
        bowAni.SetTrigger(aniHashBowString);
    }

    IEnumerator ShotArrow()
    {
        if (arrowObject == null) yield break;

        isAiming = false;
        combatInfo.animator.SetTrigger(combatInfo.aniHashAttack);
        combatInfo.animator.SetFloat(combatInfo.aniHashAniMult, 2f);
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("ShotArrow (BowCombat)"))
        {
            yield return null;
        }
        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("ShotArrow (BowCombat)") &&
           combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.5f)
        {
            yield return null;
        }
        if (arrowObject != null) Fire(arrowObject);
        combatInfo.playerInfo.movementInfo.inputLock = false;

        while(isShot)
        {
            yield return null;
        }
        combatInfo.animator.SetFloat(combatInfo.aniHashAniMult, 1f);
        sibartest = false;
    }

    bool sibartest = false;
    IEnumerator ShotStart()
    {
        if (arrowObject != null) yield break;
        if (sibartest) yield break;
        sibartest = true;


        combatInfo.animator.SetTrigger(aniHashBowZoom);
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("DrawArrow (BowCombat)"))
        {
            yield return null;
        }

        // ����Ȱ��ȭ
        isAiming = true;
        isShot = true;
        timer = 0;
        timer += deltaTime;

        // movement ����
        combatInfo.playerInfo.movementInfo.moveState.ChangeState(MoveState.AimingMovement);

        // aim �������� y�� ȸ��
        Vector3 direction = followAim.position - transform.position;
        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
        transform.rotation = targetQuaternion;

        arrowObject = arrowPool.Get();
        arrowObject.transform.parent = combatInfo.playerInfo.leftWeapon.transform;
        arrowObject.transform.localPosition = defaultArrowPos;
        arrowObject.transform.localRotation = Quaternion.Euler(defaultArrowRot);

    }


    bool flag = false;
    Vector3 zoomPos;
    protected override void BasicAttackSystem()
    {
        deltaTime = Time.deltaTime;
        if (isShot && !isAiming)
        {
            delayTimer += deltaTime;
            if (delayTimer>delayTime)
            {
                delayTimer = 0;
                isShot = false;
            }
        }


        if (combatInfo&& !combatInfo.IsCombat) return;

        // ���콺 Ŭ���� ���ظ�� Ȱ��ȭ
        if (Input.GetMouseButtonDown(0) && !isAiming &&!isShot)
        {
            flag = false;
            StartCoroutine(ShotStart());
        }

        // ������ Ȱ��ȭ �Ǿ��ְ� ���� �ð��� ������ ī�޶� ���ظ��� ���� �� ī�޶� �ٶ󺸴¹����� ĳ���Ͱ� �ٶ󺸸� �� ������ ����
        if (isAiming && arrowObject != null)
        {

            timer += deltaTime;
            followAim.position = Vector3.Lerp(followAim.position, basicAim.position, deltaTime * 10.0f);
            followAim.rotation = Quaternion.Lerp(followAim.rotation, basicAim.rotation, deltaTime * 10.0f);

            if (timer > zoomTime)
            {
                combatInfo.playerInfo.cameraInfo.CameraStateSystem.ChangeState(CameraState.BowZoom);
                combatInfo.Crosshair.SetActive(true);

                
                if (timer>minShotTime)
                {
                    // �㸮ȸ������ ��ü ȸ�� �� �Ӹ� ���� ����
                    spine.LookAt(followAim.position);
                    head.LookAt(followAim.position);
                    leftUpperArm.LookAt(followAim.position);

                    spine.rotation = spine.rotation * Quaternion.Euler(new Vector3(0, 90, 0));
                    head.rotation = head.rotation * Quaternion.Euler(new Vector3(0, -60, 0));
                    if(combatInfo.playerInfo.movementInfo.moveVec.x == -1)
                    {
                        leftUpperArm.rotation = leftUpperArm.rotation * Quaternion.Euler(new Vector3(-97.5f, 90, 30));
                    }
                    else
                    {
                        leftUpperArm.rotation = leftUpperArm.rotation * Quaternion.Euler(new Vector3(-97.5f, 90, 0));
                    }
                    zoomPos = followAim.position;
                }

                // �ٶ󺸴� ���� ray (Test)
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.yellow);
                Debug.DrawRay(arrowObject.transform.position, (followAim.position - arrowObject.transform.position).normalized * 100f, Color.magenta);

            }


            // �������� ������ ���¹������� ȸ��
            if (combatInfo.playerInfo.movementInfo.moveVec != Vector2.zero)
            {
                Vector3 direction = followAim.position - transform.position;
                float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
                transform.rotation = targetQuaternion;
            }

            // �÷��̾�� ������ y�� ������ ���
            Vector3 playerAngles = this.transform.rotation.eulerAngles;
            Vector3 aimAngles = followAim.rotation.eulerAngles;
            float angleDifference = Mathf.DeltaAngle(playerAngles.y, aimAngles.y);

            // ������ �����̻� �������� �� �������� ȸ�� ���
            if (angleDifference < -rotatedAngle)
            {
                if (combatInfo.playerInfo.actionLock) return;
                combatInfo.playerInfo.actionLock = true;
                combatInfo.playerInfo.movementInfo.inputLock = true;
                StartCoroutine(TurnAni("Left", angleDifference));
            }
            else if (angleDifference > rotatedAngle)
            {
                if (combatInfo.playerInfo.actionLock) return;
                combatInfo.playerInfo.actionLock = true;
                combatInfo.playerInfo.movementInfo.inputLock = true;
                StartCoroutine(TurnAni("Right", angleDifference));
            }
        }

        // ���콺���� ��ư���� ���� ��� ���ؽð��� ����� ��� �����Ѱ����� �߻�
        // ª�� ��� ��ó Ÿ�ٿ��� �߻� -> ������ ����߻�
        if (Input.GetMouseButtonUp(0)) flag = true;
        if ((Input.GetMouseButtonUp(0) || flag) && isAiming && arrowObject != null)
        {
            // movement ����
            combatInfo.playerInfo.movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            combatInfo.playerInfo.movementInfo.inputLock = true;
            combatInfo.Crosshair.SetActive(false);
            combatInfo.animator.ResetTrigger(aniHashBowZoom);

            arrowObject.GetComponent<ArrowController>().trailRenderer.enabled = true;
            if (bowAni != null)
            {
                StartCoroutine(bowAniTrigger(0.015f));
            }



            combatInfo.playerInfo.cameraInfo.CameraStateSystem.ChangeState(CameraState.BackView);

            // ���� �ð��� ����� �� ���, ������ �������� ȭ���� �߻��մϴ�.
            if (isAiming && timer > minShotTime)
            {
                // aim �������� y�� ȸ��
                Vector3 direction = followAim.position - transform.position;
                float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
                transform.rotation = targetQuaternion;

                StartCoroutine(ShotArrow());
                return;
            }
            else
            {
                // ���� �ð��� ª���� ���, ����� ���͸� Ÿ������ �����ϰ� �߻��մϴ�.
                PlayerInfo.GetInstance.CheckAndRotateToTarget(10.0f);
                StartCoroutine(ShotArrow());
                return;
            }


        }

    }


    private void Fire(ArrowController arrowobj)
    {
        arrowobj.transform.parent = arrowobj.objectPoolParent.transform;
        arrowobj.IsTrigger = true;
        //arrowobj.arrowRigidbody = arrowobj.gameObject.AddComponent<Rigidbody>();
        arrowobj.arrowRigidbody.isKinematic = false;
        arrowobj.arrowRigidbody.freezeRotation = true;
        arrowobj.arrowRigidbody.useGravity = true;

        Vector3 dir;
        if (timer > minShotTime)
            dir = (zoomPos - arrowObject.transform.position).normalized;
        else if (target)
            dir = (target.transform.position - arrowObject.transform.position).normalized;
        else
            dir = transform.forward;

        arrowobj.arrowRigidbody.velocity = dir * 60f;
        arrowobj.dirSet();
        arrowObject = null;
        target = null;
    }


    IEnumerator Setting()
    {
        yield return null;

        if (combatInfo == null)
        {
            combatInfo = GetComponent<CombatInfo>();
            yield return null;
        }

        if (combatInfo.animator == null)
        {
            combatInfo.animator = GetComponent<Animator>();
            yield return null;
        }

        combatInfo.animator.SetBool(aniHashBow, true);
        playerCamera = combatInfo.playerInfo.movementInfo.mainCamera.GetComponent<Camera>();
        spine = combatInfo.animator.GetBoneTransform(HumanBodyBones.Spine);
        head = combatInfo.animator.GetBoneTransform(HumanBodyBones.Head);
        basicAim = playerCamera.transform.GetChild(0);
        //aim = playerCamera.transform.GetChild(0);
        bowAni = combatInfo.playerInfo.leftWeapon.transform.GetChild(0).GetComponent<Animator>();
        arrowPrefab = combatInfo.playerInfo.leftWeapon.transform.Find("Arrow").gameObject;
        defaultArrowPos = arrowPrefab.transform.localPosition;
        defaultArrowRot = arrowPrefab.transform.localRotation.eulerAngles;

        leftUpperArm = combatInfo.animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);

        followAim = new GameObject().GetComponent<Transform>();
        followAim.position = basicAim.position;
        followAim.rotation = basicAim.rotation;
        followAim.name = "Aim";
        followAim.parent = playerCamera.transform;
    }


    private ArrowController CreateArrow()
    {
        ArrowController arrow = Instantiate(arrowPrefab).GetComponent<ArrowController>();

        arrow.transform.parent = combatInfo.playerInfo.leftWeapon.transform;
        arrow.Set(arrowPool);
        return arrow;
    }

    private void OnGetArrow(ArrowController arrow)
    {
        arrow.gameObject.SetActive(true);
    }

    private void OnReleaseArrow(ArrowController arrow)
    {
        arrow.gameObject.SetActive(false);
    }

    private void OnDestroyArrow(ArrowController arrow)
    {
        Destroy(arrow.gameObject);
    }
}
    