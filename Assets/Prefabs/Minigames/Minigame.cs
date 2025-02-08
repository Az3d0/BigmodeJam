using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Minigame : MonoBehaviour
{
    protected InputSystem_Actions m_inputs;
    public event Action<bool> OnGameEnded;
    [HideInInspector] public float MinigameLenth;

    protected AudioSource m_minigameMusicAudioSource;

    [SerializeField] protected MinigameCountdown m_minigameCountdown;
    [SerializeField] private GameObject m_title;
    [SerializeField] private AudioClip m_minigameMusicAudioClip;

    //update this in child classes depending on specific win requirements
    [HideInInspector] public bool win = false;

    protected virtual void Awake()
    {
        m_minigameMusicAudioSource = GetComponent<AudioSource>();
        if (m_minigameMusicAudioClip) m_minigameMusicAudioSource.clip = m_minigameMusicAudioClip;
        else Debug.LogWarning("No minigame music audioclip assigned");
    }
    protected virtual void Start()
    {
        if (m_title != null)
        {
            m_minigameCountdown.UpdateTimerText(MinigameLenth.ToString());
            PlayTitleAnimationAndStartCountdown();
        }
        else
        {
            m_minigameCountdown.StartCountdown(MinigameLenth);
        }

        m_minigameMusicAudioSource.Play();
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
