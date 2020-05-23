using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerMovingObject : MonoBehaviour
{
    [SerializeField, Range(0.0f, 2.0f)]
    private float m_RayLength = 1.1f;

    private CharacterController m_CharacterController;
    private PlayerMovement m_PlayerMovement;

    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (CollisionMovingObject(out RaycastHit objectHit))
        { 
            MoveTruck moveTruck = objectHit.collider.GetComponent<MoveTruck>();
            m_PlayerMovement.ExternalForce = objectHit.transform.forward * moveTruck.Speed;
        }
        else
        {
            m_PlayerMovement.ExternalForce = Vector3.zero;
        }
    }

    private bool CollisionMovingObject(out RaycastHit objectHit)
    {
        if (m_CharacterController.isGrounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, (m_CharacterController.height / 2) * m_RayLength))
            {
                if (rayHit.collider.CompareTag("MovingObject"))
                {
                    objectHit = rayHit;
                    return true;
                }
            }
        }

        objectHit = new RaycastHit();
        return false;
    }
}
