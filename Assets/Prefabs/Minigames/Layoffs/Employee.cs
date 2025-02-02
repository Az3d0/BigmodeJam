using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Employee : ClickableObject
{
    [SerializeField] float m_force = 50;
    private SpriteRenderer m_spriteRenderer;
    [SerializeField] Sprite m_kickedSprite;

    [Header("SFX")]
    [SerializeField] private List<AudioSource> m_sounds = new List<AudioSource>();
    protected override void Awake()
    {
        if(gameObject.TryGetComponent(out SpriteRenderer sprite))
        {
            m_spriteRenderer = sprite;
        }
        base.Awake();
    }
    public override void OnClicked()
    {
        if(m_sounds.Count > 0)
        {
            int random = Random.Range(0, m_sounds.Count);
            m_sounds[random].Play();
        }

        m_spriteRenderer.sprite = m_kickedSprite;
        m_rigidBody.AddForce(new Vector2(m_force, m_force));
        base.OnClicked();
    }

}
