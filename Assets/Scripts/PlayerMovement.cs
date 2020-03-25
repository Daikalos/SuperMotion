using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController m_CharacterController;
    private Vector3 m_MoveDirection = Vector3.zero;

    private float m_Velocity = 0.0f;

    [Range(1.0f, 80.0f)]
    public float m_Speed = 10.0f;
    [Range(2.0f, 20.0f)]
    public float m_JumpSpeed = 5.0f;
    [Range(2.0f, 40.0f)]
    public float m_Gravity = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rörelse
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        m_MoveDirection = transform.right * x + transform.forward * z;

        //Jump
        if (Input.GetButton("Jump") && m_CharacterController.isGrounded)
        {
            m_Velocity = m_JumpSpeed;
        }
        if (!m_CharacterController.isGrounded)
        {
            m_Velocity -= m_Gravity * Time.deltaTime;
            m_MoveDirection.y = m_Velocity;
        }

        m_CharacterController.Move(m_MoveDirection * m_Speed * Time.deltaTime);
    }
}
