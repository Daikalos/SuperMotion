using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTruck : MonoBehaviour
{
    public float m_speed = 2f;

    public Transform[] wayPointArray;

    int currentWayPoint;
    Transform targetWayPoint;


    // Start is called before the first frame update
    void Start()
    {
        currentWayPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWayPoint < wayPointArray.Length)
        {
            if (targetWayPoint == null)
            {
                targetWayPoint = wayPointArray[currentWayPoint];
            }

            Move();
        }
        //  start at the first one again
        else
        {
            currentWayPoint = 0;
            targetWayPoint = wayPointArray[currentWayPoint];
        }
    }

    void Move()
    {
        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, m_speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, m_speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position)
        {
            currentWayPoint++;
            Debug.Log(currentWayPoint.ToString());

            // osäker på varför jag måste ha det två ggr för att undvika felmeddelande
            if (currentWayPoint >= wayPointArray.Length)
            {
                targetWayPoint = wayPointArray[0];
            }
            else
            {
                targetWayPoint = wayPointArray[currentWayPoint];
            }
        }
    }
}
