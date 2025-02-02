using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointNClickMinigame : Minigame
{
    protected DragableObject m_draggedObject;

    [Header("Optional MouseFollower")]
    [SerializeField] protected GameObject m_mouseFollowerAsset;
    protected SpriteRenderer m_mouseFollowerSpriteRenderer;
    [SerializeField] protected Sprite m_mouseFollowerSprite_Normal;
    [SerializeField] protected Sprite m_mouseFollowerSprite_Clicked;
    protected GameObject m_mouseFollower;

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
        if (m_mouseFollowerAsset == null) return;
        Vector3 mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);
        m_mouseFollower.transform.localPosition = mousePosition;
    }
    protected virtual void OnCLick(InputAction.CallbackContext context)
    {
        RaycastFromMouse(context);

        if (m_mouseFollowerAsset == null) return;
        m_mouseFollowerSpriteRenderer.sprite = m_mouseFollowerSprite_Clicked;
        m_clickSFX.Play();
    }

    protected virtual void OnRelease(InputAction.CallbackContext context)
    {
        ReleaseDraggableObject(context);

        if (m_mouseFollowerAsset == null) return;
        m_mouseFollowerSpriteRenderer.sprite = m_mouseFollowerSprite_Normal;
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
        if (m_mouseFollowerAsset == null) return;

        m_mouseFollower = Instantiate(m_mouseFollowerAsset);
        if (m_mouseFollower.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            m_mouseFollowerSpriteRenderer = spriteRenderer;
        }
        else if (m_mouseFollower.transform.GetChild(0).TryGetComponent(out spriteRenderer))
        {
            m_mouseFollowerSpriteRenderer = spriteRenderer;
        }
        else
        {
            Debug.Log("No spriteRenderer attached");
        }
        m_mouseFollower.transform.parent = transform;
        m_mouseFollower.transform.position = gameObject.transform.position;
    }
}
