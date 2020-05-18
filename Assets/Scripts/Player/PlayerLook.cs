using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField, Range(0.0f, 20.0f)]
    private float m_MouseSensitivity = 1.7f;

    private Vector3 m_CameraRotation;
    private float m_FieldOfView;

    public Camera MainCamera { get; private set; }

    public Vector3 CameraRotation { get => m_CameraRotation; set => m_CameraRotation = value; }
    public Vector3 CameraStartPos { get; private set; }

    public float FieldOfView { get => m_FieldOfView; set => m_FieldOfView = value; }
    public float NormalFOV { get; private set; }
    
    private void Awake()
    {
        MainCamera = Camera.main;

        m_MouseSensitivity *= PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        m_FieldOfView = MainCamera.fieldOfView;

        CameraStartPos = MainCamera.transform.localPosition;
        NormalFOV = m_FieldOfView;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;

            m_CameraRotation.x -= mouseY;
            m_CameraRotation.x = Mathf.Clamp(m_CameraRotation.x, -90f, 90f);

            transform.localRotation = Quaternion.Euler(m_CameraRotation);
            transform.parent.Rotate(Vector3.up * mouseX);

            MainCamera.fieldOfView = m_FieldOfView;
        }
    }
}
