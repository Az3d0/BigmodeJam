using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Mop : DragableObject
{
    [SerializeField] private List<AudioSource> m_SFX = new List<AudioSource>();
    Stopwatch m_stopwatch;

    protected override void Awake()
    {
        m_stopwatch = new Stopwatch();
        m_stopwatch.Start();
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
        if (!m_isBeingDragged) { return; }

        // this is not doing anything at the moment
        if(m_stopwatch.ElapsedMilliseconds % 100 == 5)
        {
            int random = Random.Range(0, m_SFX.Count);
            m_SFX[random].Play();
        }
        base.FixedUpdate();
    }
}
