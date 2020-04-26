using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : PlayerAbility
{
    private readonly PlayerMovement m_PlayerMovement;

    public PlayerSpeed(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    public override void Start()
    {
        m_PlayerMovement.Speed = m_PlayerMovement.NormalSpeed * m_PlayerMovement.BoostSpeedFactor;
    }

    public override void Exit()
    {
        m_PlayerMovement.Speed = m_PlayerMovement.NormalSpeed;
    }
}
