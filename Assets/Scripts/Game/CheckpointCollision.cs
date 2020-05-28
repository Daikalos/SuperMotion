    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollision : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_Particles = null;

    [SerializeField, Tooltip("Spawn Position relative to transform")]
    private Vector3 m_SpawnPosition = Vector3.up;

    public bool IsFlagged { get; set; }

    private void Start()
    {
        IsFlagged = false;
    }

    /// <summary>
    /// Depending on if this is current checkpoint or not, set suitable color to particles
    /// </summary>
    public void SetColor()
    {
        ParticleSystem.MainModule main = m_Particles.main;
        main.startColor = (IsFlagged) ? Color.blue : Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsFlagged) //If this object is not flagged as current checkpoint, update it
            {
                other.GetComponent<PlayerCheckpoint>().SetCheckpoint(gameObject, m_SpawnPosition);
                AudioManager.m_Instance.PlayOnce("Checkpoint");
            }
        }
    }
}
