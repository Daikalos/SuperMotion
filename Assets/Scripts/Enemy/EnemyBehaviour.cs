using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Target = null;
    [SerializeField]
    private GameObject m_Bullet = null;

    [SerializeField, Tooltip("Delay before enemy fires at target"), Range(0.0f, 10.0f)]
    private float m_FireDelay = 0.9f;
    [SerializeField, Tooltip("Distance the enemy checks for target"), Range(0.0f, 300.0f)]
    private float m_TargetDistance = 50.0f;
    [SerializeField, Tooltip("Extent of the field the enemy can see horizontally in degrees"), Range(0.0f, 360.0f)]
    private float m_HorizontalFOV = 140.0f;
    [SerializeField, Tooltip("Extent of the field the enemy can see vertically in degrees"), Range(0.0f, 360.0f)]
    private float m_VerticalFOV = 120.0f;
    [SerializeField, Tooltip("Wait time until enemy returns to normal state after having spotted a target"), Range(0.0f, 15.0f)]
    private float m_LostTargetDelay = 3.0f;
    [SerializeField, Tooltip("Speed the enemy rotates towards target"), Range(0.0f, 15.0f)]
    private float m_Damping = 5.0f;

    private AudioSource m_AudioSource;

    private GameObject m_Weapon;
    private GameObject m_FirePoint;

    private IEnumerator m_FireCoroutine;
    private Vector3 m_TargetPosition;
    private Vector3 m_StartPosition;
    private Vector3 m_StartRotation;

    private bool m_CoroutineIsRunning;
    private bool m_IsTargetSighted;
    private float m_LostTargetTimer;

    void Start()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>();

        m_AudioSource.clip = AudioManager.m_Instance.GetSound("Gun").m_Clip;
        m_AudioSource.spatialBlend = 1.0f;

        m_Weapon = transform.Find("Weapon").gameObject;
        m_FirePoint = transform.Find("Weapon").Find("FirePoint").gameObject;

        m_FireCoroutine = FireAtTarget(m_FireDelay);
        m_TargetPosition = Vector3.zero;
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation.eulerAngles;

        m_CoroutineIsRunning = false;
        m_IsTargetSighted = false;

        m_LostTargetTimer = 0.0f;
    }

    void Update()
    {
        m_TargetPosition = m_Target.transform.position;
        transform.position = m_StartPosition;

        if (CanSeeTarget())
        {
            m_IsTargetSighted = true;
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
        if (m_IsTargetSighted)
        {
            m_LostTargetTimer -= Time.deltaTime;
        }

        if (m_LostTargetTimer <= 0)
        {
            m_IsTargetSighted = false;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(m_StartRotation), m_Damping * Time.deltaTime);
            m_Weapon.transform.localRotation = Quaternion.Slerp(m_Weapon.transform.localRotation, Quaternion.identity, m_Damping * Time.deltaTime);
        }
    }

    private void TargetSighted()
    {
        if (m_IsTargetSighted)
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

            //Play gun sound when bullet is fired
            m_AudioSource.Play();

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
                Vector3 lookVector = (m_TargetPosition - transform.position);

                Vector3 horizontalVector = new Vector3(lookVector.x, 0.0f, lookVector.z);
                Vector3 verticalVector = new Vector3(0.0f, lookVector.y, 0.0f);

                float horizontalAngle = Mathf.Abs(Vector3.Angle(transform.forward, horizontalVector));
                float verticalAngle = Mathf.Abs(Vector3.Angle(lookVector, verticalVector) - 90.0f);

                //If target is in enemy field of view
                if (horizontalAngle < m_HorizontalFOV / 2.0f && verticalAngle < m_VerticalFOV / 2.0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
