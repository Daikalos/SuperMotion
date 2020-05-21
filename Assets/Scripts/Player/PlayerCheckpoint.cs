using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField]
    private Timer m_Timer = null;

    private GameObject[] m_Checkpoints;
    private CharacterController m_CharacterController;

    private void Awake()
    {
        m_Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        m_CharacterController = GetComponent<CharacterController>();

        //No checkpoint is set yet
        if (!CheckpointManager.Instance.CheckpointSet)
        {
            CheckpointManager.Instance.Checkpoint = transform.position;
            CheckpointManager.Instance.CheckpointTime = 0.0f;
        }

        //Deactivate character controller temporarily to be able to change position of player
        m_CharacterController.enabled = false;
        transform.position = CheckpointManager.Instance.Checkpoint;
        m_CharacterController.enabled = true;
    }

    public void SetCheckpoint(GameObject checkpoint, Vector3 spawnPos)
    {
        //Update checkpoint
        CheckpointManager.Instance.Checkpoint = checkpoint.transform.position + spawnPos;
        CheckpointManager.Instance.CheckpointTime = m_Timer.TimePassed;
        CheckpointManager.Instance.CheckpointSet = true;

        //Update each checkpoint according to current one
        foreach (GameObject c in m_Checkpoints)
        {
            CheckpointCollision cpCollision = c.GetComponent<CheckpointCollision>();
            cpCollision.IsFlagged = (c == checkpoint);

            cpCollision.SetColor();
        }
    }
}
