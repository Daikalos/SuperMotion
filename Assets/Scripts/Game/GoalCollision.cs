using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.SetState(GameState.LevelComplete);
            AudioManager.m_Instance.PlayOnce("Win");
        }
    }
}
