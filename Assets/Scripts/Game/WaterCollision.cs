using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.SetState(GameState.GameOver);
            AudioManager.m_Instance.PlayOnce("Splash");
        }
    }
}
