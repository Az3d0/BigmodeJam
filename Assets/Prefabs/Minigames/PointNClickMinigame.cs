using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointNClickMinigame : Minigame
{
    protected DragableObject m_draggedObject;
    protected event Action<GameObject> OnObjectHit;


    protected virtual void Awake()
    {

        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        m_inputs.Minigame1.Select.performed += OnCLick;
        m_inputs.Minigame1.Select.canceled += OnRelease;

    }

    protected virtual void OnCLick(InputAction.CallbackContext context)
    {
        RaycastFromMouse(context);
    }

    protected virtual void OnRelease(InputAction.CallbackContext context)
    {
        ResetDragableObject(context);
    }
    protected override void OnDestroy()
    {
        TriggerGameEnd();
        m_inputs.Minigame1.Select.started -= OnCLick;
        m_inputs.Minigame1.Select.canceled -= OnRelease;
        m_inputs.Minigame1.Disable();
        base.OnDestroy();
    }
    protected void ResetDragableObject(InputAction.CallbackContext context)
    {
        if (m_draggedObject != null)
        {
            m_draggedObject.SetIsBeingDragged(false);
            m_draggedObject = null;
        }
    }

    protected void RaycastFromMouse(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            OnObjectHit?.Invoke(hit.collider.gameObject);
        }
    }

    protected virtual void SetClickObject(GameObject hitGO)
    {
        if (hitGO.TryGetComponent(out ClickableObject clickableObject))
        {
            clickableObject.OnClicked();
        }
    }
    protected virtual void SetDragableObject(GameObject hitGO)
    {
        if (hitGO.TryGetComponent(out DragableObject dragableObject))
        {
            m_draggedObject = dragableObject;
            m_draggedObject.SetIsBeingDragged(true);
        }
    }
}
