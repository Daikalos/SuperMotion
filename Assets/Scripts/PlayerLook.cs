using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField, Range(5.0f, 350.0f)]
    private float m_MouseSensitivity = 100f;

    private Transform m_PlayerBody;
    private float m_XRotation; // Rotation along x-axis
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_PlayerBody = transform.parent.GetComponent<Transform>();
        m_XRotation = 0.0f;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;

        m_XRotation -= mouseY;
        m_XRotation = Mathf.Clamp(m_XRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(m_XRotation, 0f, 0f);
        m_PlayerBody.Rotate(Vector3.up * mouseX);
    }
}
