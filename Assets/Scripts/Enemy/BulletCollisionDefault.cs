using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionDefault : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Bullet" && other.tag != "Enemy" && other.tag != "Player")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
