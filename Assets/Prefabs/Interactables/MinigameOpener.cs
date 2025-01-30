using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class MinigameOpener : MonoBehaviour
{
    [SerializeField] private GameObject m_minigamePrefab;
    [SerializeField] private float minigameLength = 10;
    public AudioSource minigameMusic;
    public AudioSource mainMusic;

    private GameObject m_minigame;

    public static event Action<bool> OnMinigameEnd;
    public void OpenMinigame()
    {
        //Stop main music and play minigame music
        if (mainMusic != null) 
        { 
            mainMusic.Stop();
        } else
        {
            Debug.Log("Main music audio source not assigned to Minigame Opener!");
        }
        if (minigameMusic != null)
        {
            Debug.Log("Minigame music audio source not assigned to Minigame Opener!");
            minigameMusic.Play();
        }

        m_minigame = Instantiate(m_minigamePrefab);
        m_minigame.SetActive(true);

        Countdown.Instance.StartCountdown(minigameLength);
        Countdown.Instance.m_currentlyOpenMinigame = this;

        PlayerControls.Instance.DisablePlayerMovement();
    }

    public void CloseMinigame()
    {
        Destroy(m_minigame);
        PlayerControls.Instance.EnablePlayerMovement();

        //Resume main music
        if (minigameMusic != null)
        {
            minigameMusic.Stop();
        }
        if (mainMusic != null)
        {
            mainMusic.Play();
        }
    }
}
