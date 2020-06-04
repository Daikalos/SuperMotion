using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionPlayer : MonoBehaviour
{
    private EnemyBullet m_EnemyBullet;

    private void Start()
    {
        m_EnemyBullet = transform.parent.GetComponent<EnemyBullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_EnemyBullet.ObjectCollision)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.m_Instance.PlayOnce("GameOver");
                GameManager.Instance.SetState(GameState.GameOver);

                Destroy(transform.parent.gameObject);
            }
        }
    }
}
