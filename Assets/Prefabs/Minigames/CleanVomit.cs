using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CleanVomit : Minigame
{
    private InputSystem_Actions m_inputs;
    private DragableObject m_draggedObject;
    [SerializeField] private GameObject m_vomitAsset;
    [SerializeField] private List<GameObject> m_vomitSpawnPoints = new List<GameObject>();
    private List<GameObject> m_selectedSpawnPoints = new List<GameObject>();

    private void Awake()
    {

        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        m_inputs.Minigame1.Select.performed += RaycastFromMouse;
        m_inputs.Minigame1.Select.canceled += ResetDragableObject;
        GenerateVomit();
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
        win = true; //HARDCODED FOR TESTING
        TriggerGameEnd();
        m_inputs.Minigame1.Select.started -= RaycastFromMouse;
        m_inputs.Minigame1.Select.canceled -= ResetDragableObject;
        m_inputs.Minigame1.Disable();
    }

    private void GenerateVomit()
    {
        m_selectedSpawnPoints.Clear();
        while (m_selectedSpawnPoints.Count < 3)
        {
            int random = UnityEngine.Random.Range(0, 6);
            if (m_selectedSpawnPoints.Contains(m_vomitSpawnPoints[random])) return;
            m_selectedSpawnPoints.Add(m_vomitSpawnPoints[random]);
        }

        foreach(GameObject vomitSpawn in m_selectedSpawnPoints)
        {
            var vomit = Instantiate(m_vomitAsset);
            vomit.transform.parent = vomitSpawn.transform;
            vomit.transform.position = vomitSpawn.transform.position;
        }
    }
}
