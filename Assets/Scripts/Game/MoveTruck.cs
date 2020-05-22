using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTruck : MonoBehaviour
{
    [SerializeField, Tooltip("The speed of the truck"), Range(1f, 30f)]
    float m_Speed = 2f;

    [Tooltip("The waypoints in the scene that you want the truck to move between")]
    public Transform[] m_wayPointArray;

    int m_CurrentWayPoint;
    Transform m_TargetWayPoint;

    public float Speed { get => m_Speed; set => m_Speed = value; }
    public float NormalSpeed { get; private set; }

    void Start()
    {
        m_CurrentWayPoint = 0;
        m_TargetWayPoint = m_wayPointArray[m_CurrentWayPoint];

        NormalSpeed = Speed;
    }

    void Update()
    {
        if (m_CurrentWayPoint < m_wayPointArray.Length)
        {
            Move();
        }
    }

    void Move()
    {
        // rotate towards the target
        transform.rotation = Quaternion.LookRotation(m_TargetWayPoint.position - transform.position);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, m_TargetWayPoint.position, m_Speed * Time.deltaTime);

        if (transform.position == m_TargetWayPoint.position)
        {
            m_CurrentWayPoint++;

            // osäker på varför jag måste ha det två ggr för att undvika felmeddelande
            if (m_CurrentWayPoint >= m_wayPointArray.Length)
            {
                m_CurrentWayPoint = 0;
            }

            m_TargetWayPoint = m_wayPointArray[m_CurrentWayPoint];
        }
    }
}
