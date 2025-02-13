using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class Employee : ClickableObject
{
    [Header("Employee specific values")]
    [Space(15)]

    [SerializeField] float m_force = 50;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void OnClicked()
    {
        m_rigidBody.AddForce(new Vector2(m_force, m_force));
        base.OnClicked();
    }

}
