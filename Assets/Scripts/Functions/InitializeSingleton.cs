using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSingleton<T> : MonoBehaviour where T : Component
{
    private static T m_Instance = null;

    //When this object is referenced to
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>();
                if (m_Instance == null)
                {
                    //Create new object if it does not exist in scene
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    m_Instance = obj.AddComponent<T>();
                }
            }
            return m_Instance;
        }
    }
}
