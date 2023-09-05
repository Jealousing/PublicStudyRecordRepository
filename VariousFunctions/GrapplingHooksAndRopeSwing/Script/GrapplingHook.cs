using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spring
{
    public float strength;
    public float damper;
    public float target;
    public float velocity;
    public float value;

    public void Update(float deltaTime)
    {
        var direction = target - value >= 0 ? 1f : -1f;
        var force = Mathf.Abs(target - value) * strength;
        velocity += (force * direction - velocity * damper) * deltaTime;
        value += velocity * deltaTime;
    }

    public void Reset()
    {
        velocity = 0f;
        value = 0f;
    }
}


// Hook Rope�� �ʿ��� ������
[Serializable]
public class Hook
{
    public RaycastHit hitRay;

    public Vector3 cameraToHitDir;
    public Vector3 handToHitDir;

    public LineRenderer lineRenderer;

    public bool IsHook = false;
    public bool IsRope = false;

    public GameObject handObj;
    public Spring spring;
    public Vector3 currentRopePosition;
}

public class GrapplingHook : MonoBehaviour
{
    // �߻� �� ���� ������ ���� ����Ʈ
    [SerializeField] Hook[] hooklist = new Hook[2];
    MovementInfo controller;
    Transform cameraTr;
    Vector3 playerToHitDir;

    public float radius = 5.0f;
    float timer;
    public float maxDistance = 100.0f;
    public float maxHookPower;
    float currentHookPower;

    RaycastHit rayhit;

