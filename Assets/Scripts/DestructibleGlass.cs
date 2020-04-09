using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGlass : MonoBehaviour
{
    [SerializeField, Tooltip("Force the glass shatters from impact point"), Range(0.0f, 800.0f)]
    private float m_ShatterForce = 300.0f;

    public GameObject m_DestroyedVersion;

    public void Shatter(Vector3 hitDirection, RaycastHit rayHit)
    {
        GameObject newObject = Instantiate(m_DestroyedVersion, transform.position, transform.rotation) as GameObject;
        newObject.transform.localScale = transform.localScale;

        for (int i = 0; i < newObject.transform.childCount; i++)
        {
            Rigidbody childRigidBody = newObject.transform.GetChild(i).GetComponent<Rigidbody>();

            childRigidBody.AddForce(
                (hitDirection * m_ShatterForce) + 
                (childRigidBody.transform.position - rayHit.point) * (1.0f / (childRigidBody.transform.position - rayHit.point).magnitude) * m_ShatterForce);

            Destroy(newObject.transform.GetChild(i).GetComponent<Rigidbody>(), 4.0f);
            Destroy(newObject.transform.GetChild(i).GetComponent<Collider>(), 4.0f);
        }

        Destroy(gameObject);
    }
}
