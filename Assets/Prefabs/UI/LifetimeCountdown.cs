using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LifetimeCountdown : Countdown
{
    [SerializeField] private FlashingLight m_flashingLight;
    public override void Awake()
    {
        base.Awake();
    }
    public override void FixedUpdate()
    {
        if(m_countdownFillUI.TryGetComponent(out Image image))
        {
            if(m_stopwatch.ElapsedMilliseconds < m_timerLengthinMilSec * 0.3)
            {
                m_flashingLight.verticalShift = 1;
                m_flashingLight.amplitude = 1;
            }
            else if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec * 0.3 && m_stopwatch.ElapsedMilliseconds < m_timerLengthinMilSec * 0.7)
            {
                m_flashingLight.verticalShift = 2;
                m_flashingLight.amplitude = 2;
                image.color = Color.yellow;
                m_flashingLight.speed = 7;
            }
            else if(m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec * 0.7)
            {
                m_flashingLight.speed = 15;
                image.color = Color.red;
            }
        }
        base.FixedUpdate();
    }
    public override void OnTimesUp()
    {
        base.OnTimesUp();
    }
}
