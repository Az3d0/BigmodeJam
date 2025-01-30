using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CleanVomit : Minigame
{
    private InputSystem_Actions m_inputs;
    private DragableObject m_draggedObject;

    private void Awake()
    {

        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        m_inputs.Minigame1.Select.performed += RaycastFromMouse;
        m_inputs.Minigame1.Select.canceled += ResetDragableObject;
    }

    private void ResetDragableObject(InputAction.CallbackContext context)
    {
        if(m_draggedObject != null)
        {
            m_draggedObject.SetIsBeingDragged(false);
            m_draggedObject = null;
        }
    }

    private void RaycastFromMouse(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.TryGetComponent(out DragableObject dragableObject) )
            {
                m_draggedObject = dragableObject;
                m_draggedObject.SetIsBeingDragged(context.ReadValueAsButton());
            }
            Debug.Log(hit.collider.gameObject.name);
        } 
    }

    

    protected override void OnDestroy()
    {
        win = true;
        TriggerGameEnd();
        m_inputs.Minigame1.Select.started -= RaycastFromMouse;
        m_inputs.Minigame1.Select.canceled -= ResetDragableObject;
        m_inputs.Minigame1.Disable();
    }
}
