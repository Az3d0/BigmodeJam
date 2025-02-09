using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Task : MonoBehaviour
{
    [Header("Task")]
    [Space(10)]

    [SerializeField] private LifetimeCountdown m_LifetimeCountdown;
    [SerializeField] private float m_lifetimeLength;
    private bool m_lifeTimeCountdownOver = false;

    [Header("Minigame")]
    [Space(10)]

    [SerializeField] private GameObject m_minigamePrefab;
    public float MinigameLength = 5;

    [Header("DebugMode")]
    [Space(10)]

    [SerializeField] private bool m_debugMode = false;

    private AudioSource m_audioSource;
    private GameObject m_minigameGO;
    private Minigame m_minigame;
    public static event Action<bool> OnMinigameEnd;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (m_debugMode)
        {
            m_lifetimeLength = 100000;
        }
        m_LifetimeCountdown.StartCountdown(m_lifetimeLength);

        //debug stuff

        m_LifetimeCountdown.OnTimesUp += LoseMinigame;

    }

    private void LoseMinigame()
    {
        //this bool is needed to trigger lose state when timer expired while playing the minigame
        m_lifeTimeCountdownOver = true;

        //this statement checks if the player is in the minigame or not
        if (m_minigameGO == null)
        {
            GameManager.Instance.UpdateXp(false);
            Debug.Log("Lost");
            Destroy(gameObject);

        }
    }

    public void OpenMinigame()
    {
        //Stop main music and play minigame music
        if (MainMusic.Instance.AudioSourceComponent != null)
        {
            MainMusic.Instance.AudioSourceComponent.volume = 0;
            MainMusic.Instance.AudioSourceComponent.Pause();
        }

        m_minigameGO = Instantiate(m_minigamePrefab);
        m_minigameGO.SetActive(true);


        if (m_minigameGO.TryGetComponent(out Minigame minigameScript))
        {
            m_minigame = minigameScript;
            m_minigame.MinigameLenth = MinigameLength;
            m_minigame.OnGameEnded += MinigameEnded;
        }
        PlayerControls.Instance.DisablePlayerMovement(false, true);

    }

    private void MinigameEnded(bool isWon)
    {
        CloseMinigame();

        if (m_debugMode) 
        { 
            OpenMinigame();
            return;
        }

        if (isWon)
        {
            GameManager.Instance.UpdateXp(true);
            Debug.Log("won");
            Destroy(gameObject);
        }
        else
        {
            if (m_lifeTimeCountdownOver)
            {
                GameManager.Instance.UpdateXp(false);
                Debug.Log("Lost");
                Destroy(gameObject);
            }
        }
    }

    public void CloseMinigame()
    {

        Destroy(m_minigameGO);
        m_minigame.OnGameEnded -= MinigameEnded;
        PlayerControls.Instance.EnablePlayerMovement(false);

        if (MainMusic.Instance.AudioSourceComponent != null)
        {
            MainMusic.Instance.AudioSourceComponent.Play();
            MainMusic.Instance.CrossFade();
        }
    }

    private void OnDestroy()
    {
        if(transform.parent != null)
        {
            if (transform.parent.TryGetComponent(out SpawnPoint spawnPoint)) spawnPoint.isOccupied = false;
        }

        m_LifetimeCountdown.OnTimesUp -= LoseMinigame;

    }
}
