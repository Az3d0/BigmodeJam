using UnityEngine;

public class Employee : ClickableObject
{
    [SerializeField] float m_force = 50;
    private SpriteRenderer m_spriteRenderer;
    [SerializeField] Sprite m_kickedSprite;

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
        m_spriteRenderer.sprite = m_kickedSprite;
        m_rigidBody.AddForce(new Vector2(m_force, m_force));
        base.OnClicked();
    }

}
