using UnityEngine;
using UnityEngine.UI;

public class LifetimeCountdown : Countdown
{
    [SerializeField] private TaskMarker m_taskMarker;

    public override void Awake()
    {
        base.Awake();
    }
    public override void FixedUpdate()
    {
        if (m_countdownFillUI.TryGetComponent(out Image image))
        {
            if (m_taskMarker == null) return;
            if (m_stopwatch.ElapsedMilliseconds < m_timerLengthinMilSec * 0.3)
            {
                m_taskMarker.verticalShift = 0.6f;
                m_taskMarker.amplitude = 0.06f;
                m_taskMarker.speed = 3;
            }
            else if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec * 0.3 && m_stopwatch.ElapsedMilliseconds < m_timerLengthinMilSec * 0.7)
            {
                m_taskMarker.amplitude = 0.2f;
                image.color = Color.yellow;
                m_taskMarker.speed = 6;
            }
            else if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec * 0.7)
            {
                m_taskMarker.speed = 10;
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
