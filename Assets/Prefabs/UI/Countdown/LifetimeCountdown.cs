using UnityEngine;
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
        if (m_countdownFillUI.TryGetComponent(out Image image))
        {
            if (m_stopwatch.ElapsedMilliseconds < m_timerLengthinMilSec * 0.3)
            {
                m_flashingLight.verticalShift = 0.6f;
                m_flashingLight.amplitude = 0.6f;
                m_flashingLight.speed = 3;
            }
            else if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec * 0.3 && m_stopwatch.ElapsedMilliseconds < m_timerLengthinMilSec * 0.7)
            {
                m_flashingLight.verticalShift = 2;
                m_flashingLight.amplitude = 2;
                image.color = Color.yellow;
                m_flashingLight.speed = 7;
            }
            else if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec * 0.7)
            {
                m_flashingLight.speed = 15;
                image.color = Color.red;
            }
        }
        base.FixedUpdate();
    }
    public override void TimesUp()
    {
        base.TimesUp();
    }
}
