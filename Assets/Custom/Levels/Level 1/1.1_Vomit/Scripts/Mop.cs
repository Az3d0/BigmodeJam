using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Mop : DragableObject
{
    [SerializeField] private List<AudioSource> m_SFX = new List<AudioSource>();

    protected override void Awake()
    {

        base.Awake();
    }

    public override void OnGrabbed()
    {
        int random = Random.Range(0, m_SFX.Count);
        m_SFX[random].Play();
        base.OnGrabbed();
    }

    public override void OnReleased()
    {
        int random = Random.Range(0, m_SFX.Count);
        m_SFX[random].Play();
        base.OnReleased();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
