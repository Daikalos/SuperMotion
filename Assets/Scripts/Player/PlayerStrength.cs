using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrength : PlayerAbility
{
    private readonly PlayerMovement m_PlayerMovement;
    private readonly Collider m_PlayerCollider;
    private readonly Camera m_Camera;

    public PlayerStrength(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
        m_PlayerCollider = playerObject.GetComponent<Collider>();
        m_Camera = playerObject.GetComponentInChildren<PlayerLook>().MainCamera;
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.m_Instance.Play("Fist");

            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out RaycastHit objectHit, m_PlayerMovement.PunchDistance))
            {
                if (objectHit.collider.tag == "Glass")
                {
                    objectHit.collider.GetComponent<DestructibleGlass>().Shatter(m_PlayerCollider, m_Camera.transform.forward, objectHit);
                    AudioManager.m_Instance.Play("BrokenGlass");
                }

                if (objectHit.collider.tag == "Ball")
                {
                    objectHit.collider.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * m_PlayerMovement.PunchStrength);
                }

                if (objectHit.collider.tag == "Enemy")
                {

                }

                AudioManager.m_Instance.Play("Hit");
            }
        }
    }
}
