using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Cursor : MonoBehaviour
{
    protected AudioSource m_audioSource;

    protected SpriteRenderer m_cursorSpriteRenderer;
    [SerializeField] protected Sprite m_cursorSprite_Normal;
    [SerializeField] protected Sprite m_cursorSprite_Clicked;

    [SerializeField] protected AudioClip m_clickSFX;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        if (m_clickSFX != null) m_audioSource.clip = m_clickSFX;

        if(gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Debug.LogWarning("SpriteRenderer detected on cursor object. Please ensure cursor has a child with a SpriteRenderer instead. This is to ensure that the cursor can be alligned with the actual cursor");
        }
        if (gameObject.transform.GetChild(0).TryGetComponent(out spriteRenderer))
        {
            m_cursorSpriteRenderer = spriteRenderer;
        }
    }

    protected virtual void FixedUpdate()
    {
        //rewrite this from input to other way of tracking
        Vector3 mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);
        transform.localPosition = mousePosition;
    }

    public void ClickCursor()
    {
        if (m_cursorSprite_Clicked != null) m_cursorSpriteRenderer.sprite = m_cursorSprite_Clicked;

        if (m_clickSFX != null) m_audioSource.Play();
    }
    public void ReleaseCursor()
    {
        if (m_cursorSprite_Normal != null) m_cursorSpriteRenderer.sprite = m_cursorSprite_Normal;
    }
}
