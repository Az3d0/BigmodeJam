using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WashingMachine : MonoBehaviour
{

    [SerializeField] private Sprite m_machineOpenSprite;
    [SerializeField] private Sprite m_machineClosedSprite;
    [SerializeField] private Sprite m_machineWashingSprite;
    [SerializeField] private Sprite m_machineReadySprite;

    [SerializeField] private AudioSource m_doorSlam;
    [SerializeField] private AudioSource m_beep;
    [SerializeField] private AudioSource m_Spin;
    private Stopwatch m_stopwatch;
    private SpriteRenderer m_machineSpriteRenderer;
    private bool m_isOpen;
    private bool m_isOn;

    private int m_amountOfMoney = 0;
    private int m_moneyCapacity = 3;

    public event Action OnWashCompleted;
    public event Action OnWashAnimationDone;
    private void Awake()
    {
        m_stopwatch = new Stopwatch();
        m_stopwatch.Stop();
        m_machineSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void AddMoney()
    {
        m_amountOfMoney++;
        if (m_amountOfMoney == m_moneyCapacity)
        {
            StartWash(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Money money) && m_isOpen)
        {
            AddMoney();
            Destroy(collision.gameObject);
            //money.OnAboveWashingMachine(this, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Money money))
        {
            //money.OnAboveWashingMachine(this, false);
        }
    }

    private void StartWash()
    {
        m_machineSpriteRenderer.sprite = m_machineWashingSprite;
        m_isOn = true;
        StartAnimation();
        OnWashCompleted?.Invoke();
    }

    private void StartAnimation()
    {
        m_Spin.Play();
        //add animation here 
        m_stopwatch.Start();
    }

    private void FixedUpdate()
    {

        if (!m_stopwatch.IsRunning) return;
        if (m_stopwatch.ElapsedMilliseconds >= 1500)
        {
            m_machineSpriteRenderer.sprite = m_machineReadySprite;
        }
        if (m_stopwatch.ElapsedMilliseconds >= 2000)
        {
            m_stopwatch.Stop();
            OnWashAnimationDone?.Invoke();
        }
    }

    public void OpenDoor()
    {
        if (m_isOn) return;
        m_doorSlam.Play();
        m_isOpen = !m_isOpen;
        if(m_isOpen)
        {
            m_machineSpriteRenderer.sprite = m_machineOpenSprite;
        }
        else
        {
            m_machineSpriteRenderer.sprite = m_machineClosedSprite;
        }

    }
}
