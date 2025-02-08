using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LayoffMinigame : MouseMinigame
{
    [Header("Layoff Stuff")]
    [SerializeField] private GameObject m_employeeAsset;
    [SerializeField] private List<GameObject> m_employeeSpots = new List<GameObject>();



    private int m_employeeNum;
    private int m_firedEmployeeNum;

    public override void Start()
    {
        GenerateEmployees();
        base.Start();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void GenerateEmployees()
    {
        m_employeeNum = Random.Range(3, m_employeeSpots.Count);

        for (int i = 0; i < m_employeeNum; i++)
        {
            Instantiate(m_employeeAsset, m_employeeSpots[i].transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Employee employee))
        {
            m_firedEmployeeNum++;
            if(m_firedEmployeeNum == m_employeeNum)
            {
                win = true;
                TriggerGameEnd();
            }
        }
    }
}
