using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMinigame : Minigame
{
    protected DragableObject m_draggedObject;
    [Header("Optional Cursor")]
    [Space(15)]

    [SerializeField] protected GameObject m_cursorAsset;
    private GameObject m_cursorGO;
    private Cursor m_cursor;

    protected override void Awake()
    {
        base.Awake();
        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        m_inputs.Minigame1.Select.started += OnCLick;
        m_inputs.Minigame1.Select.canceled += OnRelease;

        TryGenerateCursor();
    }

    protected virtual void OnCLick(InputAction.CallbackContext context)
    {
        RaycastFromMouse(context);

        if(m_cursor != null) m_cursor.ClickCursor();
    }

    protected virtual void OnRelease(InputAction.CallbackContext context)
    {
        ReleaseDraggableObject(context);

        if (m_cursor != null) m_cursor.ReleaseCursor();
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

    private void TryGenerateCursor()
    {
        if (m_cursorAsset == null) return;

        m_cursorGO = Instantiate(m_cursorAsset);
        if (m_cursorGO.TryGetComponent(out  Cursor cursor))
        {
            m_cursor = cursor;
        }

        m_cursorGO.transform.parent = transform;
        m_cursorGO.transform.position = gameObject.transform.position;
    }
}
