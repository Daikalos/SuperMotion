using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility
{
    /// <summary>
    /// Called once when switched on
    /// </summary>
    public virtual void Start()
    {

    }

    /// <summary>
    /// Called once when switched off
    /// </summary>
    public virtual void Exit()
    {

    }

    /// <summary>
    /// Called when ability is active
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// Always called even when not active
    /// </summary>
    public virtual void ConstantUpdate()
    {

    }
}
