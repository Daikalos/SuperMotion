using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerAbility
{
    private readonly PlayerMovement m_PlayerMovement;

    public PlayerJump(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    public override void Start()
    {
        m_PlayerMovement.JumpHeight = m_PlayerMovement.NormalJumpHeight * m_PlayerMovement.HighJumpFactor;
        m_PlayerMovement.SlopeJump = m_PlayerMovement.NormalSlopeJump * m_PlayerMovement.SlopeHighJumpFactor;
    }

    public override void Exit()
    {
        m_PlayerMovement.JumpHeight = m_PlayerMovement.NormalJumpHeight;
        m_PlayerMovement.SlopeJump = m_PlayerMovement.NormalSlopeJump;
    }
}
