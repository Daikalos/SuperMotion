using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController m_CharacterController;
    private Collider m_Collider;
    private Vector3 m_MoveDirection;
    private Vector3 m_HitNormal;

    private bool m_IsGrounded;
    private float m_Velocity;

    [Header("Player Attributes")]
    [Tooltip("Player's movement speed"), Range(1.0f, 80.0f)]
    public float m_Speed = 10.0f;
    [Tooltip("Initial speed when pressing jump"), Range(2.0f, 20.0f)]
    public float m_JumpSpeed = 5.0f;
    [Tooltip("Speed to decrease player velocity when in air"), Range(2.0f, 40.0f)]
    public float m_Gravity = 10.0f;

    [Header("Slope Attributes")]
    [Tooltip("Friction when sliding down slopes"), Range(0.0f, 1.0f)]
    public float m_SlideFriction = 0.3f;
    [Tooltip("How far to look down from bottom of player"), Range(1.0f, 4.0f)]
    public float m_SlopeRayLength = 1.9f;
    [Tooltip("At which force to push down player onto slope"), Range(0.5f, 15.0f)]
    public float m_SlopeForce = 9.0f;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Collider = GetComponent<Collider>();

        m_MoveDirection = Vector3.zero;
        m_HitNormal = Vector3.zero;

        m_IsGrounded = false;
        m_Velocity = 0.0f;
    }

    void Update()
    {
        //Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        m_MoveDirection = transform.right * x + transform.forward * z;

        //If ground is tilted below slopelimit or not
        m_IsGrounded = (Vector3.Angle(Vector3.up, m_HitNormal) <= m_CharacterController.slopeLimit);

        if (m_CharacterController.isGrounded)
        {
            m_Velocity = 0.0f;
            if (m_IsGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    m_Velocity = m_JumpSpeed;
                }
            }
            else
            {
                //Move in direction of the given normal
                m_MoveDirection.x += (1.0f - m_HitNormal.y) * m_HitNormal.x * (1.0f - m_SlideFriction);
                m_MoveDirection.z += (1.0f - m_HitNormal.y) * m_HitNormal.z * (1.0f - m_SlideFriction);
            }
        }

        //Gravity
        m_Velocity -= m_Gravity * Time.deltaTime;
        m_MoveDirection.y = m_Velocity;

        //Finally move character
        m_CharacterController.Move(m_MoveDirection * m_Speed * Time.deltaTime);

        //Move character down, prevent bouncing down slope
        if (m_Velocity <= 0.0f && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, (m_CharacterController.height / 2) * m_SlopeRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                m_CharacterController.Move(Vector3.down * (m_CharacterController.height / 2) * m_SlopeForce * Time.deltaTime);
            }
        }

        //Touching Ceiling
        if ((m_CharacterController.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (m_Velocity > 0.0f)
            {
                m_Velocity = 0.0f;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_HitNormal = hit.normal;
    }
}
