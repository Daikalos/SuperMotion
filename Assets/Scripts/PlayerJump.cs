using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump
{
    private readonly PlayerMovement m_PlayerMovement;

    public PlayerJump(GameObject playerObject)
    {
        m_PlayerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        m_PlayerMovement.JumpHeight = m_PlayerMovement.NormalJumpHeight * m_PlayerMovement.HighJumpFactor;
    }
}
