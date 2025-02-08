using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class Employee : ClickableObject
{
    [SerializeField] float m_force = 50;
    private SpriteRenderer m_spriteRenderer;
    [SerializeField] Sprite m_kickedSprite;

    private AudioSource m_audioSource;

    [Header("SFX")]
    [SerializeField] private List<AudioClip> m_audioClips = new List<AudioClip>();

    protected override void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_audioSource = GetComponent<AudioSource>();
        base.Awake();
    }
    public override void OnClicked()
    {
        if(m_audioClips.Count > 0)
        {
            int random = Random.Range(0, m_audioClips.Count);
            m_audioSource.clip = m_audioClips[random];
            m_audioSource.Play();
        }

        m_spriteRenderer.sprite = m_kickedSprite;
        m_rigidBody.AddForce(new Vector2(m_force, m_force));
        base.OnClicked();
    }

}
