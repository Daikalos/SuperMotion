using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyCollision : MonoBehaviour
{
    [Tooltip("The Collider that kills the player (RIP)")]
    public Collider m_ColliderBox;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameManager.Instance.SetState(GameState.GameOver);
        }
    }

    private void Update()
    {
        OnTriggerEnter(m_ColliderBox);
    }
}
