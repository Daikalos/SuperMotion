using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController m_CharacterController;
    private Collider m_Collider;
    private Vector3 m_MoveDirection;
    private Vector3 m_HitNormal;

    private bool m_IsGrounded = false;
    private float m_Velocity = 0.0f;

    [Range(1.0f, 80.0f)]
    public float m_Speed = 10.0f;
    [Range(2.0f, 20.0f)]
    public float m_JumpSpeed = 5.0f;
    [Range(2.0f, 40.0f)]
    public float m_Gravity = 10.0f;
    [Range(0.0f, 1.0f)]
    public float m_SlideFriction = 0.3f;
    [Range(1.0f, 2.5f)]
    public float m_SlopeRayLength = 1.5f;
    [Range(0.5f, 10.0f)]
    public float m_SlopeForce = 5.0f;


    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Collider = GetComponent<Collider>();

        m_MoveDirection = Vector3.zero;
        m_HitNormal = Vector3.zero;
    }

    void Update()
    {
        //Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        m_MoveDirection = transform.right * x + transform.forward * z;

        //Slide
        m_IsGrounded = (Vector3.Angle(Vector3.up, m_HitNormal) <= m_CharacterController.slopeLimit);

        //Jump
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
