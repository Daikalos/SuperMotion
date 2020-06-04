using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            AudioManager.m_Instance.PlayOnce("GameOver");
            GameManager.Instance.SetState(GameState.GameOver);
        }
    }
}
