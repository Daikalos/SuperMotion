using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Short script to prevent truck from squeezing player through wall
/// </summary>
public class PreventCrush : MonoBehaviour
{
    private MoveTruck m_MoveTruck;

    private void Start()
    {
        m_MoveTruck = transform.parent.GetComponent<MoveTruck>();
    }

    private void OnTriggerStay(Collider other)
    {
        //If there is a wall behind player stop truck
        if (Physics.Raycast(other.bounds.center, transform.forward, other.bounds.extents.x + (m_MoveTruck.Speed * Time.deltaTime)))
        {
            m_MoveTruck.Speed = 0.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_MoveTruck.Speed = m_MoveTruck.NormalSpeed;
    }
}
