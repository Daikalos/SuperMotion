using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //private PlayerDash m_PlayerDash;
    //private PlayerJump m_PlayerJump;
    private PlayerStrength m_PlayerStrength;
    private SpeedAbility m_SpeedAbility;

    private UpdateAbility m_UpdateAbility;

    public virtual void Start()
    {
        //m_PlayerDash = new PlayerDash(gameObject);
        //m_PlayerJump = new PlayerJump(gameObject);
        m_PlayerStrength = new PlayerStrength(gameObject);
        m_SpeedAbility = new SpeedAbility(gameObject);

        //Standard at start
        m_UpdateAbility = m_PlayerStrength.Update;
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
            //m_UpdateAbility = m_PlayerJump.Update;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_UpdateAbility = m_PlayerStrength.Update;
        }
    }

    private delegate void UpdateAbility();
}
