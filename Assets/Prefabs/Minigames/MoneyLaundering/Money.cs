using UnityEngine;

public class Money : DragableObject
{

    private bool m_isWashable = false;
    private WashingMachine m_washingMachine;
    public override void OnReleased()
    {
        if (m_isWashable && m_isBeingDragged)
        {
            m_washingMachine.AddMoney();
            //add to washedmoney
            Destroy(gameObject);
        }
        base.OnReleased();
    }

    public void OnAboveWashingMachine(WashingMachine washingMachine, bool state)
    {
        m_washingMachine = washingMachine;
        m_isWashable = state;
    }
}
