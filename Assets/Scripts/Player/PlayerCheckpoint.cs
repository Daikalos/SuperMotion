using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private GameObject[] m_Checkpoints;
    private CharacterController m_CharacterController;

    void Start()
    {
        m_Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        m_CharacterController = GetComponent<CharacterController>();

        //No checkpoint is set yet
        if (CheckpointManager.Instance.Checkpoint == Vector3.zero)
        {
            CheckpointManager.Instance.Checkpoint = transform.position;
        }

        //Deactivate character controller to be able to change position of player
        m_CharacterController.enabled = false;
        transform.position = CheckpointManager.Instance.Checkpoint;
        m_CharacterController.enabled = true;
    }

    public void SetCheckpoint(GameObject checkpoint, Vector3 spawnPos)
    {
        //Update checkpoint
        CheckpointManager.Instance.Checkpoint = checkpoint.transform.position + spawnPos;

        //Update each checkpoint according to current one
        foreach (GameObject c in m_Checkpoints)
        {
            CheckpointCollision cpCollision = c.GetComponent<CheckpointCollision>();
            cpCollision.IsFlagged = (c == checkpoint);

            cpCollision.SetColor();
        }
    }
}
