using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField, Range(0.0f, 20.0f)]
    private float m_MouseSensitivity = 1.7f;

    private Camera m_Camera;

    private float m_XRotation;
    private float m_YRotation;
    private float m_ZRotation;
    private float m_FieldOfView;

    public Camera MainCamera { get => m_Camera; }

    public Vector3 CameraStartPos { get; private set; }

    public float ZRotation { get => m_ZRotation; set => m_ZRotation = value; }
    public float FieldOfView { get => m_FieldOfView; set => m_FieldOfView = value; }
    public float NormalFOV { get; private set; }
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_Camera = Camera.main;

        m_XRotation = 0.0f;
        m_YRotation = 0.0f;
        m_ZRotation = 0.0f;
        m_FieldOfView = m_Camera.fieldOfView;
        m_MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f) * m_MouseSensitivity;

        CameraStartPos = m_Camera.transform.localPosition;
        NormalFOV = m_FieldOfView;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;

            m_XRotation -= mouseY;
            m_XRotation = Mathf.Clamp(m_XRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(m_XRotation, m_YRotation, m_ZRotation);
            transform.parent.Rotate(Vector3.up * mouseX);

            m_Camera.fieldOfView = m_FieldOfView;
        }
    }
}
