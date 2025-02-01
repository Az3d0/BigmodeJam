using UnityEngine;

public class MinigameCountdown : Countdown
{
    [HideInInspector] public Task CurrentTask;

    public override void Awake()
    {
        m_countdownBackgroundUI.SetActive(false);
        m_countdownFillUI.SetActive(false);
        m_countdownTextUI.SetActive(false);

        base.Awake();
    }
    public override void OnTimesUp()
    {
        CurrentTask.CloseMinigame();
        base.OnTimesUp();

    }
}
