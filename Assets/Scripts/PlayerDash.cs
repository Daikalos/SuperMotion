using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash
{
    private readonly CharacterController m_CharacterController;
    private readonly PlayerMovement m_PlayerMovement;
    private float dashSpeed;

    public PlayerDash(GameObject playerObject)
    {
        m_CharacterController = playerObject.GetComponent<CharacterController>();
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
        dashSpeed = 20f;
    }

    public void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = m_CharacterController.transform.right * horizInput + m_CharacterController.transform.forward * vertInput;

        if (Input.GetMouseButtonDown(1))
        {
            m_PlayerMovement.Velocity += moveDirection * dashSpeed;
            Debug.Log("Dash - Move direction: " + moveDirection);
        }
    }
}
