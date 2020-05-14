using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassShatter : MonoBehaviour
{
    [SerializeField, Tooltip("Object to switch to when destroyed")]
    private GameObject m_DestroyedVersion = null;
    [SerializeField, Tooltip("Force the glass shatters from hit direction"), Range(0.0f, 800.0f)]
    private float m_HitForce = 150.0f;
    [SerializeField, Tooltip("Force the glass shatters from impact point"), Range(0.0f, 800.0f)]
    private float m_ImpactForce = 150.0f;

    public void Shatter(Vector3 hitDirection, Vector3 impactPoint)
    {
        GameObject newObject = Instantiate(m_DestroyedVersion, transform.position, transform.rotation) as GameObject;
        newObject.transform.localScale = transform.localScale;

        AudioClip clip = AudioManager.m_Instance.GetSound("BrokenGlass").m_Clip;
        AudioSource.PlayClipAtPoint(clip, impactPoint, 1.5f);

        //Ignore collision between player and glass shards
        Collider playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        for (int i = 0; i < newObject.transform.childCount; i++)
        {
            Rigidbody childRigidBody = newObject.transform.GetChild(i).GetComponent<Rigidbody>();
            Collider childCollider = newObject.transform.GetChild(i).GetComponent<Collider>();

            //Apply a force to each glass shard relative to hit direction and impact point
            float forceDistance = (1.0f / (childRigidBody.transform.position - impactPoint).magnitude);
            childRigidBody.AddForce(
                (hitDirection * forceDistance * m_HitForce) + 
                (childRigidBody.transform.position - impactPoint) * forceDistance * m_ImpactForce);

            Physics.IgnoreCollision(childCollider, playerCollider);

            //Fixes performance and jitter
            Destroy(childRigidBody, 10.0f);
            Destroy(childCollider, 10.0f);
        }

        Destroy(gameObject);
    }
}