    [SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;

    public AnimationCurve affectCurve;
    float ropeWidth = 0.05f;
    public int lineCount;
    public float waveHeight;
    public float waveCount;
    public float waveSpeed;
    public float ropeSpeed;
    public float damper;
    public float strength;

    bool startFlag = false;
    bool stopFlag = false;
    bool endFlag = false;
    bool IsForwardMove = false;


    int maxCount = 300;
    float maxSpeed = 100.0f;

    #region Event Functions

    void Start()
    {
        controller = GetComponent<MovementInfo>();
        cameraTr = GetComponent<CameraInfo>().cameraObj.transform;

        hooklist[0] = new Hook();
        hooklist[1] = new Hook();

        hooklist[0].handObj = leftHand;
        hooklist[1].handObj = rightHand;

        hooklist[0].lineRenderer = leftHand.GetComponent<LineRenderer>();
        hooklist[1].lineRenderer = rightHand.GetComponent<LineRenderer>();

        hooklist[0].lineRenderer.startWidth = ropeWidth;
        hooklist[0].lineRenderer.endWidth = ropeWidth;
        hooklist[1].lineRenderer.startWidth = ropeWidth;
        hooklist[1].lineRenderer.endWidth = ropeWidth;

        hooklist[0].spring = new Spring();
        hooklist[1].spring = new Spring();

        hooklist[0].spring.target = 0;
        hooklist[1].spring.target = 0;
    }


    void Update()
    {
        /*
         �߻�(Ready to launch): R
         �� �������� �̵��ϴ� �� : ��Ŭ�� (LB click)
         �� ������ �̿��� ���� (Swing) : ��Ŭ�� (RB click)
         */
        if (Input.GetKeyDown(KeyCode.R))
        {
            HookRay();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ClearHook();
        }

        if (Input.GetMouseButton(0))
        {
            stopFlag = false;
            if (!startFlag)
            {
                startFlag = true;
                StartCoroutine(HookPointMoveCoroutine());
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            stopFlag = true;
        }

        if (Input.GetMouseButton(1))
        {
            stopFlag = false;
            if (!startFlag)
            {
                startFlag = true;
                StartCoroutine(SlerpMoveCoroutine());
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            stopFlag = true;
        }

    }
    private void LateUpdate()
    {
        DrawRope();
    }

    #endregion

    /// <summary>
    /// ���� Hook�� ������ ���� �����ϴ� �Լ�
    /// Function to set based on the information in the incoming Hook
    /// </summary>
    /// <param name="hook_"> �޾ƿ� ���� ���� </param>
    void SetHookList(RaycastHit hook_)
    {
        //Ÿ���� ����
        //Direction of the target
        Vector3 targetDir = (hook_.point - this.transform.position).normalized;

        // ����-> �������� ��Ȯ�� ����� ���� Ȯ�ΰ��� 
        // Cross Product -> Inner Product for accurate orientation and angle.
        Vector3 cross = Vector3.Cross(this.transform.forward, targetDir);
        float Angle = Vector3.Dot(Vector3.up, cross) * Mathf.Rad2Deg;

        if (Angle > 0)
        {
            hooklist[1].IsHook = false;
            hooklist[1].IsRope = false;
            hooklist[1].currentRopePosition = hooklist[1].handObj.transform.position;
            hooklist[1].lineRenderer.positionCount = 0;
            hooklist[1].spring.Reset();
            hooklist[1].hitRay = hook_;
            UpdateDir(hooklist[1]);
            SetIkHand(1);
            StartCoroutine(LineRendererSetCoroutine(1));
        }
        else
        {
            hooklist[0].IsHook = false;
            hooklist[0].IsRope = false;
            hooklist[0].currentRopePosition = hooklist[0].handObj.transform.position;
            hooklist[0].lineRenderer.positionCount = 0;
            hooklist[0].spring.Reset();
            hooklist[0].hitRay = hook_;
            UpdateDir(hooklist[0]);
            SetIkHand(0);
            StartCoroutine(LineRendererSetCoroutine(0));
        }
        controller.handIK.ChangeState(HandIkState.GRAPPLINGHOOK);
    }

    /// <summary>
    /// �� ����Ʈ�� ��ȣ�� ���� ���� hook�� �� �غ� �Ǿ������� ���η������� �����ϴ� �Լ�
    /// Function that sets up a linelander when the arm is ready to shoot hook according to the number in the hooklist.
    /// </summary>
    /// <param name="i"> ��ȣ�� ���� ���� ���� </param>
    /// <returns></returns>
    IEnumerator LineRendererSetCoroutine(int i)
    {
        if (i == 0)
        {
            while (true)
            {
                if (controller.handIK.isLeftHandReady)
                    break;

                yield return null;
            }
        }
        else if (i == 1)
        {
            while (true)
            {
                if (controller.handIK.isRightHandReady)
                    break;

                yield return null;
            }
        }

        hooklist[i].IsHook = true;
        hooklist[i].lineRenderer.positionCount = lineCount + 1;
        hooklist[i].spring.velocity =waveSpeed;
        hooklist[i].spring.damper= damper;
        hooklist[i].spring.strength =strength;

        while (true)
        {
            if (hooklist[i].lineRenderer.positionCount == 0)
                yield break;

            if (Vector3.Distance(hooklist[i].hitRay.point, hooklist[i].lineRenderer.GetPosition(lineCount)) < 0.2f)
            {
                hooklist[i].IsRope = true;
                break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// ray ���� ������Ʈ 
    /// update ray direction
    /// </summary>
    /// <param name="hook"> �޾ƿ� ���� ���� </param>
    void UpdateDir(Hook hook)
    {
        hook.cameraToHitDir = hook.hitRay.point - cameraTr.transform.position;
        hook.handToHitDir = hook.hitRay.point - hook.handObj.transform.position;
    }

    /// <summary>
    /// ray�� �̿��� hook �߻�
    /// Launch hook using ray
    /// </summary>
    void HookRay()
    {
        Hook _hook = new Hook();

        // ���� �ȸ��� ��� ������ ���� �߰� ����
        if (Physics.Raycast(cameraTr.position, cameraTr.forward, out _hook.hitRay, maxDistance, LayerMask.NameToLayer("Player")))
        {
        }
        else
        {
            if (Physics.SphereCast(cameraTr.position, radius, cameraTr.forward, out _hook.hitRay, maxDistance, LayerMask.NameToLayer("Player")))
            {
            }
        }

        if (_hook.hitRay.collider)
        {
            SetHookList(_hook.hitRay);
        }
    }

    /// <summary>
    /// ȣ��Ǹ� �տ� ���� ������� Ȱ��ȭ�ϰ� ���� ��ġ�� ȸ�� ��ǥ ���� �����Ѵ�.
    /// When called, activate the inverse kinematics of the hand and set the hand position and rotation target value.
    /// </summary>
    void SetIkHand()
    {
        controller.handIK.leftPositionWeight = 1.0f;
        controller.handIK.leftRotationWeight = 1.0f;
        controller.handIK.rightPositionWeight = 1.0f;
        controller.handIK.rightRotationWeight = 1.0f;

        if (hooklist[0].IsHook && hooklist[1].IsHook)
        {
            controller.handIK.leftHandIKPositionTarget = hooklist[0].hitRay.point;
            controller.handIK.leftHandIKRotationTarget = hooklist[0].handToHitDir;
            controller.handIK.rightHandIKPositionTarget = hooklist[1].hitRay.point;
            controller.handIK.rightHandIKRotationTarget = hooklist[1].handToHitDir;

        }
        else
        {
            if (hooklist[0].IsHook)
            {
                controller.handIK.leftHandIKPositionTarget = hooklist[0].hitRay.point;
                controller.handIK.rightHandIKPositionTarget = hooklist[0].hitRay.point;
                controller.handIK.leftHandIKRotationTarget = hooklist[0].handToHitDir;
                controller.handIK.rightHandIKRotationTarget = hooklist[0].handToHitDir;
            }
            else if (hooklist[1].IsHook)
            {
                controller.handIK.leftHandIKPositionTarget = hooklist[1].hitRay.point;
                controller.handIK.rightHandIKPositionTarget = hooklist[1].hitRay.point;
                controller.handIK.leftHandIKRotationTarget = hooklist[1].handToHitDir;
                controller.handIK.rightHandIKRotationTarget = hooklist[1].handToHitDir;
            }
            else
            {

            }
        }
    }

    /// <summary>
    /// ȣ��Ǹ� ������ ������ �տ� ���� ������� Ȱ��ȭ�ϰ� ���� ��ġ�� ȸ�� ��ǥ ���� �����Ѵ�.
    /// When called, activate the reverse kinematics for the hand to modify the setting and set the hand position and rotation target value.
    /// </summary>
    /// <param name="i"> ������ ���� ��ȣ 0: left , 1:right </param>
    /// <returns></returns>
    bool SetIkHand(int i)
    {
        if (i == 0)
        {
            controller.handIK.leftPositionWeight = 1.0f;
            controller.handIK.leftRotationWeight = 1.0f;
            controller.handIK.leftHandIKPositionTarget = hooklist[i].hitRay.point;
            // ���� ��ġ�� ���������� ����� �����̵����� �ʹ� ���ڿ������� �� �̵��� ����
            /*   Vector3.Lerp(
                   leftUpperArm.transform.position, hooklist[0].hitRay.point,
                   PlayerController.GetInstance.handIK.leftArmLength / Vector3.Distance(leftUpperArm.transform.position, hooklist[0].hitRay.point)
                   );*/
            controller.handIK.leftHandIKRotationTarget = hooklist[i].handToHitDir;
            return controller.handIK.isLeftHandReady;
        }
        else if (i == 1)
        {
            controller.handIK.rightPositionWeight = 1.0f;
            controller.handIK.rightRotationWeight = 1.0f;
            controller.handIK.rightHandIKPositionTarget = hooklist[i].hitRay.point;
            /*Vector3.Lerp(
                rightUpperArm.transform.position, hooklist[1].hitRay.point,
                PlayerController.GetInstance.handIK.rightArmLength / Vector3.Distance(rightUpperArm.transform.position, hooklist[1].hitRay.point)
                );*/
            controller.handIK.rightHandIKRotationTarget = hooklist[i].handToHitDir;

            return controller.handIK.isRightHandReady;
        }

        return false;
    }

    /// <summary>
    /// Rope�� lineRenderer�� �׷��ش�.
    /// Draw Rope with a line renderer.
    /// </summary>
    void DrawRope()
    {
        for (int i = 0; i < hooklist.Length; i++)
        {
            if (hooklist[i].IsHook)
            {
                UpdateDir(hooklist[i]);
                Debug.DrawRay(cameraTr.position, hooklist[i].cameraToHitDir, Color.red);
                Debug.DrawRay(hooklist[i].handObj.transform.position, hooklist[i].handToHitDir, Color.blue);
                if (SetIkHand(i))
                {
                    hooklist[i].spring.Update(Time.deltaTime);
                    Vector3 up = Quaternion.LookRotation(hooklist[i].handToHitDir.normalized) * Vector3.up;
                    hooklist[i].currentRopePosition = Vector3.Lerp(hooklist[i].currentRopePosition, hooklist[i].hitRay.point, Time.deltaTime * ropeSpeed);

                    for (int j = 0; j < lineCount + 1; j++)
                    {
                        float delta = j / (float)lineCount;
                        Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * hooklist[i].spring.value * affectCurve.Evaluate(delta);

                        hooklist[i].lineRenderer.SetPosition(j, Vector3.Lerp(hooklist[i].handObj.transform.position, hooklist[i].currentRopePosition, delta) + offset);
                    }
                }

            }
        }


    }

    /// <summary>
    /// ���� �ʱ�ȭ (����ϱ���)
    /// Initialize hook
    /// </summary>
    void ClearHook()
    {
        for (int i = 0; i < hooklist.Length; i++)
        {
            hooklist[i].lineRenderer.positionCount = 0;
            hooklist[i].IsHook = false;
            hooklist[i].IsRope = false;

            hooklist[i].spring.Reset();
            hooklist[i].currentRopePosition = hooklist[i].handObj.transform.position;
        }

        controller.handIK.leftPositionWeight = 0.0f;
        controller.handIK.rightPositionWeight = 0.0f;

        controller.handIK.leftRotationWeight = 0.0f;
        controller.handIK.rightRotationWeight = 0.0f;
        controller.handIK.ChangeState(HandIkState.NORMAL);

    }

    /// <summary>
    /// ����� ���� ���� ���� �ʱ�ȭ �� �������� Ʈ���� Ȱ��ȭ
    /// Initialize used rope information and enable rope termination triggers.
    /// </summary>
    void ClearEndHook()
    {
        for (int i = 0; i < hooklist.Length; i++)
        {
            if (hooklist[i].IsRope)
            {
                hooklist[i].lineRenderer.positionCount = 0;
                hooklist[i].IsHook = false;
                hooklist[i].IsRope = false;

                hooklist[i].spring.Reset();
                hooklist[i].currentRopePosition = hooklist[i].handObj.transform.position;


                controller.handIK.leftPositionWeight = 0.0f;
                controller.handIK.leftRotationWeight = 0.0f;

                controller.handIK.rightPositionWeight = 0.0f;
                controller.handIK.rightRotationWeight = 0.0f;
            }
        }
        if (controller.animator.GetCurrentAnimatorStateInfo(0).IsName("RopeSwing"))
        {
            controller.animator.SetTrigger("GrapplinghookEnd");
        }
    }

    /// <summary>
    /// ���� �̿��� �̵� �Լ� ���� �����ؾߵǴ� �κ�
    /// The part that needs to be set when using the movement function using the hook.
    /// </summary>
    void HookMoveSetting()
    {
        SetIkHand();

        //�̵� �Ұ� ���·� ���� (Change to Not Moveable)
        controller.moveState.ChangeState(MoveState.NotMovement);
        //�߷� ���� (Gravity deletion)
        controller.player_rigidbody.useGravity = false;
    }

    /// <summary>
    /// �÷��� �� ���� �� �߷°� �ǵ�����
    /// Reset flag values and return gravity values.
    /// </summary>
    void SettingReset()
    {
        stopFlag = false;
        startFlag = false;
        controller.player_rigidbody.useGravity = true;
    }

    /// <summary>
    /// �뽬�� ������ �޾� ���� �ð����� ���� ������ �̵�
    /// Directed to dash and moved to the force received during the time received.
    /// </summary>
    /// <param name="direction"> �̵��� ���� </param>
    /// <param name="movePower"> �̵��� �� </param>
    /// <param name="targetTime"> �̵��� �ð�</param>
    /// <returns></returns>
    private IEnumerator DashCoroutine(Vector3 direction, float movePower, float targetTime)
    {
        float DashTimer = 0;

        ClearEndHook();
        controller.animator.SetBool("Grounded", false);
        while (DashTimer < targetTime)
        {
            DashTimer += Time.deltaTime;

            controller.player_rigidbody.MovePosition(this.transform.position + direction * movePower / 2 * Time.deltaTime);

            yield return null;
        }
        controller.moveState.ChangeState(MoveState.DefaultMovement);
        SettingReset();
        StartCoroutine(ForwardMoveCoroutine(movePower));
    }

    /// <summary>
    /// ���� �����Ҷ����� ������ ���ư��� �Լ�
    /// function that moves forward until it lands on the ground.
    /// </summary>
    /// <param name="movePower"> ������ �� </param>
    /// <returns></returns>
    IEnumerator ForwardMoveCoroutine(float movePower)
    {
        if (IsForwardMove) yield break;
        else IsForwardMove = true;

        while (!controller.isGround)
        {
            if (Physics.Raycast(this.transform.position + transform.up * 1f, this.transform.forward, 0.3f, LayerMask.NameToLayer("Player")))
            {
                break;
            }

            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
            controller.player_rigidbody.MovePosition(this.transform.position + this.transform.forward * movePower / 5 * Time.deltaTime);
            yield return null;
        }
        IsForwardMove = false;

    }

    /// <summary>
    /// ���� ���� ù��° �̵� ��� : �ش� �������� �̵�
    /// movement through the hook: movement in that direction.
    /// </summary>
    /// <returns></returns>
    IEnumerator HookPointMoveCoroutine()
    {
        Vector3 startpoint = transform.position;
        while (true)
        {
            Vector3 movePoint;

            if (hooklist[0].IsRope && hooklist[1].IsRope)
            {
                movePoint = Vector3.Lerp(hooklist[0].hitRay.point, hooklist[1].hitRay.point, 0.5f);
            }
            else
            {
                if (hooklist[0].IsRope)
                {
                    movePoint = hooklist[0].hitRay.point;
                }
                else if (hooklist[1].IsRope)
                {
                    movePoint = hooklist[1].hitRay.point;
                }
                else
                {
                    SettingReset();
                    break;
                }
            }

            HookMoveSetting();

            if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("RopeSwing"))
            {
                controller.animator.SetTrigger("GrapplinghookStart");
            }

            controller.isGround = false;


            //���� ����Ʈ (Calibration point)
            Vector3 correctionPoint = movePoint;

            correctionPoint.y = movePoint.y + controller.player_collider.height * 1.25f;

            if (Physics.CheckBox(correctionPoint, new Vector3(1, 1, 1), Quaternion.identity, LayerMask.NameToLayer("Player")))
            {
                correctionPoint.y = movePoint.y - controller.player_collider.height;
            }
            else if (controller.player_collider.height < movePoint.y)
            {
                correctionPoint.y = movePoint.y + controller.player_collider.height * 0.6f;
                correctionPoint += (correctionPoint - startpoint).normalized * 0.9f;
            }
            else
            {
                correctionPoint = movePoint;
            }

            float distance = 0.0f;

            // �ӵ� �����κ� (Speed increase part)
            timer += Time.deltaTime;
            distance = Vector3.Distance(correctionPoint, this.transform.position);

            if (timer >= 0.3f)
            {
                timer = 0;
                if (currentHookPower < maxHookPower)
                {
                    currentHookPower += 10;
                }
            }
            if (distance < 2.0f)
            {
                if (!hooklist[0].IsRope && !hooklist[1].IsRope)
                {
                    if (currentHookPower > 1.0f)
                        currentHookPower = distance * distance * 5;
                }
            }
            // �̵� ���� ���ϱ� (Find direction of movement)
            Vector3 correctionDir = correctionPoint - this.transform.position;
            playerToHitDir = Vector3.Lerp(this.transform.position, correctionPoint, 0.5f);
            Vector3 lerpDir = playerToHitDir - this.transform.position;

            // ȸ���� �̵� (Rotation and movement)
            this.transform.rotation = Quaternion.LookRotation(new Vector3(correctionDir.x, 0, correctionDir.z));
            controller.player_rigidbody.AddForce(lerpDir.normalized * Time.deltaTime * currentHookPower , ForceMode.Impulse);

            //�̵��߿� �÷��̾� �տ� ���� ���� ��� (If there is a wall in front of the player while moving.)
            if (Physics.Raycast(this.transform.position + transform.up * 0.5f, this.transform.forward, out rayhit, 2.5f, LayerMask.NameToLayer("Player")))
            {
                /*
                 
                 ����翬�� ����
                    if (Input.GetKey(KeyCode.Space))
                    {
                    this.transform.rotation = Quaternion.LookRotation(-rayhit.normal);

                    controller.player_rigidbody.MovePosition(this.transform.position  -(rayhit.point - transform.position).normalized * 3.0f);
                    controller.moveState.ChangeState(MoveState.ClimbingMovement);

                        if (controller.isAnimator)
                        {
                            controller.animator.SetTrigger(controller.aniHashClimbingHook);
                            controller.animator.ResetTrigger(controller.aniHashClimbingEndJump);
                        }
                    endFlag = true;
                    break;
                    }

                 */

                if (Vector3.Distance(this.transform.position, rayhit.point) < 0.6f)
                {
                    controller.moveState.ChangeState(MoveState.DefaultMovement);
                    endFlag = true;
                    break;
                }

            }
            // �̵��� ��ġ�� ���� ��ġ�� ���ؼ� Compared to the current location and the location to be moved.
            else if (distance < 1.0f || Vector3.Distance(this.transform.position, movePoint) < 0.75f)
            {
                if (hooklist[0].IsRope && hooklist[1].IsRope)
                {
                    Debug.Log(currentHookPower);
                    StartCoroutine(DashCoroutine(correctionDir, currentHookPower, 0.5f));
                    yield break;
                }
                else
                {
                    StartCoroutine(DashCoroutine(correctionDir, 10f, 0.1f));
                }

                controller.moveState.ChangeState(MoveState.DefaultMovement);
                endFlag = true;
                break;
            }


            if (stopFlag)
            {
                controller.moveState.ChangeState(MoveState.DefaultMovement);
                endFlag = true;
                break;
            }


            yield return null;
        }

        if (endFlag)
        {
            //�̵��� ������ done moving.
            ClearEndHook();
            SettingReset();
            controller.player_rigidbody.velocity = Vector3.zero;
            currentHookPower = 0;
            timer = 0;
        }

    }

    /// <summary>
    /// ���� ���� �ι�° �̵� ��� : �������� �̵�
    /// move through the hook: to swing.
    /// </summary>
    /// <returns></returns>
    IEnumerator SlerpMoveCoroutine()
    {

        if (hooklist[0].IsRope && hooklist[1].IsRope)
        {
            SettingReset();
            yield break;
        }
        else if (hooklist[0].IsRope || hooklist[1].IsRope)
        {
            Hook hook;

            if (hooklist[0].IsRope)
            {
                hook = hooklist[0];
            }
            else if (hooklist[1].IsRope)
            {
                hook = hooklist[1];
            }
            else
            {
                SettingReset();
                yield break;
            }


            Vector3 startPos = this.transform.position + (hook.hitRay.point - transform.position) * 0.2f;
            Vector3 endPos = hook.hitRay.point + (hook.hitRay.point - transform.position) * 0.65f;
            controller.fallTime = 0.0f;

            Vector3 center = (startPos + endPos) * 0.5f;

            startPos = startPos - center;
            endPos = endPos - center;

            float speed = 0.0f;
            int count = 8;

            if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("RopeSwing"))
            {
                controller.animator.SetTrigger("GrapplinghookStart");
            }

            controller.isGround = false;
            HookMoveSetting();

            while (true)
            {
                Vector3 temp;

                temp = Vector3.Slerp(startPos, endPos, count / maxCount);
                temp += center;

                Vector3 dir = temp - transform.position;

                if (count > 20)
                    controller.GroundedCheck();

                if (Physics.Raycast(this.transform.position + transform.up * 0.5f, this.transform.forward, out rayhit, 1.0f, LayerMask.NameToLayer("Player")) || controller.isGround)
                {
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                    endFlag = true;
                    break;
                }

                if (Vector3.Distance(temp, transform.position) < 3.0f)
                {
                    if (count >= maxCount)
                    {
                        StartCoroutine(DashCoroutine(dir, speed, 0.15f));
                        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                        yield break;
                    }
                    else
                    {
                        count+=5;
                    }
                }
                else if (speed < maxSpeed)
                {
                    speed += 0.85f;
                }

                this.transform.rotation = Quaternion.LookRotation(dir);
                controller.player_rigidbody.MovePosition(this.transform.position + dir.normalized * Time.deltaTime * speed);

                if (stopFlag)
                {
                    StartCoroutine(DashCoroutine(dir, speed, 0.15f));
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                    endFlag = true;
                    break;
                }

                yield return null;
            }

            if (endFlag)
            {
                controller.moveState.ChangeState(MoveState.DefaultMovement);
                ClearEndHook();
                SettingReset();
                controller.player_rigidbody.velocity = Vector3.zero;
                currentHookPower = 0;
                timer = 0;
            }
            yield return null;
        }
        else
        {
            startFlag = false;
        }
        yield return null;

    }
}
