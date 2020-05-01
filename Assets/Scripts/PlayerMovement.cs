using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField, Tooltip("Player's movement speed"), Range(1.0f, 80.0f)]
    private float m_Speed = 10.0f;
    [SerializeField, Tooltip("Jump height when pressing jump"), Range(0.0f, 30.0f)]
    private float m_JumpHeight = 4.3f;
    [SerializeField, Tooltip("Speed to decrease player velocity when in air"), Range(-60.0f, 0.0f)]
    private float m_Gravity = -40.0f;

    [Header("Slope Attributes")]
    [SerializeField, Tooltip("Speed when sliding down slopes"), Range(0.0f, 40.0f)]
    private float m_SlideSpeed = 15.0f;
    [SerializeField, Tooltip("How far to look down from bottom of player"), Range(0.0f, 5.0f)]
    private float m_SlopeRayLength = 1.5f;
    [SerializeField, Tooltip("At which force to push down player onto slope"), Range(0.0f, 20.0f)]
    private float m_SlopeForce = 15.0f;
    [SerializeField, Tooltip("Jump force when pressing jump on a slope higher than slopelimit"), Range(0.0f, 80.0f)]
    private float m_SlopeJump = 40.0f;
    [SerializeField, Tooltip("Speed of which the player regains control of character after walljump/slopejump"), Range(0.0f, 4.0f)]
    private float m_RegainControl = 1.1f;

    [Header("Abilities Attributes")]
    [SerializeField, Tooltip("Speed when using speed ability"), Range(0.0f, 5.0f)]
    private float m_BoostSpeedFactor = 2.0f;
    [SerializeField, Tooltip("Speed when using dash ability"), Range(0.0f, 100.0f)]
    private float m_DashSpeed = 50.0f;
    [SerializeField, Tooltip("For how long the dash is active"), Range(0.0f, 10.0f)]
    private float m_DashTime = 0.20f;
    [SerializeField, Tooltip("Height when using jump ability"), Range(0.0f, 5.0f)]
    private float m_HighJumpFactor = 2.0f;
    [SerializeField, Tooltip("Distance the player can hit objects when using strength ability"), Range(0.0f, 8.0f)]
    private float m_PunchDistance = 5.0f;
    [SerializeField, Tooltip("How hard the player punches a moveable object"), Range(0.0f, 1500.0f)]
    private float m_PunchStrength = 500.0f;

    private CharacterController m_CharacterController;
    private PlayerCameraEffects m_PlayerCameraEffects;
    private GameObject m_PreviousSlope;
    private GameObject m_CurrentSlope;

    private Vector3 m_Velocity;
    private Vector3 m_SlopeNormal;
    private RaycastHit m_SlideOffEdge;

    private bool m_IsGrounded;
    private bool m_CanJump;
    private bool m_CanSlopeJump;
    private float m_SlopeLimit;

    public Vector3 Velocity { get => m_Velocity; set => m_Velocity = value; }

    public bool CanSlopeJump { get => m_CanSlopeJump; set => m_CanSlopeJump = value; }

    public float Speed { get => m_Speed; set => m_Speed = value; }
    public float JumpHeight { get => m_JumpHeight; set => m_JumpHeight = value; }
    public float Gravity { get => m_Gravity; set => m_Gravity = value; }
    public float SlopeJump { get => m_SlopeJump; set => m_SlopeJump = value; }

    public float BoostSpeedFactor { get => m_BoostSpeedFactor; }
    public float DashSpeed { get => m_DashSpeed; }
    public float DashTime { get => m_DashTime; }
    public float HighJumpFactor { get => m_HighJumpFactor; }
    public float PunchDistance { get => m_PunchDistance; }
    public float PunchStrength { get => m_PunchStrength; }

    public float NormalSpeed { get; private set; }
    public float NormalJumpHeight { get; private set; }
    public float NormalSlopeJump { get; private set; }

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerCameraEffects = GetComponent<PlayerCameraEffects>();

        m_Velocity = Vector3.zero;
        m_SlopeNormal = Vector3.zero;
        m_SlideOffEdge = new RaycastHit();

        m_IsGrounded = false;
        m_CanJump = true;
        m_CanSlopeJump = true; 

        m_SlopeLimit = m_CharacterController.slopeLimit;

        NormalSpeed = m_Speed;
        NormalJumpHeight = m_JumpHeight;
        NormalSlopeJump = m_SlopeJump;
    }

    void Update()
    {
        Movement();
        CollisionGround();
        CollisionEvents();
    }

    private void Movement()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        JumpInput();

        Vector3 moveDirection = Vector3.ClampMagnitude(transform.right * horizInput + transform.forward * vertInput, 1.0f) * m_Speed;

        //Air Movement after walljump/slopejump, allow player to regain control
        m_Velocity.x = ((m_Velocity.x != 0 || m_Velocity.z != 0) && (OppositeSigns(m_Velocity.x, moveDirection.x) || Mathf.Abs(m_Velocity.x) < Mathf.Abs(moveDirection.x)) && moveDirection.x != 0) ? 
            Mathf.Lerp(m_Velocity.x, moveDirection.x, m_RegainControl * Time.deltaTime) : m_Velocity.x;
        m_Velocity.z = ((m_Velocity.x != 0 || m_Velocity.z != 0) && (OppositeSigns(m_Velocity.z, moveDirection.z) || Mathf.Abs(m_Velocity.z) < Mathf.Abs(moveDirection.z)) && moveDirection.z != 0) ? 
            Mathf.Lerp(m_Velocity.z, moveDirection.z, m_RegainControl * Time.deltaTime) : m_Velocity.z;
        
        //Turn off movement after walljump/slopejump
        moveDirection *= ((m_Velocity.x != 0 || m_Velocity.z != 0) ? 0.0f : 1.0f);

        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_CharacterController.Move((moveDirection + m_Velocity) * Time.deltaTime);

        if ((horizInput != 0 || vertInput != 0) && OnSlope())
        {
            //Push character down to create smooth movement when walking down slopes
            m_CharacterController.Move(Vector3.down * (m_CharacterController.height / 2) * m_SlopeForce * Time.deltaTime);
        }

        if (m_SlideOffEdge.collider != null)
        {
            //Player slides off object when standing on edge
            m_CharacterController.Move(m_SlideOffEdge.normal * Time.deltaTime);
        }
    }

    private void JumpInput()
    {
        if ((m_CharacterController.isGrounded && m_CanJump) || m_CanSlopeJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                m_CharacterController.slopeLimit = 90.0f;
                m_CanSlopeJump = false;
                m_CanJump = false;

                if (m_IsGrounded)
                {
                    m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
                }
                else
                {
                    //Jump in the direction of the slope normal
                    m_Velocity = m_SlopeNormal * AngleToValue(m_SlopeNormal, m_SlopeJump);
                    m_PreviousSlope = m_CurrentSlope;
                }
                AudioManager.instance.Play("Step");
            }
        }
    }

    private void CollisionGround()
    {
        //If ground is tilted below slopelimit or not
        m_IsGrounded = (Vector3.Angle(Vector3.up, m_SlopeNormal) <= m_SlopeLimit) ? m_CharacterController.isGrounded : false;

        if (m_CharacterController.isGrounded)
        {
            m_CharacterController.slopeLimit = m_SlopeLimit;
            m_CanJump = m_IsGrounded;

            m_PlayerCameraEffects.Velocity = m_Velocity;
            m_PlayerCameraEffects.CheckCameraShake = true;

            m_Velocity = Vector3.zero;
            m_Velocity.y = m_Gravity * Time.deltaTime;

            if (!m_IsGrounded)
            {
                //The downward direction of the slope
                Vector3 slideDirection = -Vector3.Cross(Vector3.Cross(m_SlopeNormal, Vector3.up), m_SlopeNormal);
                m_CharacterController.Move(slideDirection * AngleToValue(m_SlopeNormal, m_SlideSpeed) * Time.deltaTime);
            }
        }
    }

    private void CollisionEvents()
    {
        //Touching Ceiling
        if ((m_CharacterController.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (m_Velocity.y > 0.0f)
            {
                m_Velocity.y = 0.0f;
            }
        }
    }

    private bool OnSlope()
    {
        return
            m_CanJump &&
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, (m_CharacterController.height / 2) * m_SlopeRayLength) &&
            hit.normal != Vector3.up;
    }

    /// <summary>
    /// Returns a interval representation (0 to max) of the angle given in the assigned limit
    /// </summary>
    private float AngleToValue(Vector3 angle, float max)
    {
        //The more tilted the angle, the higher the return value
        return ((Vector3.Angle(Vector3.up, angle) / 90.0f) * max);
    }

    private bool OppositeSigns(float x, float y)
    {
        return Mathf.Sign(x) != Mathf.Sign(y);
    }

    private void OnControllerColliderHit(ControllerColliderHit objectHit)
    {
        if (m_PreviousSlope != objectHit.gameObject && m_IsGrounded)
        {
            m_PreviousSlope = null;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, (m_CharacterController.height / 2) * m_SlopeRayLength))
        {
            if (rayHit.collider == objectHit.collider && rayHit.normal == objectHit.normal)
            {
                if (Vector3.Angle(Vector3.up, rayHit.normal) > m_SlopeLimit)
                {
                    //Allow player to jump once on a slope higher than slopelimit
                    if (m_PreviousSlope != m_CurrentSlope)
                    {
                        m_CanSlopeJump = true;
                    }
                }
                else
                {
                    m_CanSlopeJump = false;
                }
            }
        }

        if (Physics.SphereCast(transform.position, m_CharacterController.radius - m_CharacterController.skinWidth, Vector3.down, out RaycastHit slopeHit, (m_CharacterController.height / 2) * m_SlopeRayLength))
        {
            if (slopeHit.normal == objectHit.normal)
            {
                if (slopeHit.collider == objectHit.collider)
                {
                    m_CurrentSlope = objectHit.gameObject;
                    m_SlopeNormal = objectHit.normal;
                }

                if (slopeHit.collider == objectHit.collider && m_Velocity.y > 0.0f)
                {
                    //Fixes when player attempts to jump up a steep slope when slopelimit is set to 90 degrees when jumping, need further testing
                    m_CharacterController.slopeLimit = m_SlopeLimit;

                    m_Velocity = Vector3.zero;
                    m_Velocity.y = m_Gravity * Time.deltaTime;
                }
            }
        }

        m_SlideOffEdge = new RaycastHit();
        if (Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out RaycastHit sphereHit, (m_CharacterController.height / 2) * m_SlopeRayLength))
        {
            if (rayHit.collider == null && m_IsGrounded)
            {
                m_SlideOffEdge = sphereHit;
            }
        }
    }
}
