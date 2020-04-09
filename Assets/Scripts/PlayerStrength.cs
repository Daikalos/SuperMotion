using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrength
{
    private readonly PlayerMovement m_PlayerMovement;

    public PlayerStrength(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Main update method for ability, called each frame
    /// </summary>
    public void Update()
    {
        
    }
}
