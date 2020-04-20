﻿using System.Collections;
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

    [Header("Camera Attributes")]
    [SerializeField, Tooltip("FOV when player is using the speed ability"), Range(0.0f, 30.0f)]
    private float m_SpeedFOV = 10.0f;
    [SerializeField, Tooltip("Speed the camera adjusts the FOV"), Range(0.0f, 1.0f)]
    private float m_SpeedSmoothFOV = 0.10f;
    [SerializeField, Tooltip("Speed the camera adjusts the FOV to normal"), Range(0.0f, 1.0f)]
    private float m_SpeedResetFOV = 0.20f;
    [SerializeField, Tooltip("The boundary for downward velocity after shaking becomes visible"), Range(-30.0f, 0.0f)]
    private float m_JumpShakeBounds = -20.0f;
    [SerializeField, Tooltip("How long the camera should shake"), Range(0.0f, 2.0f)]
    private float m_JumpShakeDuration = 0.15f;
    [SerializeField, Tooltip("Amplitude of the shake"), Range(0.0f, 2.0f)]
    private float m_JumpShakeAmount = 0.08f;
    [SerializeField, Tooltip("The amplitude limit of the shake"), Range(0.0f, 2.0f)]
    private float m_JumpShakeLimit = 0.20f;

    private CharacterController
        m_CharacterController;
    private PlayerLook
        m_PlayerLook;
    private GameObject
        m_PreviousSlope,
        m_CurrentSlope;
    private Vector3
        m_Velocity,
        m_HitNormal,
        m_MoveSpeed,
        m_LastPos;
    private bool
        m_IsGrounded,
        m_CanJump,
        m_IsCameraShaking;
    private float
        m_SlopeLimit,
        m_JumpShakeTimer,
        m_JumpShakeAmplitude;

    public Vector3 Velocity { get => m_Velocity; set => m_Velocity = value; }

    public bool CanJump { get => m_CanJump; set => m_CanJump = value; }

    public float Speed { get => m_Speed; set => m_Speed = value; }
    public float JumpHeight { get => m_JumpHeight; set => m_JumpHeight = value; }
    public float Gravity { get => m_Gravity; set => m_Gravity = value; }

    public float SlopeJump { get => m_SlopeJump; set => m_SlopeJump = value; }

    public float NormalSpeed { get; set; }
    public float NormalJumpHeight { get; set; }

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerLook = GetComponentInChildren<PlayerLook>();

        m_HitNormal = Vector3.zero;
        m_Velocity = Vector3.zero;
        m_LastPos = transform.position;

        m_IsGrounded = false;
        m_CanJump = true;
        m_IsCameraShaking = false;

        m_SlopeLimit = m_CharacterController.slopeLimit;
        m_JumpShakeTimer = 0.0f;
        m_JumpShakeAmplitude = 0.0f;

        NormalSpeed = Speed;
        NormalJumpHeight = JumpHeight;
    }

    void Update()
    {
        m_MoveSpeed = transform.position - m_LastPos;
        m_LastPos = transform.position;

        Movement();
        CameraEffects();
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
        
        moveDirection *= ((m_Velocity.x != 0 || m_Velocity.z != 0) ? 0.0f : 1.0f);

        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_CharacterController.Move((moveDirection + m_Velocity) * Time.deltaTime);

        if ((horizInput != 0 || vertInput != 0) && OnSlope())
        {
            //Push character down to create smooth movement when walking down slopes
            m_CharacterController.Move(Vector3.down * (m_CharacterController.height / 2) * m_SlopeForce * Time.deltaTime);
        }
    }

    private void JumpInput()
    {
        if (m_CharacterController.isGrounded || m_CanJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                m_CharacterController.slopeLimit = 90.0f;
                m_CanJump = false;

                if (m_IsGrounded)
                {
                    m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
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
    }

    private void CameraEffects()
    {
        //Speed Ability Effect
        m_PlayerLook.FieldOfView = ((m_MoveSpeed.x != 0.0f || m_MoveSpeed.z != 0.0f) && m_Speed != NormalSpeed) ?
            Mathf.SmoothStep(m_PlayerLook.FieldOfView, m_PlayerLook.NormalFOV +
            Mathf.Clamp((m_MoveSpeed.magnitude / (m_Speed * Time.deltaTime)), 0.0f, 1.0f / (m_Speed * Time.deltaTime)) * m_SpeedFOV, m_SpeedSmoothFOV) :
            Mathf.SmoothStep(m_PlayerLook.FieldOfView, m_PlayerLook.NormalFOV, m_SpeedResetFOV);

        //Jump Ability Effect
        if (m_CharacterController.isGrounded)
        {
            if (!m_IsCameraShaking && m_Velocity.y < m_JumpShakeBounds)
            {
                m_IsCameraShaking = true;
                m_JumpShakeTimer = m_JumpShakeDuration;

                m_JumpShakeAmplitude = Mathf.Clamp((m_Velocity.y / m_JumpShakeBounds) * m_JumpShakeAmount, m_JumpShakeAmount, m_JumpShakeLimit);
            }
        }

        if (m_IsCameraShaking && m_JumpShakeTimer > 0.0f)
        {
            m_PlayerLook.CameraTransform.localPosition = m_PlayerLook.OriginalPosition + Random.insideUnitSphere * m_JumpShakeAmplitude;
            m_JumpShakeTimer -= Time.deltaTime;
        }
        else
        {
            m_IsCameraShaking = false;
            m_JumpShakeTimer = 0.0f;
            m_PlayerLook.CameraTransform.localPosition = m_PlayerLook.OriginalPosition;
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
            m_Velocity.y = m_Gravity * Time.deltaTime;

            if (m_IsGrounded)
            {
                m_CanJump = true;
            }
            else
            {
                //Allow player to jump once on a slope higher than slopelimit
                if (m_PreviousSlope != m_CurrentSlope)
                {
                    if (Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out RaycastHit hit, (m_CharacterController.height / 2) * m_SlopeRayLength))
                    {
                        m_CanJump = true;
                    }
                }

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

    private bool OppositeSigns(float x, float y)
    {
        return Mathf.Sign(x) != Mathf.Sign(y);
    }

    private void OnControllerColliderHit(ControllerColliderHit objectHit)
    {
        m_CurrentSlope = objectHit.gameObject;
        m_HitNormal = objectHit.normal;

        if (m_PreviousSlope != objectHit.gameObject && m_IsGrounded)
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
