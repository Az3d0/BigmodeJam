using System;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    protected InputSystem_Actions m_inputs;
    public event Action<bool> OnGameEnded;
    [SerializeField] private MinigameCountdown m_minigameCountdown;
    [HideInInspector] public float MinigameLenth;

    //update this in child classes depending on specific win requirements
    [HideInInspector] public bool win = false;

    public virtual void Start()
    {
        m_minigameCountdown.StartCountdown(MinigameLenth);
        m_minigameCountdown.OnTimesUp += TriggerGameEnd;
    }
    protected void TriggerGameEnd()
    {
        Debug.Log($"{gameObject.name} - Minigame won? {win}");

        OnGameEnded?.Invoke(win);
    }

    protected virtual void OnDestroy()
    {
        m_minigameCountdown.OnTimesUp -= TriggerGameEnd;
    }

}
