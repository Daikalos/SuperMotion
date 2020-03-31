using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Collider))]
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
    [SerializeField, Tooltip("Friction when sliding down slopes"), Range(0.0f, 1.0f)]
    private float m_SlideFriction = 0.3f;
    [SerializeField, Tooltip("Speed when sliding down slopes"), Range(0.0f, 40.0f)]
    private float m_SlideSpeed = 35.0f;
    [SerializeField, Tooltip("How far to look down from bottom of player"), Range(0.0f, 5.0f)]
    private float m_SlopeRayLength = 1.5f;
    [SerializeField, Tooltip("At which force to push down player onto slope"), Range(0.0f, 20.0f)]
    public float m_SlopeForce = 15.0f;

    private CharacterController m_CharacterController;
    private Collider m_Collider;
    private Vector3
        m_MoveDirection,
        m_Velocity,
        m_HitNormal;
    private bool 
        m_IsGrounded,
        m_IsJumping;
    private float m_SlopeLimit;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Collider = GetComponent<Collider>();

        m_MoveDirection = Vector3.zero;
        m_HitNormal = Vector3.zero;
        m_Velocity = Vector3.zero;

        m_IsGrounded = false;
        m_IsJumping = false;

        m_SlopeLimit = m_CharacterController.slopeLimit;
    }

    void Update()
    {
        //Movement
        float x = Input.GetAxis("Horizontal") * m_Speed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * m_Speed * Time.deltaTime;

        m_MoveDirection = transform.right * x + transform.forward * z;

        //If ground is tilted below slopelimit or not
        m_IsGrounded = (Vector3.Angle(Vector3.up, m_HitNormal) <= m_CharacterController.slopeLimit) ? m_CharacterController.isGrounded : false;

        if (m_CharacterController.isGrounded)
        {
            m_CharacterController.slopeLimit = m_SlopeLimit;
            m_IsJumping = false;
            m_Velocity.y = 0.0f;

            if (m_IsGrounded)
            {
                if (Input.GetButtonDown("Jump") && !m_IsJumping)
                {
                    m_CharacterController.slopeLimit = 90.0f;
                    m_IsJumping = true;
                    m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
                }
            }
            else
            {
                //Move in direction of the normal which the player has collided with
                m_MoveDirection.x += ((1.0f - m_HitNormal.y) * m_HitNormal.x) * (1.0f - m_SlideFriction) * AngleToValue(m_HitNormal, m_SlideSpeed) * Time.deltaTime;
                m_MoveDirection.z += ((1.0f - m_HitNormal.y) * m_HitNormal.z) * (1.0f - m_SlideFriction) * AngleToValue(m_HitNormal, m_SlideSpeed) * Time.deltaTime;
            }
        }

        m_CharacterController.Move(m_MoveDirection);

        //Gravity
        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_CharacterController.Move(m_Velocity * Time.deltaTime);

        //Move character down, prevent bouncing down slope
        if (!m_IsJumping && Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out RaycastHit hit, (m_CharacterController.height / 2) * m_SlopeRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                m_CharacterController.Move(Vector3.down * (m_CharacterController.height / 2) * m_SlopeForce * Time.deltaTime);
            }
        }

        //Touching Ceiling
        if ((m_CharacterController.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (m_Velocity.y > 0.0f)
            {
                m_Velocity.y = 0.0f;
            }
        }
    }
    
    /// <summary>
    /// Returns a interval representation (0 to max) of the angle given in the assigned limit
    /// </summary>
    private float AngleToValue(Vector3 anAngle, float aMax)
    {
        //The more tilted the angle, the higher the return value
        return ((Vector3.Angle(Vector3.up, anAngle) / 90.0f) * aMax);
    }

    private void OnControllerColliderHit(ControllerColliderHit aHit)
    {
        m_HitNormal = aHit.normal;
    }
}
