using System;
using System.Collections.Generic;
using UnityEngine;

public class MoneyLaunder : MouseMinigame
{
    [Header("washingmachine")]
    [SerializeField] private List<GameObject> m_washingMachineSpawnPoints = new List<GameObject>();
    [SerializeField] private GameObject m_washingMachineAsset;

    private int m_numberOfWashingMachines;
    private int m_numberOfLoadedWashes = 0;
    private int m_numberofFinisehdWashes = 0;
    [Header("money")]
    [SerializeField] private GameObject m_moneySpawnPoint;
    [SerializeField] private GameObject m_smallPileOfMoneyAsset;
    [SerializeField] private GameObject m_largePileOfMoneyAsset;

    protected override void Start()
    {
        base.Start();

        GenerateWashingMachines();
        GenerateMoney();
    }
    private void GenerateWashingMachines()
    {
        //m_numberOfWashingMachines = UnityEngine.Random.Range(1, m_washingMachineSpawnPoints.Count + 1);

        //temporarily set to 1
        m_numberOfWashingMachines = 1;
        for (int i = 0; i < m_numberOfWashingMachines; i++)
        {
            GameObject washingMachineGO = Instantiate(m_washingMachineAsset);
            washingMachineGO.transform.position = m_washingMachineSpawnPoints[i].transform.position;
            washingMachineGO.transform.parent = m_washingMachineSpawnPoints[i].transform;

            if (washingMachineGO.TryGetComponent(out WashingMachine washingMachine))
            {
                washingMachine.OnWashCompleted += UpdateNumberOfLoadedWashes;
                washingMachine.OnWashAnimationDone += UpdateNumberOfFinishedWashes;

            }
        }
    }

    private void UpdateNumberOfFinishedWashes()
    {
        m_numberofFinisehdWashes++;
        Debug.Log($"finisehedwashes: {m_numberofFinisehdWashes}, numofmachines: {m_numberOfWashingMachines}");
        if (m_numberofFinisehdWashes == m_numberOfWashingMachines)
        {
            win = true;
            TriggerGameEnd();
        }
    }

    private void UpdateNumberOfLoadedWashes()
    {
        m_numberOfLoadedWashes++;
        if (m_numberOfLoadedWashes == m_numberOfWashingMachines)
        {
            m_minigameCountdown.StopCountdown();
        }
    }

    private void GenerateMoney()
    {
        if(m_numberOfWashingMachines == 1)
        {
            Instantiate(m_smallPileOfMoneyAsset, m_moneySpawnPoint.transform);
        }
        else if(m_numberOfWashingMachines == 2)
        {
            Instantiate(m_largePileOfMoneyAsset, m_moneySpawnPoint.transform);
        }
    }
}
