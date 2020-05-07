using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private Dictionary<string, PlayerAbility> m_PlayerAbilites;

    private StartAbility m_StartAbility;
    private ExitAbility m_ExitAbility;
    private UpdateAbility m_UpdateAbility;

    [SerializeField]
    private GameObject
        m_SpeedText = null,
        m_DashText = null,
        m_JumpText = null, 
        m_StrengthText = null;
    private GameObject m_PreviousText;

    public void Start()
    {
        m_PlayerAbilites = new Dictionary<string, PlayerAbility>();

        m_PlayerAbilites.Add("Speed", new PlayerSpeed(gameObject));
        m_PlayerAbilites.Add("Dash", new PlayerDash(gameObject));
        m_PlayerAbilites.Add("Jump", new PlayerJump(gameObject));
        m_PlayerAbilites.Add("Strength", new PlayerStrength(gameObject));

        //Standard at start
        m_StartAbility = m_PlayerAbilites["Strength"].Start;
        m_ExitAbility = m_PlayerAbilites["Strength"].Exit;
        m_UpdateAbility = m_PlayerAbilites["Strength"].Update;
        m_PreviousText = m_StrengthText;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            KeyInput();
            m_UpdateAbility();

            foreach (PlayerAbility ability in m_PlayerAbilites.Values)
            {
                ability.ConstantUpdate();
            }
        }
    }

    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAbility("Speed");
            ActivateAbilityText(m_SpeedText);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAbility("Dash");
            ActivateAbilityText(m_DashText);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAbility("Jump");
            ActivateAbilityText(m_JumpText);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetAbility("Strength");
            ActivateAbilityText(m_StrengthText);
        }
    }

    private void SetAbility(string abilityName)
    {
        m_ExitAbility();

        m_StartAbility = m_PlayerAbilites[abilityName].Start;
        m_ExitAbility = m_PlayerAbilites[abilityName].Exit;
        m_UpdateAbility = m_PlayerAbilites[abilityName].Update;

        m_StartAbility();

        AudioManager.m_Instance.Play("AbilitySelect");
    }

    private void ActivateAbilityText(GameObject text)
    {
        m_PreviousText.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        m_PreviousText = text;
    }

    private delegate void StartAbility();
    private delegate void ExitAbility();
    private delegate void UpdateAbility();
}
