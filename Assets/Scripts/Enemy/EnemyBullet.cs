using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField, Tooltip("Force applied on bullet when fired"), Range(0.0f, 1600.0f)]
    private float m_Speed = 1400.0f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * m_Speed);
    }
}
