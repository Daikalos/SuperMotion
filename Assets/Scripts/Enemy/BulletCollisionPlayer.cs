using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.m_Instance.Play("Death");
            GameManager.Instance.SetState(GameState.GameOver);

            Destroy(transform.parent.gameObject);
        }
    }
}
