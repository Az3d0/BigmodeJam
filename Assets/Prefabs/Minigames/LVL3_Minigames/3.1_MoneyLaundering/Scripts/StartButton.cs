using UnityEngine;

public class StartButton : ClickableObject
{
    [SerializeField] WashingMachine m_washingMachine;
    public override void OnClicked()
    {
        m_washingMachine.OpenDoor();
        base.OnClicked();
    }
}
