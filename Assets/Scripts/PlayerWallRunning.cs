using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerWallRunning : MonoBehaviour
{
    [Header("Wall Running Attributes")]
    [SerializeField, Tooltip("How far to look for a wall"), Range(0.0f, 2.0f)]
    private float m_RayLength = 0.05f;
    [SerializeField, Tooltip("Running max point"), Range(0.0f, 20.0f)]
    private float m_RunningHeight = 2.5f;
    [SerializeField, Tooltip("Speed to decrease player fall velocity when wall running"), Range(-80.0f, 0.0f)]
    private float m_Gravity = -15.0f;
    [SerializeField, Tooltip("Jump force when pressing jump while wall running"), Range(0.0f, 30.0f)]
    private float m_WallJump = 10.0f;

    private CharacterController m_CharacterController;
    private PlayerMovement m_PlayerMovement;
    private PlayerLook m_PlayerLook;

    private RaycastHit m_WallHit;
    private Vector3 m_Velocity;
    private Vector3 m_MoveSpeed;
    private Vector3 m_LastPos;

    private bool m_WallFound;
    private bool m_CanWallJump;
    private float m_WallCheckDistance;

    public int MoveDirection { get; private set; }

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerLook = GetComponentInChildren<PlayerLook>();

        m_Velocity = Vector3.zero;
        m_MoveSpeed = Vector3.zero;
        m_LastPos = transform.position;

        m_WallFound = true;
        m_CanWallJump = true;

        m_WallCheckDistance = 15.0f;
    }

    void Update()
    {
        m_MoveSpeed = transform.position - m_LastPos;
        m_LastPos = transform.position;

        WallCollision();

        if (CanWallRun())
        {
            if (m_PlayerMovement.enabled)
            {
                m_PlayerMovement.enabled = false;
                m_PlayerMovement.Velocity = Vector3.zero;

                //Push player towards the wall
                Plane wallPlane = new Plane(m_WallHit.normal, m_WallHit.point);
                m_CharacterController.Move(-m_WallHit.normal * (wallPlane.GetDistanceToPoint(transform.position) - m_CharacterController.radius));

                m_Velocity.y = Mathf.Sqrt(m_RunningHeight * -2.0f * m_Gravity);
            }

            WallRun();
            WallJump();
        }
        else
        {
            CheckForWall();

            m_PlayerMovement.enabled = true;
            m_CanWallJump = true;
        }
    }

    private void WallRun()
    {
        //Find what direction to move; 1 = left, -1 = right
        Vector3 direction = Vector3.Cross(transform.forward, m_WallHit.normal);
        MoveDirection = (Vector3.Dot(direction, Vector3.up) > 0.0f) ? 1 : -1;
        
        m_CharacterController.Move(Vector3.Cross(m_WallHit.normal, Vector3.up) * MoveDirection * m_PlayerMovement.Speed * Time.deltaTime);

        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_CharacterController.Move(m_Velocity * Time.deltaTime);
    }

    private void WallJump()
    {
        if (Input.GetButtonDown("Jump") && m_CanWallJump)
        {
            //Player must face away from wall to jump
            if (Vector3.Angle(-m_WallHit.normal, transform.forward) > 90.0f)
            {
                m_PlayerMovement.Velocity =
                    m_WallHit.normal * m_WallJump +
                    Vector3.up * Mathf.Sqrt(m_PlayerMovement.JumpHeight * -2.0f * m_PlayerMovement.Gravity);

                m_CanWallJump = false;
            }
        }
    }

    private void WallCollision()
    {
        m_WallHit = new RaycastHit();
        float wallDistance = Mathf.Infinity;

        //Cast rays in multiple directions using the player's look direction as reference
        for (int i = -2; i <= 2; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.AngleAxis(65.0f * i, Vector3.up) * transform.forward, out RaycastHit objectHit, m_CharacterController.radius + m_RayLength))
            {
                //If wall is perpendicular to the ground and player is moving alongside the wall
                if (Vector3.Dot(objectHit.normal, Vector3.up) == 0 && Vector3.Dot(m_MoveSpeed, transform.forward) > 0)
                {
                    if (objectHit.distance < wallDistance)
                    {
                        wallDistance = objectHit.distance;
                        m_WallHit = objectHit;
                    }
                }
            }
        }
    }

    private void CheckForWall()
    {
        m_WallFound = false;

        //Player has to be looking at wall to be able to wall run
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit objectHit, m_CharacterController.radius + m_WallCheckDistance))
        {
            if (Vector3.Dot(objectHit.normal, Vector3.up) == 0)
            {
                if (objectHit.collider != null && objectHit.collider == m_WallHit.collider)
                {
                    m_WallFound = true;
                }
            }
        }
    }

    public bool CanWallRun()
    {
        return
            m_WallHit.collider != null && 
            m_WallHit.collider.tag == "Runnable Wall" &&
            m_WallFound &&
            m_CanWallJump &&
            !m_CharacterController.isGrounded &&
            m_PlayerMovement.Velocity.y >= 0.0f;
    }
}
