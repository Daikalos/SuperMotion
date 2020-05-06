using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Target = null;
    [SerializeField]
    private GameObject m_Bullet = null;

    [SerializeField, Tooltip("Delay before enemy fires a shoot at target"), Range(0.0f, 10.0f)]
    private float m_FireDelay = 1.0f;
    [SerializeField, Tooltip("Distance the enemy checks for target"), Range(0.0f, 300.0f)]
    private float m_TargetDistance = 50.0f;
    [SerializeField, Tooltip("Extent of the field the enemy can see in degrees"), Range(0.0f, 360.0f)]
    private float m_FieldOfView = 170.0f;
    [SerializeField, Tooltip("Wait time until enemy returns to normal state after having spotted a target"), Range(0.0f, 15.0f)]
    private float m_LostTargetDelay = 3.0f;
    [SerializeField, Tooltip("Speed the enemy rotates towards target"), Range(0.0f, 15.0f)]
    private float m_Damping = 5.0f;

    private GameObject m_Weapon;
    private GameObject m_FirePoint;

    private IEnumerator m_FireCoroutine;
    private Vector3 m_TargetPosition;
    private Vector3 m_StartPosition;
    private Vector3 m_StartRotation;

    private bool m_CoroutineIsRunning;
    private bool m_TargetSighted;
    private float m_LostTargetTimer;

    void Start()
    {
        m_Weapon = transform.Find("Weapon").gameObject;
        m_FirePoint = transform.Find("Weapon").Find("FirePoint").gameObject;

        m_FireCoroutine = FireAtTarget(m_FireDelay);
        m_TargetPosition = Vector3.zero;
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation.eulerAngles;

        m_CoroutineIsRunning = false;
        m_TargetSighted = false;

        m_LostTargetTimer = 0.0f;
    }

    void Update()
    {
        m_TargetPosition = m_Target.transform.position;
        transform.position = m_StartPosition;

        if (CanSeeTarget())
        {
            m_TargetSighted = true;
            m_LostTargetTimer = m_LostTargetDelay;
        }
        else
        {
            TargetLost();
        }

        TargetSighted();
    }

    private void TargetLost()
    {
        if (m_TargetSighted)
        {
            m_LostTargetTimer -= Time.deltaTime;
        }

        if (m_LostTargetTimer <= 0)
        {
            m_TargetSighted = false;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(m_StartRotation), m_Damping * Time.deltaTime);
            m_Weapon.transform.localRotation = Quaternion.Slerp(m_Weapon.transform.localRotation, Quaternion.identity, m_Damping * Time.deltaTime);
        }
    }

    private void TargetSighted()
    {
        if (m_TargetSighted)
        {
            RotateEnemy();
            RotateWeapon();

            if (!m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = true;
                StartCoroutine(m_FireCoroutine);
            }
        }
        else
        {
            if (m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = false;
                StopCoroutine(m_FireCoroutine);
            }
        }
    }

    private IEnumerator FireAtTarget(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            GameObject bullet = Instantiate(m_Bullet, 
                m_FirePoint.transform.position, 
                Quaternion.LookRotation(m_TargetPosition - m_Weapon.transform.position)) as GameObject;

            Destroy(bullet, 20.0f); //Bullet gets destroyed after 20sec, performance fix
        }
    }

    private void RotateEnemy()
    {
        Vector3 lookPosition = m_TargetPosition - transform.position;

        //Ignore y-coordinate to only rotate by x-axis and z-axis
        lookPosition.y = 0;

        Quaternion rotateEnemy = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateEnemy, m_Damping * Time.deltaTime);
    }

    private void RotateWeapon()
    {
        Vector3 lookPosition = m_TargetPosition - m_Weapon.transform.position;
        Vector3 rotateWeapon = Quaternion.LookRotation(lookPosition).eulerAngles;

        //Lock rotation so only x-coordinate can rotate
        rotateWeapon.z = 0;
        rotateWeapon.y = 0;

        m_Weapon.transform.localEulerAngles = rotateWeapon;
    }

    private bool CanSeeTarget()
    {
        //No walls between target and enemy
        if (!Physics.Linecast(transform.position, m_TargetPosition))
        {
            //In target distance of enemy
            if (Vector3.Distance(transform.position, m_TargetPosition) <= m_TargetDistance)
            {
                //If target is in enemy field of view
                if (Mathf.Abs(Vector3.Angle(transform.forward, m_TargetPosition - transform.position)) < m_FieldOfView / 2.0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
