using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Conspiracy : PointNClickMinigame
{
    [SerializeField] private List<Neighbourhood> m_neighbourhoods = new List<Neighbourhood>();

    int m_convertedNeighbourHoods = 0;
    protected override void Awake()
    {
        base.Awake();

        foreach (Neighbourhood neighbourhood in m_neighbourhoods)
        {
            neighbourhood.OnLiesBelieved += UpdateConvertedNeighbourhoods;
        }
    }

    private void UpdateConvertedNeighbourhoods()
    {
        m_convertedNeighbourHoods++;
        if(m_convertedNeighbourHoods == 3)
        {
            win = true;
            TriggerGameEnd();
        }
    }
}
