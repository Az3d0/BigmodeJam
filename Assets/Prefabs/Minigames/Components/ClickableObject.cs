using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class ClickableObject : MonoBehaviour
{
    protected Rigidbody2D m_rigidBody;
    protected SpriteRenderer m_spriteRenderer;
    protected AudioSource m_audioSource;


    [Header("Optional OnClicked Effects")]
    [Space(15)]

    [Tooltip("Play random AudioClip from list OnClicked")]
    [SerializeField] private List<AudioClip> m_audioClips = new List<AudioClip>();

    [Tooltip("Change to this sprite OnClicked")]
    [SerializeField] protected Sprite m_onClickedSprite;

    protected virtual void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_audioSource = GetComponent<AudioSource>();
    }
    public virtual void OnClicked()
    {
        PlayRandomSFX(m_audioClips);
        SwapSprites();
    }

    protected void PlayRandomSFX(List<AudioClip> audioClips)
    {
        if (audioClips.Count > 0)
        {
            int random = Random.Range(0, audioClips.Count);
            m_audioSource.clip = audioClips[random];
            m_audioSource.Play();
        }
    }

    private void SwapSprites()
    {
        if(m_onClickedSprite != null)
        {
            m_spriteRenderer.sprite = m_onClickedSprite;

        }
    }
}
