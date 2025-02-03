using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyPlanetsMinigame : Minigame
{
    public int totalNumberOfPlanets;
    public int counter;
    public Sprite explosion;
    public List<AudioClip> m_SFX = new List<AudioClip>();
    public AudioSource audioSource = new AudioSource();

    public void TriggerGameEndParent()
    {
        TriggerGameEnd();

    }
}
