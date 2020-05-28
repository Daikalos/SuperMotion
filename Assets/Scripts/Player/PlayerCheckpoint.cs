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

        LoadCheckpoint();
    }

    private void LoadCheckpoint()
    {
        LevelHandler.Instance.NewLevel();

        //No checkpoint is set yet
        if (!LevelHandler.Instance.CheckpointSet)
        {
            LevelHandler.Instance.CheckpointPosition = transform.position;
            LevelHandler.Instance.CheckpointRotation = transform.rotation.eulerAngles;
            LevelHandler.Instance.CheckpointTime = 0.0f;
        }

        //Deactivate character controller temporarily to be able to change position of player
        m_CharacterController.enabled = false;
        transform.position = LevelHandler.Instance.CheckpointPosition;
        m_CharacterController.enabled = true;

        transform.rotation = Quaternion.Euler(LevelHandler.Instance.CheckpointRotation);
    }

    public void SetCheckpoint(GameObject checkpoint, Vector3 spawnPos)
    {
        //If there is a checkpoint set, no need to use countdown whenever player loads via checkpoint button later
        LevelHandler.Instance.Countdown = false;

        //Update checkpoint
        LevelHandler.Instance.CheckpointPosition = checkpoint.transform.position + spawnPos;
        LevelHandler.Instance.CheckpointRotation = new Vector3(0.0f, checkpoint.transform.rotation.eulerAngles.y, 0.0f);
        LevelHandler.Instance.CheckpointTime = m_Timer.TimePassed;
        LevelHandler.Instance.CheckpointSet = true;

        //Update each checkpoint according to current one
        foreach (GameObject c in m_Checkpoints)
        {
            CheckpointCollision cpCollision = c.GetComponent<CheckpointCollision>();
            cpCollision.IsFlagged = (c == checkpoint);

            cpCollision.SetColor();
        }
    }
}
