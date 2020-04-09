using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAbility
{
    private readonly PlayerMovement m_PlayerMovement;

    public SpeedAbility(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }
    
    // Update is called once per frame
    public void Update()
    {
        m_PlayerMovement.Speed = 20f;
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    m_PlayerMovement.Speed = 10f;
        //}         
    }
}
