using System.Diagnostics;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Countdown : MonoBehaviour
{

    [SerializeField] protected GameObject m_countdownBackgroundUI;
    [SerializeField] protected GameObject m_countdownFillUI;
    [SerializeField] protected GameObject m_countdownTextUI;

    protected Stopwatch m_stopwatch;
    protected float m_timerLengthinMilSec;

    public virtual void Awake()
    {
        m_stopwatch = new Stopwatch();
        m_stopwatch.Stop();
    }
    private void OnEnable()
    {
        PlayerControls.OnPause += PauseCountdown;
        PlayerControls.OnUnpause += ResumeCountdown;
    }
    private void OnDisable()
    {
        PlayerControls.OnPause -= PauseCountdown;
        PlayerControls.OnUnpause -= ResumeCountdown;

    }
    public void StartCountdown(float timerLengthInSec)
    {
        UnityEngine.Debug.Log("start");
        m_countdownBackgroundUI.SetActive(true);
        m_countdownFillUI.SetActive(true);
        m_countdownTextUI.SetActive(true);

        m_timerLengthinMilSec = timerLengthInSec * 1000;
        
        m_stopwatch.Reset();
        m_stopwatch.Start();
    }

    public void PauseCountdown()
    {
        m_stopwatch.Stop();
    }

    public void ResumeCountdown()
    {
        m_stopwatch.Start();
    }

    public void StopCountdown()
    {
        m_countdownBackgroundUI.SetActive(false);
        m_countdownFillUI.SetActive(false);
        m_countdownTextUI.SetActive(false);

        m_stopwatch.Stop();
    }
    public virtual void FixedUpdate()
    {
        if (!m_stopwatch.IsRunning) return;

        m_countdownFillUI.transform.localScale = new Vector3(1f  - m_stopwatch.ElapsedMilliseconds / (1000f * m_timerLengthinMilSec / 1000f) , 1, 1);

        m_countdownTextUI.GetComponent<TextMeshProUGUI>().text = (m_timerLengthinMilSec/1000 - m_stopwatch.ElapsedMilliseconds / 1000).ToString();

        if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec)
        {
            OnTimesUp();
        }
    }

    public virtual void OnTimesUp()
    {
        StopCountdown();
    }
}
