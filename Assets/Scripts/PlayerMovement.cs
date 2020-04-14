using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField, Tooltip("Speed of which the player gets back control of character after jumping off a slope"), Range(0.0f, 10.0f)]
    private float m_AirResistance = 1.2f;

    private CharacterController 
        m_CharacterController;
    private GameObject 
        m_PreviousSlope,
        m_CurrentSlope;
    private Vector3
        m_Velocity,
        m_HitNormal;
    private bool
        m_IsGrounded,
        m_CanJump;
    private float
        m_SlopeLimit;

    public Vector3 Velocity { get => m_Velocity; set => m_Velocity = value; }

    public float Speed { get => m_Speed; set => m_Speed = value; }
    public float JumpHeight {  get => m_JumpHeight; set => m_JumpHeight = value; }
    public float Gravity { get => m_Gravity; set => m_Gravity = value; }

    public float NormalSpeed { get; set; }
    public float NormalJumpHeight { get; set; }

    public float SlopeJump { get => m_SlopeJump; set => m_SlopeJump = value; }

    public bool CanJump { get => m_CanJump; set => m_CanJump = value; }

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();

        m_HitNormal = Vector3.zero;
        m_Velocity = Vector3.zero;

        m_IsGrounded = false;
        m_CanJump = true;

        m_SlopeLimit = m_CharacterController.slopeLimit;

        NormalSpeed = Speed;
        NormalJumpHeight = JumpHeight;
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

        Vector3 moveDirection = transform.right * horizInput + transform.forward * vertInput;
        Vector3 moveCharacter = Vector3.ClampMagnitude(moveDirection, 1.0f) * m_Speed * Time.deltaTime;

        if (m_Velocity.x != 0 || m_Velocity.z != 0)
        {
            //Allow player to regain control of character when jumping off slope
            m_Velocity.x = Mathf.Lerp(m_Velocity.x, moveCharacter.x, m_AirResistance * Time.deltaTime);
            m_Velocity.z = Mathf.Lerp(m_Velocity.z, moveCharacter.z, m_AirResistance * Time.deltaTime);
        }
        
        m_CharacterController.Move(moveCharacter);

        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_CharacterController.Move(m_Velocity * Time.deltaTime);

        if ((horizInput != 0 || vertInput != 0) && OnSlope())
        {
            //Push character down to create smooth movement when walking down slopes
            m_CharacterController.Move(Vector3.down * (m_CharacterController.height / 2) * m_SlopeForce * Time.deltaTime);
        }
    }

    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump") && m_CanJump)
        {
            m_CharacterController.slopeLimit = 90.0f;
            m_CanJump = false;

            if (m_IsGrounded)
            {
                m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
                Debug.Log("jumpheight: " + m_JumpHeight);
            }
            else
            {
                //Jump in the direction of the slope normal
                m_Velocity = m_HitNormal * AngleToValue(m_HitNormal, m_SlopeJump);
                m_PreviousSlope = m_CurrentSlope;
            }
            AudioManager.instance.Play("Step");
        }
    }

    private void CollisionGround()
    {
        //If ground is tilted below slopelimit or not
        m_IsGrounded = (Vector3.Angle(Vector3.up, m_HitNormal) <= m_SlopeLimit) ? m_CharacterController.isGrounded : false;

        if (m_CharacterController.isGrounded)
        {
            m_CharacterController.slopeLimit = m_SlopeLimit;
            m_Velocity = Vector3.zero;

            if (m_IsGrounded)
            {
                m_CanJump = true;
            }
            else
            {
                //Allow player to jump once on a slope higher than slopelimit
                if (m_PreviousSlope != m_CurrentSlope) { m_CanJump = true;  }

                //The downward direction of the slope
                Vector3 slideDirection = -Vector3.Cross(Vector3.Cross(m_HitNormal, Vector3.up), m_HitNormal);
                m_CharacterController.Move(slideDirection * AngleToValue(m_HitNormal, m_SlideSpeed) * Time.deltaTime);
            }
        }
        else
        {
            m_CanJump = false;
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
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out RaycastHit hit, (m_CharacterController.height / 2) * m_SlopeRayLength) &&
            hit.normal != Vector3.up;
    }

    /// <summary>
    /// Returns a interval representation (0 to max) of the angle given in the assigned limit
    /// </summary>
    private float AngleToValue(Vector3 anAngle, float aMax)
    {
        //The more tilted the angle, the higher the return value
        return ((Vector3.Angle(Vector3.up, anAngle) / 90.0f) * aMax);
    }

    private void OnControllerColliderHit(ControllerColliderHit objectHit)
    {
        m_CurrentSlope = objectHit.gameObject;
        m_HitNormal = objectHit.normal;

        //Reset previous each time a new object is collided with
        if (m_PreviousSlope != objectHit.gameObject)
        {
            m_PreviousSlope = null;
        }

        //Bug fix, fixes when player attempts to jump up a steep slope when slopelimit is set to 90 degrees when jumping, need further testing
        if (!m_CanJump && Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out RaycastHit hit, (m_CharacterController.height / 2) * m_SlopeRayLength))
        {
            if (Vector3.Angle(Vector3.up, hit.normal) > m_SlopeLimit)
            {
                m_CharacterController.slopeLimit = m_SlopeLimit;
            }
        }
    }
}
