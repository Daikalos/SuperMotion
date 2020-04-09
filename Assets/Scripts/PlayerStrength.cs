using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrength
{
    private readonly PlayerMovement m_PlayerMovement;
    private readonly Transform m_CameraTransform;

    public PlayerStrength(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
        m_CameraTransform = playerObject.GetComponentInChildren<Camera>().transform;
    }

    /// <summary>
    /// Main update method for ability, called each frame
    /// </summary>
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out RaycastHit objectHit, 5.0f))
            {
                if (objectHit.collider.tag == "Glass")
                {
                    objectHit.collider.GetComponent<DestructibleGlass>().Shatter(m_CameraTransform.forward, objectHit);
                }
            }
        }
    }
}
