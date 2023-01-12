using Cinemachine.Utility;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using Quaternion = UnityEngine.Quaternion;

public class TPSController : MonoBehaviour
{
    [SerializeField] UnityEvent<TPSController> JumpInput = new UnityEvent<TPSController>();
    [SerializeField] UnityEvent<TPSController> ChargeInput = new UnityEvent<TPSController>();
    [SerializeField] UnityEvent<TPSController> ShootInput = new UnityEvent<TPSController>();
    [SerializeField] UnityEvent<TPSController> SprintInput = new UnityEvent<TPSController>();
    [SerializeField] UnityEvent<TPSController> LocomotionInput = new UnityEvent<TPSController>();

    public float inputJump;
    public float inputCharge;
    public float inputShoot;
    public float inputSprint;
    public float inputHorizontal;
    public float inputVertical;

    [SerializeField] UnityEvent OnSprint = new UnityEvent();
    [SerializeField] UnityEvent OnRaiseToThrow = new UnityEvent();
    [SerializeField] UnityEvent OnShooting = new UnityEvent();
    [SerializeField] UnityEvent OnStandStill = new UnityEvent();
    [SerializeField] UnityEvent OnChargeCharge = new UnityEvent();
    [SerializeField] UnityEvent OnChargeStart = new UnityEvent();
    [SerializeField] UnityEvent OnCharge = new UnityEvent();

    Rigidbody m_rigidbody;
    [SerializeField] public Transform viewPoint;
    AimConstraint viewPointConstraint;
    Transform horizontalCam;
    Vector2 polarViewCoord = new Vector2();
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float turnSpeed;
    float turnTime = 0;
    float dirRotVel = 0;
    [SerializeField] float JumpSpeed = 10;
    bool isJumping = false;
    bool isFalling = true;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform dirIndicator;
    [SerializeField] Transform smoothedDirIndicator;
    [SerializeField] Transform baseSpine;
    [SerializeField] GameObject model;
    [SerializeField] Transform shorOrigin;
    [SerializeField] float onJumpShootLavDisableTime;
    float jumpTime = 0f;
    bool isRaiseToThrow = false;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeChargeTimeLength;
    [SerializeField] float chargeCooldownTimeLength;
    float chargeChargeTime = 0f;
    float chargeTime = 0f;
    bool isCharging = false;
    bool isChargeChargeing = false;
    [SerializeField] GameObject bulletPrefab;
    Animator animator;
    int shotIndex;
    int shotAirIndex;
    //Vector3 dirRotVel = new Vector3();
    Vector3 velocity;
    Vector3 preTargetVelocity;
    Vector3 targetVelocity;
    //[SerializeField] Transform targetDirectionTransform;
    Vector3 velocitySmoothCurrentVel;
    Vector3 direction;
    Collider m_collider;
    float distToGround;
    [SerializeField] bool showCursor = true;
    bool preShowCursor = true;
    [SerializeField] GameObject Ball;
    public bool hasBall { get; private set; }
    [SerializeField] bool hasBallDisplay;

    [SerializeField] bool targeting;
    public GameObject target;

    [SerializeField] Collider ballStealTrigger;
    [SerializeField] Collider ballStolenTrigger;
    [SerializeField] float stealCooltime = 1;
    float stealTime = -1;
    [SerializeField] public Transform ballHoldSocket;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        horizontalCam = new GameObject("CamHorizontal").transform;
        animator = model.GetComponent<Animator>();
        shotIndex = animator.GetLayerIndex("Shot");
        shotAirIndex = animator.GetLayerIndex("ShotAir");
        distToGround = m_collider.bounds.extents.y;
        //Debug.Log(m_collider.bounds.center);

        viewPointConstraint = viewPoint.GetComponent<AimConstraint>();
        ConstraintSource source = new ConstraintSource();
        source.weight = 1;
        source.sourceTransform = target.transform;
        viewPointConstraint.SetSource(0, source);

        hasBall = hasBallDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        hasBallDisplay = hasBall;
        if (hasBall)
        {
            ballStolenTrigger.enabled = true;
            ballStealTrigger.enabled = false;
        }
        else
        {
            ballStolenTrigger.enabled = false;
            ballStealTrigger.enabled = true;
        }
        if (stealTime != -1)
        {
            stealTime += Time.deltaTime;
        }
        if (stealTime > stealCooltime)
        {
            ballStolenTrigger.enabled = false;
            ballStealTrigger.enabled = false;
            stealTime = -1;
        }
        if (targeting)
        {
            TargetLook();
        }
        else
        {
            MouseLook();
        }
        inputHorizontal = 0f;
        inputVertical = 0f; 
        if (IsGrounded())
        {

            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Dance"))
            {
                LocomotionInput.Invoke(this);
                SprintInput.Invoke(this);
                if (inputSprint > 0.1)
                {
                    inputVertical = 1;
                    OnSprint.Invoke();
                }
                if (Mathf.Abs(inputHorizontal) <= 0.05f && Mathf.Abs(inputVertical) <= 0.05f)
                {
                    OnStandStill.Invoke();
                }
                JumpInput.Invoke(this);
                //if (Input.GetAxis("LeftTrigger") > 0.1)
                //{
                //    JumpStart();
                //}
                if (inputJump > 0.1)
                {
                    JumpStart();
                }
            }
            if (!(isCharging || isChargeChargeing))
            {
                Locomotion();
            }

        }

