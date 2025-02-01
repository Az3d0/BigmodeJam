using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class Task : MonoBehaviour
{
    //Task Fields
    [Header("Task")] 
    [SerializeField] private LifetimeCountdown m_LifetimeCountdown;
    [SerializeField] private float m_lifetimeLength;

    //Minigame Fields
    [Header("Minigame")]
    [SerializeField] private GameObject m_minigamePrefab;
    public float MinigameLength = 5;
    public AudioSource minigameMusic;
    [SerializeField] private GameObject m_minigameCountdown;
    private GameObject m_minigame;
    public static event Action<bool> OnMinigameEnd;

    private void Start()
    {
        m_LifetimeCountdown.StartCountdown(m_lifetimeLength);
    }
    public void OpenMinigame()
    {
        //Stop main music and play minigame music
        if (MainMusic.Instance.AudioSourceComponent != null) 
        {
            MainMusic.Instance.AudioSourceComponent.Stop();
        }
        else
        {
            Debug.Log("Main music audio source not assigned to Minigame Opener!");
        }
        if (minigameMusic != null)
        {
            minigameMusic.Play();
        }

        m_minigame = Instantiate(m_minigamePrefab);
        m_minigame.SetActive(true);

        if (m_minigameCountdown.TryGetComponent(out MinigameCountdown minigameCountdownScript))
        {
            minigameCountdownScript.StartCountdown(MinigameLength);
            minigameCountdownScript.CurrentTask = this;
        }
        else
        {
            Debug.Log("no minigamecountdown found");
        }
        
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
        if (MainMusic.Instance.AudioSourceComponent != null)
        {
            MainMusic.Instance.AudioSourceComponent.Play();
        }
    }
}
