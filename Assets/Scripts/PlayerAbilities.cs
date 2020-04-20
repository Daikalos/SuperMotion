using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;

    private PlayerDash m_PlayerDash;
    private PlayerJump m_PlayerJump;
    private PlayerStrength m_PlayerStrength;
    private PlayerSpeed m_PlayerSpeed;

    private UpdateAbility m_UpdateAbility;

    public GameObject speedText, dashText, jumpText, strengthText;
    private GameObject previousText;

    public void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();

        m_PlayerDash = new PlayerDash(gameObject);
        m_PlayerJump = new PlayerJump(gameObject);
        m_PlayerStrength = new PlayerStrength(gameObject);
        m_PlayerSpeed = new PlayerSpeed(gameObject);

        //Standard at start
        m_UpdateAbility = m_PlayerStrength.Update;
        previousText = strengthText;
    }

    void Update()
    {
        KeyInput();

        m_UpdateAbility();
    }

    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAbility(m_PlayerSpeed.Update);
            ActivateAbilityText(speedText);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAbility(m_PlayerDash.Update);
            ActivateAbilityText(dashText);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAbility(m_PlayerJump.Update);
            ActivateAbilityText(jumpText);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetAbility(m_PlayerStrength.Update);
            ActivateAbilityText(strengthText);
        }
    }

    private void ResetValues()
    {
        m_PlayerMovement.Speed = m_PlayerMovement.NormalSpeed;
        m_PlayerMovement.JumpHeight = m_PlayerMovement.NormalJumpHeight;
        m_PlayerMovement.SlopeJump = m_PlayerMovement.NormalSlopeJump;
    }

    private void SetAbility(UpdateAbility updateAbility)
    {
        ResetValues();
        m_UpdateAbility = updateAbility;

        AudioManager.instance.Play("AbilitySelect");
    }

    private void ActivateAbilityText(GameObject text)
    {
        previousText.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        previousText = text;
    }

    private delegate void UpdateAbility();
}
