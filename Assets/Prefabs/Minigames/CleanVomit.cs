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
    private List<GameObject> m_generatedVomits = new List<GameObject>();
    [SerializeField] private int m_numberOfVomits = 3;
    private int m_cleanedVomits = 0;

    private void Awake()
    {

        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        m_inputs.Minigame1.Select.performed += RaycastFromMouse;
        m_inputs.Minigame1.Select.canceled += ResetDragableObject;

    }

    public override void Start()
    {
        GenerateVomit();
        base.Start();
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
        TriggerGameEnd();
        m_inputs.Minigame1.Select.started -= RaycastFromMouse;
        m_inputs.Minigame1.Select.canceled -= ResetDragableObject;
        m_inputs.Minigame1.Disable();
    }

    private void GenerateVomit()
    {
        m_selectedSpawnPoints.Clear();
        while (m_selectedSpawnPoints.Count < m_numberOfVomits)
        {
            int random = UnityEngine.Random.Range(0, m_vomitSpawnPoints.Count);
            if (!m_selectedSpawnPoints.Contains(m_vomitSpawnPoints[random]))
            {
                m_selectedSpawnPoints.Add(m_vomitSpawnPoints[random]);
            }
        }

        foreach(GameObject vomitSpawn in m_selectedSpawnPoints)
        {
            var vomit = Instantiate(m_vomitAsset);
            vomit.transform.parent = vomitSpawn.transform;
            vomit.transform.position = vomitSpawn.transform.position;
            VomitPuddle vomitScript = vomit.GetComponent<VomitPuddle>();
            vomitScript.OnCleaned += UpdateCleanedVomits;
        }
    }

    private void UpdateCleanedVomits()
    {
        m_cleanedVomits++;
        if (m_cleanedVomits == m_numberOfVomits)
        {
            win = true;
            TriggerGameEnd();
        }
    }
}
