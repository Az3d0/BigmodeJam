using System;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public event Action<bool> OnGameEnded;
    [SerializeField] private MinigameCountdown m_minigameCountdown;
    [HideInInspector] public float MinigameLenth;

    //update this in child classes depending on specific win requirements
    public bool win = false;

    private void Start()
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
