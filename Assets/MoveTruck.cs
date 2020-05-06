using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTruck : MonoBehaviour
{
    [SerializeField, Tooltip("The spped of the truck"), Range(1f, 30f)]
    public float m_Speed = 2f;

    [Tooltip("The waypoints in the scene that you want the truck to move between")]
    public Transform[] wayPointArray;

    int m_CurrentWayPoint;
    Transform m_TargetWayPoint;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentWayPoint = 0;
        m_TargetWayPoint = wayPointArray[m_CurrentWayPoint];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentWayPoint < wayPointArray.Length)
        {
            Move();
        }
    }

    void Move()
    {
        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, m_TargetWayPoint.position - transform.position, m_Speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, m_TargetWayPoint.position, m_Speed * Time.deltaTime);

        if (transform.position == m_TargetWayPoint.position)
        {
            m_CurrentWayPoint++;

            // osäker på varför jag måste ha det två ggr för att undvika felmeddelande
            if (m_CurrentWayPoint >= wayPointArray.Length)
            {
                m_CurrentWayPoint = 0;
            }

            m_TargetWayPoint = wayPointArray[m_CurrentWayPoint];
        }
    }
}
