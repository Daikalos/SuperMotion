using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraEffects : MonoBehaviour
{
    [Header("Speed Ability")]
    [SerializeField, Tooltip("FOV when player is using the speed ability"), Range(0.0f, 30.0f)]
    private float m_SpeedFOV = 10.0f;
    [SerializeField, Tooltip("Speed the camera adjusts the FOV"), Range(0.0f, 50.0f)]
    private float m_SpeedSmoothFOV = 4.0f;
    [SerializeField, Tooltip("Speed the camera adjusts the FOV to normal"), Range(0.0f, 50.0f)]
    private float m_SpeedResetFOV = 6.0f;

    [Header("Jump Ability")]
    [SerializeField, Tooltip("The boundary for downward velocity after shaking becomes visible"), Range(-30.0f, 0.0f)]
    private float m_CameraShakeBounds = -20.0f;
    [SerializeField, Tooltip("How long the camera should shake"), Range(0.0f, 2.0f)]
    private float m_CameraShakeDuration = 0.15f;
    [SerializeField, Tooltip("Amplitude of the shake"), Range(0.0f, 2.0f)]
    private float m_CameraShakeAmount = 0.08f;
    [SerializeField, Tooltip("The amplitude limit of the shake"), Range(0.0f, 2.0f)]
    private float m_CameraShakeLimit = 0.20f;

    [Header("Wall Running")]
    [SerializeField, Tooltip("Speed of which the camera moves towards the tilt value"), Range(0.0f, 50.0f)]
    private float m_CameraTiltSpeed = 8.0f;
    [SerializeField, Tooltip("Angle in degrees camera rotates when wall running"), Range(0.0f, 90.0f)]
    private float m_CameraTilt = 25.0f;

    private CharacterController m_CharacterController;
    private PlayerMovement m_PlayerMovement;
    private PlayerWallRunning m_PlayerWallRunning;
    private PlayerLook m_PlayerLook;

    private Vector3 m_MoveSpeed;
    private Vector3 m_LastPos;

    private bool m_IsCameraShaking;
    private float m_CameraShakeTimer;
    private float m_CameraShakeAmplitude;

    public Vector3 Velocity { get; set; }
    public bool CheckCameraShake { get; set; }

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerWallRunning = GetComponent<PlayerWallRunning>();
        m_PlayerLook = GetComponentInChildren<PlayerLook>();

        m_IsCameraShaking = false;
        m_CameraShakeTimer = 0.0f;
        m_CameraShakeAmplitude = 0.0f;
    }

    void Update()
    {
        m_MoveSpeed = transform.position - m_LastPos;
        m_LastPos = transform.position;

        SpeedAbility();
        JumpAbility();
        WallRunning();
    }

    private void SpeedAbility()
    {
        if ((m_MoveSpeed.x != 0.0f || m_MoveSpeed.z != 0.0f) && m_PlayerMovement.Speed != m_PlayerMovement.NormalSpeed)
        {
            float newFOV = Mathf.Clamp((m_CharacterController.velocity.magnitude / m_PlayerMovement.Speed) * m_SpeedFOV, 0.0f, m_SpeedFOV);
            m_PlayerLook.FieldOfView = Mathf.Lerp(m_PlayerLook.FieldOfView, m_PlayerLook.NormalFOV + newFOV, m_SpeedSmoothFOV * Time.deltaTime);
        }
        else
        {
            m_PlayerLook.FieldOfView = Mathf.Lerp(m_PlayerLook.FieldOfView, m_PlayerLook.NormalFOV, m_SpeedResetFOV * Time.deltaTime);
        }
    }

    private void JumpAbility()
    {
        if (CheckCameraShake)
        {
            if (!m_IsCameraShaking && Velocity.y < m_CameraShakeBounds)
            {
                m_IsCameraShaking = true;
                m_CameraShakeTimer = m_CameraShakeDuration;

                m_CameraShakeAmplitude = Mathf.Clamp((m_PlayerMovement.Velocity.y / m_CameraShakeBounds) * m_CameraShakeAmount, m_CameraShakeAmount, m_CameraShakeLimit);
            }

            Velocity = Vector3.zero;
            CheckCameraShake = false;
        }

        if (m_IsCameraShaking && m_CameraShakeTimer > 0.0f)
        {
            m_PlayerLook.CameraTransform.localPosition = m_PlayerLook.OriginalPosition + Random.insideUnitSphere * m_CameraShakeAmplitude;
            m_CameraShakeTimer -= Time.deltaTime;
        }
        else
        {
            m_IsCameraShaking = false;
            m_CameraShakeTimer = 0.0f;
            m_PlayerLook.CameraTransform.localPosition = m_PlayerLook.OriginalPosition;
        }
    }

    private void WallRunning()
    {
        if (m_PlayerWallRunning.CanWallRun() && m_PlayerWallRunning.enabled)
        {
            m_PlayerLook.ZRotation = Mathf.LerpAngle(m_PlayerLook.ZRotation, m_CameraTilt * -m_PlayerWallRunning.MoveDirection, m_CameraTiltSpeed * Time.deltaTime);
        }
        else
        {
            m_PlayerLook.ZRotation = Mathf.LerpAngle(m_PlayerLook.ZRotation, 0.0f, m_CameraTiltSpeed * Time.deltaTime);
        }
    }
}
