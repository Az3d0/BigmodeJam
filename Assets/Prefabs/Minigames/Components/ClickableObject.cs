using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClickableObject : MonoBehaviour
{
    protected Rigidbody2D m_rigidBody;
    protected virtual void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }
    public virtual void OnClicked()
    {
    }
}
