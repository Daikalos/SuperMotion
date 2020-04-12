using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAbility
{
    private readonly PlayerMovement m_PlayerMovement;
    private bool isBoosting = false;
    private float boostSpeed = 20f;
    private float normalSpeed = 10f;

    public SpeedAbility(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && isBoosting == false)
        {
            m_PlayerMovement.Speed = boostSpeed;
            isBoosting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && isBoosting == true)
        {
            m_PlayerMovement.Speed = normalSpeed;
            isBoosting = false;
        }
    }
}
