using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed
{
    private readonly PlayerMovement m_PlayerMovement;
    private float boostSpeed = 20f;

    public PlayerSpeed(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }
    
    // Update is called once per frame
    public void Update()
    {
        m_PlayerMovement.Speed = boostSpeed;
    }
}