        ChargeInput.Invoke(this);
        if (inputCharge >= 0.1)
        {
            if (!isChargeChargeing && !isCharging)
            {
                isChargeChargeing = true;
            }
        }
        if (isChargeChargeing)
        {
            chargeChargeTime += Time.deltaTime;
            OnChargeCharge.Invoke();
            animator.SetBool("ChargeChargeing", true);
        }
        else
        {
            chargeChargeTime = 0f;
            animator.SetBool("ChargeChargeing", false);
        }
        if (chargeChargeTime >= chargeChargeTimeLength)
        {
            if (!isCharging)
            {
                Charge();
                isChargeChargeing = false;
            }
            isCharging = true;
        }
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            OnCharge.Invoke();
        }
        else
        {
            if (chargeTime != 0)
            {
                ChargeEnd();
            }
            chargeTime = 0;
        }
        if (chargeTime >= chargeCooldownTimeLength)
        {
            isCharging = false;
        }

        //AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(1);
        //if (stateinfo.IsName("JumpUp")) animator.SetBool("Jump", false);
        if (m_rigidbody.velocity.y < -0.05f)
        {
            isFalling = true;
        }
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            if(isFalling)
            {
                animator.SetTrigger("JumpDown");
            }
            animator.SetLayerWeight(shotAirIndex, 1);
            animator.SetLayerWeight(shotIndex, 0);
            //RaycastHit hit;
            //if (Physics.Raycast(m_rigidbody.position,new Vector3(0,m_rigidbody.velocity.y,0),out hit, m_rigidbody.velocity.y,groundLayer))
            //{

            //}

        }
        else
        {
            jumpTime = 0;

            animator.SetLayerWeight(shotAirIndex, 0);
            animator.SetLayerWeight(shotIndex, 1);
        }

        Vector3 noVerticalVelocity = m_rigidbody.velocity;
        noVerticalVelocity.y = 0;
        if (noVerticalVelocity.magnitude > 0)
        {
            direction = noVerticalVelocity;
        }
        //dirの角度を求める
        //float dir_angle = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z);
        //modelのオイラー角を設定(yのみ、スムース)
        //model.transform.eulerAngles = new Vector3(0,Mathf.SmoothDampAngle(model.transform.rotation.eulerAngles.y, dir_angle,ref dirRotVel, 0.5f) , 0);
        Vector3 modeldir = smoothedDirIndicator.localPosition;
        //animator2次元空間ブレンド向けの処理
        //モデルから見たdirの座標を取得
        Vector3 relate = model.transform.InverseTransformPoint(transform.position + direction * (noVerticalVelocity.magnitude / baseSpeed))/4;
        //animatorに適用
        animator.SetFloat("Horizontal", relate.x * (1 + inputSprint));
        animator.SetFloat("Vertical", relate.z * (1 + inputSprint));

        animator.SetBool("RaiseToThrow", false);
        //animator.SetBool("Shot", false);

        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Shoot")) OnShooting.Invoke();
        ShootInput.Invoke(this);
        if (inputShoot > 0.1f)
        {
            if (jumpTime >= onJumpShootLavDisableTime)
            {
                if (!isRaiseToThrow)
                {
                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 1f, m_rigidbody.velocity.z);
                }
                m_rigidbody.drag = 3;
            }
            //m_rigidbody.AddForce(Physics.gravity / 2 * Time.deltaTime, ForceMode.Force);
            isRaiseToThrow = true;
            animator.SetBool("RaiseToThrow", true);
            OnRaiseToThrow.Invoke();
        }
        else
        {
            if (isRaiseToThrow)
            {
                //Debug.Log("Throw");
                ShotStart();
                m_rigidbody.drag = 0;
            }
            isRaiseToThrow = false;
        }

        if (preShowCursor != showCursor)
        {
            Cursor.visible = showCursor;
        }
        Cursor.lockState = CursorLockMode.Locked;
        preShowCursor = showCursor;
    }

    private void LateUpdate()
    {

        
    }

    bool IsGrounded()
    {
        return true;
        //return Physics.Raycast(transform.position + m_collider.bounds.center, -Vector3.up, distToGround + 0.1f);
    }

    void TargetLook()
    {
        viewPointConstraint.weight = 1;
        viewPointConstraint.constraintActive = true;
    }
    void MouseLook()
    {
        viewPointConstraint.weight = 0;
        viewPointConstraint.constraintActive = false;

        showCursor = false;
        float mouseY = Input.GetAxis("LeftHoirizontal") * Time.deltaTime * 200 * Settings.MouseSensitivity;
        float mouseX = Input.GetAxis("LeftVertical") * Time.deltaTime * 200 * Settings.MouseSensitivity;
        if (mouseX >= 0.001)
        {
            Debug.Log(mouseX);
        }

        polarViewCoord.x += mouseX;
        polarViewCoord.y += mouseY;
        polarViewCoord.y = Mathf.Min(90.0f,polarViewCoord.y);
        polarViewCoord.y = Mathf.Max(-80.0f,polarViewCoord.y);

        //horizontalCam.rotation = Quaternion.Euler(0, polarViewCoord.x, 0);
        horizontalCam.position = Camera.main.transform.position;

        //viewPoint.rotation = Quaternion.identity;
        viewPoint.rotation = Quaternion.Euler(0, polarViewCoord.x, 0);
        viewPoint.Rotate(new Vector3(-polarViewCoord.y,0,0 ));

    }

    

    void Locomotion()
    {

        //if (inputSprint > 0.1)
        //{
        //    OnSprint.Invoke();
        //}

        horizontalCam.position = Camera.main.transform.position;

        Vector3 forward = Camera.main.transform.rotation * Vector3.forward;
        Vector3 right = Camera.main.transform.rotation * Vector3.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        targetVelocity = (right * inputHorizontal + forward * inputVertical).normalized * baseSpeed * (1 + Input.GetAxis("RightTrigger"));
        targetVelocity.y = m_rigidbody.velocity.y;

        if (targetVelocity.magnitude <= 0.001)
        {
            //targetVelocity = preTargetVelocity;
        }
        Quaternion dirrot = Quaternion.LookRotation(targetVelocity);
        baseSpine.rotation = Quaternion.Euler(baseSpine.eulerAngles.x,dirrot.eulerAngles.y, baseSpine.eulerAngles.z);
        if (m_rigidbody.velocity.magnitude <= targetVelocity.magnitude)
        {
            m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, targetVelocity, ref velocitySmoothCurrentVel, 0.2f);
                
        }
        else
        {
            m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, targetVelocity, ref velocitySmoothCurrentVel, 0.1f);
        }

        preTargetVelocity = targetVelocity;

        //Debug.Log(Input.GetAxis("RightTrigger"));

        
        dirIndicator.position = transform.position;
        dirIndicator.position += direction;

        //transform.Translate(velocity * Time.deltaTime);

    }
    public void JumpStart()
    {
        if (!isJumping) animator.SetTrigger("Jump");
        isJumping = true;

    }
    void Jump()
    {
        animator.ResetTrigger("Jump");
        m_rigidbody.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }

    public void ShotStart()
    {
        animator.SetBool("Shoot", true);
        //animator.SetTrigger("Throw");
    }

    void Shot()
    {
        //Transform bullet = Instantiate(bulletPrefab).transform;
        //bullet.position = shorOrigin.position;
        //bullet.rotation = shorOrigin.rotation;
        //bullet.GetComponent<projectile>().Throw();
        //m_rigidbody.useGravity = true;
        animator.SetBool("Shoot", false);
        Ball.GetComponent<BallController>().Throw(shorOrigin.rotation);
        hasBall = false;
    }

    public void Dance()
    {
        animator.SetTrigger("Dance");
    }
    public void Charge()
    {
        Debug.Log("Charge");
        OnChargeStart.Invoke();
        animator.SetTrigger("Charge");
        Vector3 charge_dir = transform.rotation * transform.InverseTransformPoint(target.transform.position) - Vector3.up * 2;
        Debug.Log(charge_dir);
        m_rigidbody.velocity = charge_dir.normalized * chargeSpeed + Vector3.up * 2;
        m_rigidbody.drag = 3;
        //m_rigidbody.AddForce(, ForceMode.VelocityChange);
        //transform.Translate(charge_dir * chargeSpeed);
    }

    void ChargeEnd()
    {
        m_rigidbody.drag = 0;
    }
    public void OnGround()
    {
        if (isFalling)
        {
            animator.ResetTrigger("JumpDown");
            animator.ResetTrigger("Jump");
            Debug.Log("Ground!");
            isJumping = false;
            isFalling = false;
        }
    }

    public void StealBall()
    {
        if (stealTime == -1)
        {
            hasBall = true;
            stealTime = 0;
        }
    }

    public void StolenBall()
    {
        if (stealTime == -1)
        {
            hasBall = false;
            stealTime = 0;
        }
    }
}