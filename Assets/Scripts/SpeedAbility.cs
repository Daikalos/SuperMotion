using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAbility
{
    private readonly PlayerMovement m_PlayerMovement;
    private float boostSpeed = 20f;

    public SpeedAbility(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }
    
    // Update is called once per frame
    public void Update()
    {
        m_PlayerMovement.Speed = boostSpeed;
    }
}
