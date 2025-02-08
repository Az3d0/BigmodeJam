using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMinigame : Minigame
{
    protected DragableObject m_draggedObject;

    [Header("Optional Cursor")]
    [SerializeField] protected GameObject m_cursorAsset;
    protected SpriteRenderer m_cursorSpriteRenderer;
    [SerializeField] protected Sprite m_cursorSprite_Normal;
    [SerializeField] protected Sprite m_cursorSprite_Clicked;
    protected GameObject m_cursor;

    [Header("Optional MouseFollowerSFX")]
    [SerializeField] protected AudioSource m_clickSFX;
    protected virtual void Awake()
    {

        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        m_inputs.Minigame1.Select.started += OnCLick;
        m_inputs.Minigame1.Select.canceled += OnRelease;

        CreateMouseFollower();
    }

    protected virtual void FixedUpdate()
    {
        if (m_cursorAsset == null) return;
        Vector3 mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);
        m_cursor.transform.localPosition = mousePosition;
    }
    protected virtual void OnCLick(InputAction.CallbackContext context)
    {
        RaycastFromMouse(context);

        if (m_cursorAsset == null) return;
        m_cursorSpriteRenderer.sprite = m_cursorSprite_Clicked;
        m_clickSFX.Play();
    }

    protected virtual void OnRelease(InputAction.CallbackContext context)
    {
        ReleaseDraggableObject(context);

        if (m_cursorAsset == null) return;
        m_cursorSpriteRenderer.sprite = m_cursorSprite_Normal;
    }
    protected override void OnDestroy()
    {
        m_inputs.Minigame1.Select.started -= OnCLick;
        m_inputs.Minigame1.Select.canceled -= OnRelease;
        m_inputs.Minigame1.Disable();
        base.OnDestroy();
    }
    protected void ReleaseDraggableObject(InputAction.CallbackContext context)
    {
        if (m_draggedObject != null)
        {
            m_draggedObject.OnReleased();
            m_draggedObject = null;
        }
    }

    protected void RaycastFromMouse(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            GameObject hitGO = hit.collider.gameObject;

            if (hitGO.TryGetComponent(out ClickableObject clickableObject))
            {
                clickableObject.OnClicked();
            }
            else if (hitGO.TryGetComponent(out DragableObject dragableObject))
            {
                //ensures that you only grab one item at a time
                if (m_draggedObject != null) return;
                m_draggedObject = dragableObject;
                m_draggedObject.OnGrabbed();

            }
        }
    }

    private void CreateMouseFollower()
    {
        if (m_cursorAsset == null) return;

        m_cursor = Instantiate(m_cursorAsset);
        if (m_cursor.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            m_cursorSpriteRenderer = spriteRenderer;
        }
        else if (m_cursor.transform.GetChild(0).TryGetComponent(out spriteRenderer))
        {
            m_cursorSpriteRenderer = spriteRenderer;
        }
        else
        {
            Debug.Log("No spriteRenderer attached");
        }
        m_cursor.transform.parent = transform;
        m_cursor.transform.position = gameObject.transform.position;
    }
}
