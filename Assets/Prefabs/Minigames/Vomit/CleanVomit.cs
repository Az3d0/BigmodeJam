using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CleanVomit : PointNClickMinigame
{
    [SerializeField] private GameObject m_vomitAsset;
    [SerializeField] private List<GameObject> m_vomitSpawnPoints = new List<GameObject>();
    private List<GameObject> m_selectedSpawnPoints = new List<GameObject>();
    private List<GameObject> m_generatedVomits = new List<GameObject>();
    [SerializeField] private int m_numberOfVomits = 3;
    private int m_cleanedVomits = 0;


    protected override void Awake()
    {
        OnObjectHit += SetDragableObject;
        base.Awake();
    }

    protected override void OnDestroy()
    {
        OnObjectHit -= SetDragableObject;
        base.OnDestroy();
    }
    public override void Start()
    {
        GenerateVomit();
        base.Start();
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
