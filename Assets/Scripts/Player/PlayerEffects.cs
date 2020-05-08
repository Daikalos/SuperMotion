using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Header("Speed Ability")]
    [SerializeField, Tooltip("FOV when player is using the speed ability"), Range(0.0f, 30.0f)]
    private float m_SpeedFOV = 10.0f;
    [SerializeField, Tooltip("Speed the camera adjusts the FOV"), Range(0.0f, 50.0f)]
    private float m_SpeedSmoothFOV = 4.0f;
    [SerializeField, Tooltip("Speed the camera adjusts the FOV to normal"), Range(0.0f, 50.0f)]
    private float m_SpeedResetFOV = 6.0f;

    [Header("Land Effects")]
    [SerializeField]
    private ParticleSystem m_ParticleDustEffect = null;
    [SerializeField, Tooltip("The boundary for downward velocity after landing effects becomes active"), Range(-60.0f, 0.0f)]
    private float m_LandEffectBounds = -20.0f;
    [SerializeField, Tooltip("How fast the head bob occurs"), Range(0.0f, 30.0f)]
    private float m_HeadBobSpeed = 12.0f;
    [SerializeField, Tooltip("Amplitude of how far down the camera goes"), Range(0.0f, 2.0f)]
    private float m_HeadBobAmplitude = 0.26f;
    [SerializeField]
    private int m_ParticleCountMin = 10;
    [SerializeField]
    private int m_ParticleCountMax = 18;

    [Header("Wall Running")]
    [SerializeField, Tooltip("Speed of which the camera moves towards the tilt value"), Range(0.0f, 50.0f)]
    private float m_CameraTiltSpeed = 8.0f;
    [SerializeField, Tooltip("Angle in degrees camera rotates when wall running"), Range(0.0f, 90.0f)]
    private float m_CameraTilt = 25.0f;

    private CharacterController m_CharacterController;
    private PlayerMovement m_PlayerMovement;
    private PlayerWallRunning m_PlayerWallRunning;
    private PlayerLook m_PlayerLook;
    private Camera m_Camera;

    private Vector3 m_Velocity;
    private Vector3 m_MoveSpeed;
    private Vector3 m_LastPos;

    private bool m_StartHeadBob;
    private float m_HeadBobCounter;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerWallRunning = GetComponent<PlayerWallRunning>();
        m_PlayerLook = GetComponentInChildren<PlayerLook>();
        m_Camera = m_PlayerLook.MainCamera;

        m_StartHeadBob = false;
        m_HeadBobCounter = 0.0f;
    }

    void Update()
    {
        m_MoveSpeed = transform.position - m_LastPos;
        m_LastPos = transform.position;

        SpeedAbility();
        LandingEffect();
        WallRunning();
    }

    private void SpeedAbility()
    {
        //If player is moving and is using speed ability
        if ((m_MoveSpeed.x != 0.0f || m_MoveSpeed.z != 0.0f) && m_PlayerMovement.Speed != m_PlayerMovement.NormalSpeed)
        {
            //Assign new fov by the magnitude of the player's movement
            float newFOV = Mathf.Clamp((m_CharacterController.velocity.magnitude / m_PlayerMovement.Speed) * m_SpeedFOV, 0.0f, m_SpeedFOV);
            m_PlayerLook.FieldOfView = Mathf.Lerp(m_PlayerLook.FieldOfView, m_PlayerLook.NormalFOV + newFOV, m_SpeedSmoothFOV * Time.deltaTime);
        }
        else
        {
            //Reset fov to normal when not moving or not using ability
            m_PlayerLook.FieldOfView = Mathf.Lerp(m_PlayerLook.FieldOfView, m_PlayerLook.NormalFOV, m_SpeedResetFOV * Time.deltaTime);
        }
    }

    private void LandingEffect()
    {
        if (m_Velocity.y < m_LandEffectBounds)
        {
            m_StartHeadBob = true;

            //Emit dust particles from particle system situated at the bottom of the player
            m_ParticleDustEffect.Emit(Random.Range(m_ParticleCountMin, m_ParticleCountMax));
        }

        if (m_StartHeadBob)
        {
            if (m_HeadBobCounter < Mathf.PI)
            {
                //Use Sine curve
                m_HeadBobCounter += m_HeadBobSpeed * Time.deltaTime;
                m_Camera.transform.localPosition = m_PlayerLook.CameraStartPos + Vector3.down * (Mathf.Sin(m_HeadBobCounter) * m_HeadBobAmplitude);
            }
            else
            {
                m_Camera.transform.localPosition = m_PlayerLook.CameraStartPos;

                m_StartHeadBob = false;
                m_HeadBobCounter = 0;
            }
        }

        m_Velocity = Vector3.zero;
    }

    public void CheckLandingEffect(Vector3 velocity)
    {
        m_Velocity = velocity;
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
