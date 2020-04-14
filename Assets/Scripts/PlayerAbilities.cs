using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //private PlayerDash m_PlayerDash;
    private PlayerJump m_PlayerJump;
    private PlayerStrength m_PlayerStrength;
    private SpeedAbility m_SpeedAbility;

    private UpdateAbility m_UpdateAbility;

    private PlayerMovement m_PlayerMovement;

    public void Start()
    {
        //m_PlayerDash = new PlayerDash(gameObject);
        m_PlayerJump = new PlayerJump(gameObject);
        m_PlayerStrength = new PlayerStrength(gameObject);
        m_SpeedAbility = new SpeedAbility(gameObject);

        //Standard at start
        m_UpdateAbility = m_PlayerStrength.Update;

        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        KeyInput();
        
        m_UpdateAbility();
    }

    private void KeyInput()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_UpdateAbility = m_SpeedAbility.Update;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //m_UpdateAbility = m_PlayerDash.Update;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAbility(m_PlayerJump.Update);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetAbility(m_PlayerStrength.Update);
        }
    }

    private void ResetValues()
    {
        m_PlayerMovement.JumpHeight = m_PlayerMovement.NormalJumpHeight; 
        m_PlayerMovement.Speed = m_PlayerMovement.NormalSpeed;
    }

    private void SetAbility(UpdateAbility updateAbility)
    {
        ResetValues();
        m_UpdateAbility = updateAbility;
    }

    private delegate void UpdateAbility();
}
