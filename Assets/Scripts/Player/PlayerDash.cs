using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerAbility
{
    private readonly CharacterController m_CharacterController;
    private readonly PlayerMovement m_PlayerMovement;
    private readonly PlayerWallRunning m_PlayerWallRunning;

    private Vector3 m_MoveDirection;

    private bool m_CanDash;
    private bool m_IsDashing;
    private float m_DashTimer;

    public PlayerDash(GameObject playerObject)
    {
        m_CharacterController = playerObject.GetComponent<CharacterController>();
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
        m_PlayerWallRunning = playerObject.GetComponent<PlayerWallRunning>();

        m_MoveDirection = Vector3.zero;

        m_CanDash = true;
        m_IsDashing = false;
        m_DashTimer = 0.0f;
    }

    public override void Start()
    {
        m_DashTimer = 0.0f;
    }

    public override void Exit()
    {
        m_PlayerMovement.enabled = true;
        m_PlayerWallRunning.enabled = true;
    }

    public override void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (!m_CharacterController.isGrounded)
        {
            if (Input.GetMouseButtonDown(1) && m_CanDash)
            {
                m_MoveDirection = (m_CharacterController.transform.right * horizInput + m_CharacterController.transform.forward * vertInput).normalized;

                //Dash only if player is moving while in air
                if (m_MoveDirection.magnitude > 0.0f)
                {
                    m_DashTimer = m_PlayerMovement.DashTime;
                    m_PlayerMovement.Velocity = Vector3.zero;

                    m_CanDash = false;
                    m_IsDashing = true;

                    AudioManager.m_Instance.Play("Dash");
                }
            }
        }
        else
        { 
            m_DashTimer = 0.0f;
            m_MoveDirection = Vector3.zero;
        }

        DashMovement();
    }

    private void DashMovement()
    {
        if (m_DashTimer > 0.0f)
        {
            //Disable controls while dash is active
            m_PlayerMovement.enabled = false;
            m_PlayerWallRunning.enabled = false;

            m_CharacterController.Move(m_MoveDirection * m_PlayerMovement.DashSpeed * Time.deltaTime);
            m_DashTimer -= Time.deltaTime;
        }
        else if (m_IsDashing)
        {
            //Reenable controls to player
            m_PlayerMovement.enabled = true;
            m_PlayerWallRunning.enabled = true;

            m_IsDashing = false;
        }
    }

    public override void ConstantUpdate()
    {
        if (m_CharacterController.isGrounded)
        {
            m_CanDash = true;
        }
    }
}
