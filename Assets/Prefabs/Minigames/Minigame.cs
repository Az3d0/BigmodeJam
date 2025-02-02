using DG.Tweening;
using System;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    protected InputSystem_Actions m_inputs;
    public event Action<bool> OnGameEnded;
    [SerializeField] protected MinigameCountdown m_minigameCountdown;
    [HideInInspector] public float MinigameLenth;
    [SerializeField] private GameObject m_title;

    //update this in child classes depending on specific win requirements
    [HideInInspector] public bool win = false;

    public virtual void Start()
    {
        if (m_title != null)
        {
            PlayTitleAnimationAndStartCountdown();
        }
        else
        {
            m_minigameCountdown.StartCountdown(MinigameLenth);
        }
        m_minigameCountdown.OnTimesUp += TriggerGameEnd;
    }

    private void PlayTitleAnimationAndStartCountdown()
    {
        m_title.transform.DOScale(m_title.transform.localScale * 1.1f, 0.7f).OnComplete(() =>
        {
            m_title.transform.DOScale(0f, 0.1f).OnComplete(() =>
            {
                m_minigameCountdown.StartCountdown(MinigameLenth);
            });
        });
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
