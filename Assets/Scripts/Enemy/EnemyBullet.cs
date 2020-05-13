using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the bullet"), Range(0.0f, 120.0f)]
    private float m_Speed = 12.0f;

    void Update()
    {
        transform.position += transform.forward * m_Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            if (other.tag == "Player")
            {
                AudioManager.m_Instance.Play("Death");
                GameManager.Instance.SetState(GameState.GameOver);
            }
            Destroy(gameObject);
        }
    }
}
