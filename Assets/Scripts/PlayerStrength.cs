using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrength
{
    private readonly Collider m_PlayerCollider;
    private readonly Transform m_CameraTransform;

    public PlayerStrength(GameObject playerObject)
    {
        m_PlayerCollider = playerObject.GetComponent<Collider>();
        m_CameraTransform = playerObject.GetComponentInChildren<Camera>().transform;
    }

    /// <summary>
    /// Main update method for ability, called each frame
    /// </summary>
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.Play("Fist");

            if (Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out RaycastHit objectHit, 5.0f))
            {
                if (objectHit.collider.tag == "Glass")
                {
                    objectHit.collider.GetComponent<DestructibleGlass>().Shatter(m_PlayerCollider, m_CameraTransform.forward, objectHit);
                    AudioManager.instance.Play("BrokenGlass");
                }
                AudioManager.instance.Play("Hit");
            }
        }
    }
}
