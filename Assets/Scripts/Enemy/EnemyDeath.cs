using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField, Tooltip("The force applied when hit"), Range(0.0f, 5000.0f)]
    private float m_ImpactForce = 1500.0f;

    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = AudioManager.m_Instance.AddSpatialAudioSource(gameObject, "EnemyDeath");
    }

    public void EnemyHit(Vector3 hitDirection, Vector3 impactPoint)
    {
        //Apply force relative to impact point
        Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.AddForceAtPosition(hitDirection * m_ImpactForce, impactPoint);

        //Ignore collision between player and enemy
        Collider playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, GetComponent<Collider>());

        //Ignore raycasts so player cannot hit enemy again
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        //Destroy all scripts on enemy
        Array.ForEach(gameObject.GetComponents<MonoBehaviour>(), s => Destroy(s));

        //Play scream sound when enemy is hit
        m_AudioSource.Play();
    }
}
