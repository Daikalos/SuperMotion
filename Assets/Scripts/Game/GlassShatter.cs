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

    private List<Transform> m_Children;

    private void Start()
    {
        m_Children = new List<Transform>();
    }

    public void Shatter(Vector3 hitDirection, Vector3 impactPoint)
    {
        GameObject newObject = Instantiate(m_DestroyedVersion, transform.position, transform.rotation) as GameObject;
        newObject.transform.localScale = transform.localScale;

        AudioClip clip = AudioManager.m_Instance.GetSound("BrokenGlass").m_Clip;
        AudioSource.PlayClipAtPoint(clip, impactPoint, 1.0f);

        //Ignore collision between player and glass shards
        Collider playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        for (int i = 0; i < newObject.transform.childCount; i++)
        {
            //Add each child in a list
            m_Children.Add(newObject.transform.GetChild(i));
        }

        for (int i = 0; i < m_Children.Count; i++)
        {
            //De-parent each shard to prevent non uniform scaling
            m_Children[i].parent = null;
        }

        newObject.transform.localScale = Vector3.one;

        for (int i = 0; i < m_Children.Count; i++)
        {
            m_Children[i].parent = newObject.transform;
            m_Children[i].localScale = transform.localScale;

            Rigidbody childRigidbody = m_Children[i].GetComponent<Rigidbody>();
            Collider childCollider = m_Children[i].GetComponent<Collider>();

            //Apply a force to each glass shard relative to hit direction and impact point
            float forceDistance = (1.0f / (childRigidbody.transform.position - impactPoint).magnitude);
            childRigidbody.AddForce(
                (hitDirection * forceDistance * m_HitForce) +
                (childRigidbody.transform.position - impactPoint) * forceDistance * m_ImpactForce);

            Physics.IgnoreCollision(childCollider, playerCollider);

            //Fixes performance and jitter
            Destroy(childRigidbody, 10.0f);
            Destroy(childCollider, 10.0f);
        }

        Destroy(gameObject);
    }
}
