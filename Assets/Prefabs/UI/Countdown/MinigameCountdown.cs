using TMPro;

public class MinigameCountdown : Countdown
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void TimesUp()
    {
        base.TimesUp();

    }

    public void UpdateTimerText(string text)
    {
        m_countdownTextUI.GetComponent<TextMeshProUGUI>().text = text;
    }
}
