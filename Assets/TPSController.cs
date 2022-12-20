using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class TPSController : MonoBehaviour
{
    Rigidbody m_rigidbody;
    [SerializeField] Transform viewPoint;
    Transform horizontalCam;
    Vector2 polarViewCoord = new Vector2();
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float turnSpeed;
    float turnTime = 0;
    float dirRotVel = 0;
    [SerializeField] float JumpSpeed = 10;
    [SerializeField] Transform dirIndicator;
    [SerializeField] Transform baseSpine;
    [SerializeField] GameObject model;
    [SerializeField] Transform shorOrigin;
    [SerializeField] GameObject bulletPrefab;
    Animator animator;
    //Vector3 dirRotVel = new Vector3();
    Vector3 velocity;
    Vector3 preTargetVelocity;
    Vector3 targetVelocity;
    Vector3 velocitySmoothCurrentVel;
    Vector3 direction;
    Collider m_collider;
    float distToGround;
    [SerializeField] bool showCursor = true;
    bool preShowCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        horizontalCam = new GameObject("CamHorizontal").transform;
        animator = model.GetComponent<Animator>();
        distToGround = m_collider.bounds.extents.y;
        //Debug.Log(m_collider.bounds.center);
    }

    // Update is called once per frame
    void Update()
    {
        MouseLook();
        if (IsGrounded())
        {
            Locomotion();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpInput();
            }
        }
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(1);
        if (stateinfo.IsName("Jump")) animator.SetBool("Jump", false);


        Vector3 noVerticalVelocity = m_rigidbody.velocity;
        noVerticalVelocity.y = 0;
        if (noVerticalVelocity.magnitude > 0)
        {
            direction = noVerticalVelocity;
        }
        //dirの角度を求める
        float dir_angle = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z);
        //modelのオイラー角を設定(yのみ、スムース)
        model.transform.eulerAngles = new Vector3(0,Mathf.SmoothDampAngle(model.transform.rotation.eulerAngles.y, dir_angle,ref dirRotVel, 0.5f) , 0);
        
        //animator2次元空間ブレンド向けの処理
        //モデルから見たdirの座標を取得
        Vector3 relate = model.transform.InverseTransformPoint(transform.position + direction * (noVerticalVelocity.magnitude / baseSpeed));
        //animatorに適用
        animator.SetFloat("Horizontal", relate.x);
        animator.SetFloat("Vertical", relate.z);

        if (Input.GetMouseButtonDown(0))
        {
            ShotInput();
        }
        stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Shot")) animator.SetBool("Shot", false);

        if (preShowCursor != showCursor)
        {
            Cursor.visible = showCursor;
        }
        Cursor.lockState = CursorLockMode.Locked;
        preShowCursor = showCursor;
    }

    bool IsGrounded()
    {
        return true;
        //return Physics.Raycast(transform.position + m_collider.bounds.center, -Vector3.up, distToGround + 0.1f);
    }
    void MouseLook()
    {
        showCursor = false;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 200 * Settings.MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 200 * Settings.MouseSensitivity;
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        horizontalCam.position = Camera.main.transform.position;

        Vector3 forward = Camera.main.transform.rotation * Vector3.forward;
        Vector3 right = Camera.main.transform.rotation * Vector3.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        targetVelocity = (right * horizontal + forward * vertical).normalized * baseSpeed;
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

        //dirIndicator.position = transform.position;
        //dirIndicator.position += velocity;
        //transform.Translate(velocity * Time.deltaTime);

    }
    void JumpInput()
    {
        animator.SetBool("Jump",true);
    }
    public void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }

    void ShotInput()
    {
        animator.SetBool("Shot", true);
    }

    public void Shot()
    {
        Transform bullet = Instantiate(bulletPrefab).transform;
        bullet.position = shorOrigin.position;
        bullet.rotation = shorOrigin.rotation;
        bullet.GetComponent<projectile>().Throw();
    }
}