using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BowCombat : CombatAbstract
{
    // 줌 모드에 필요한 시간 , 발사위치 조정
    public float zoomTime = 0.5f;
    public float minShotTime = 1.15f;

    // 활정보
    public Animator bowAni;

    // 화살에 대한 변수
    public ArrowController arrowObject; // 현재 대기중인 화살
    public GameObject arrowPrefab;
    public IObjectPool<ArrowController> arrowPool;

    [HideInInspector] public Vector3 defaultArrowPos;
    [HideInInspector] public Vector3 defaultArrowRot;

    public Camera playerCamera;

    private bool isAiming = false;
    bool isShot = false;

    // 타이머
    float deltaTime;
    private float timer = 0f;
    float delayTime = 0.25f;
    float delayTimer;
    

    private GameObject target = null;
   
    // aim 추적을 위한 값
    private Transform followAim;
    private Transform basicAim;

    // 애니메이션 수정을 위한 플레이어 캐릭터의 뼈 변수
    private Transform spine; // 아바타의 허리
    private Transform head; // 아바타의 머리
    private Transform leftUpperArm;

    // 애니메이션 hash값
    int aniHashBow;
    int aniHashLeftTurn;
    int aniHashRightTurn;
    [HideInInspector] public int aniHashBowString;
    [HideInInspector] public int aniHashBowZoom;
    

    // 회전해야되는 값
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
            // 오른팔 위치 조정
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
        // 회전 트리거
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

        // 실행될때까지 대기
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

        // 에임활성화
        isAiming = true;
        isShot = true;
        timer = 0;
        timer += deltaTime;

        // movement 변경
        combatInfo.playerInfo.movementInfo.moveState.ChangeState(MoveState.AimingMovement);

        // aim 방향으로 y축 회전
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

        // 마우스 클릭시 조준모드 활성화
        if (Input.GetMouseButtonDown(0) && !isAiming &&!isShot)
        {
            flag = false;
            StartCoroutine(ShotStart());
        }

        // 조준이 활성화 되어있고 일정 시간이 지나면 카메라를 조준모드로 변경 및 카메라가 바라보는방향을 캐릭터가 바라보며 그 쪽으로 조준
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
                    // 허리회전으로 상체 회전 및 머리 방향 조정
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

                // 바라보는 방향 ray (Test)
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.yellow);
                Debug.DrawRay(arrowObject.transform.position, (followAim.position - arrowObject.transform.position).normalized * 100f, Color.magenta);

            }


            // 움직임이 있으면 보는방향으로 회전
            if (combatInfo.playerInfo.movementInfo.moveVec != Vector2.zero)
            {
                Vector3 direction = followAim.position - transform.position;
                float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
                transform.rotation = targetQuaternion;
            }

            // 플레이어와 에임의 y축 각도를 계산
            Vector3 playerAngles = this.transform.rotation.eulerAngles;
            Vector3 aimAngles = followAim.rotation.eulerAngles;
            float angleDifference = Mathf.DeltaAngle(playerAngles.y, aimAngles.y);

            // 각도가 일정이상 벌어지면 그 방향으로 회전 명령
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

        // 마우스에서 버튼업을 했을 경우 조준시간이 길었을 경우 조준한곳으로 발사
        // 짧은 경우 근처 타겟에게 발사 -> 없으면 정면발사
        if (Input.GetMouseButtonUp(0)) flag = true;
        if ((Input.GetMouseButtonUp(0) || flag) && isAiming && arrowObject != null)
        {
            // movement 변경
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

            // 조준 시간이 충분히 길 경우, 조준한 방향으로 화살을 발사합니다.
            if (isAiming && timer > minShotTime)
            {
                // aim 방향으로 y축 회전
                Vector3 direction = followAim.position - transform.position;
                float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
                transform.rotation = targetQuaternion;

                StartCoroutine(ShotArrow());
                return;
            }
            else
            {
                // 조준 시간이 짧았을 경우, 가까운 몬스터를 타겟으로 설정하고 발사합니다.
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
    