using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public static Countdown Instance;

    [HideInInspector] public MinigameOpener m_currentlyOpenMinigame;

    [SerializeField] private GameObject m_countdownBackgroundUI;
    [SerializeField] private GameObject m_countdownFillUI;
    [SerializeField] private GameObject m_countdownTextUI;

    private Stopwatch m_stopwatch;
    private float m_timerLengthinMilSec;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        m_countdownBackgroundUI.SetActive(false);
        m_countdownFillUI.SetActive(false);
        m_countdownTextUI.SetActive(false);

        m_stopwatch = new Stopwatch();
        m_stopwatch.Stop();
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

    private void StopCountdown()
    {
        m_countdownBackgroundUI.SetActive(false);
        m_countdownFillUI.SetActive(false);
        m_countdownTextUI.SetActive(false);

        m_stopwatch.Stop();
    }
    private void FixedUpdate()
    {
        if (!m_stopwatch.IsRunning) return;

        m_countdownFillUI.transform.localScale = new Vector3(1f  - m_stopwatch.ElapsedMilliseconds / (1000f * m_timerLengthinMilSec / 1000f) , 1, 1);

        m_countdownTextUI.GetComponent<TextMeshProUGUI>().text = (m_timerLengthinMilSec/1000 - m_stopwatch.ElapsedMilliseconds / 1000).ToString();

        if (m_stopwatch.ElapsedMilliseconds >= m_timerLengthinMilSec)
        {
            StopCountdown();
            m_currentlyOpenMinigame.CloseMinigame();
        }
    }
}
