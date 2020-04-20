using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGlass : MonoBehaviour
{
    [SerializeField, Tooltip("Force the glass shatters from impact point"), Range(0.0f, 800.0f)]
    private float m_ShatterForce = 150.0f;
    
    [Tooltip("Object to switch to when destroyed")]
    public GameObject m_DestroyedVersion;

    public void Shatter(Collider playerCollider, Vector3 hitDirection, RaycastHit rayHit)
    {
        GameObject newObject = Instantiate(m_DestroyedVersion, transform.position, transform.rotation) as GameObject;
        newObject.transform.localScale = transform.localScale;

        for (int i = 0; i < newObject.transform.childCount; i++)
        {
            Rigidbody childRigidBody = newObject.transform.GetChild(i).GetComponent<Rigidbody>();

            //Apply a force to each glass shard relative to direction and impact point
            float forceDistance = (1.0f / (childRigidBody.transform.position - rayHit.point).magnitude);
            childRigidBody.AddForce(
                (hitDirection * forceDistance * m_ShatterForce) + 
                (childRigidBody.transform.position - rayHit.point) * forceDistance * m_ShatterForce);

            Physics.IgnoreCollision(childRigidBody.GetComponent<Collider>(), playerCollider);

            //Fixes performance and jitter
            Destroy(childRigidBody.GetComponent<Rigidbody>(), 10.0f);
            Destroy(childRigidBody.GetComponent<Collider>(), 10.0f);
        }

        Destroy(gameObject);
    }
}
