﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField, Range(5.0f, 350.0f)]
    private float m_MouseSensitivity = 100f;

    private Transform m_PlayerBody;
    private Camera m_Camera;

    private float m_XRotation;
    private float m_YRotation;
    private float m_ZRotation;
    private float m_FieldOfView;

    public Transform CameraTransform { get => m_Camera.transform; }

    public Vector3 OriginalPosition { get; private set; }

    public float ZRotation { get => m_ZRotation; set => m_ZRotation = value; }
    public float FieldOfView { get => m_FieldOfView; set => m_FieldOfView = value; }

    public float NormalFOV { get; private set; }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        m_PlayerBody = transform.parent.GetComponent<Transform>();
        m_Camera = GetComponent<Camera>();

        m_XRotation = 0.0f;
        m_YRotation = 0.0f;
        m_ZRotation = 0.0f;
        m_FieldOfView = m_Camera.fieldOfView;

        NormalFOV = m_Camera.fieldOfView;

        OriginalPosition = m_Camera.transform.localPosition;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;

        m_XRotation -= mouseY;
        m_XRotation = Mathf.Clamp(m_XRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(m_XRotation, m_YRotation, m_ZRotation);
        m_PlayerBody.Rotate(Vector3.up * mouseX);

        m_Camera.fieldOfView = m_FieldOfView;
    }
}
