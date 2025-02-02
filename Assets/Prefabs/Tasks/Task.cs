using System;
using UnityEngine;

public class Task : MonoBehaviour
{
    //Task Fields
    [Header("Task")]
    [SerializeField] private LifetimeCountdown m_LifetimeCountdown;
    [SerializeField] private float m_lifetimeLength;
    private bool m_lifeTimeCounterOver = false;
    //Minigame Fields
    [Header("Minigame")]
    [SerializeField] private GameObject m_minigamePrefab;
    public float MinigameLength = 5;
    public AudioSource minigameMusic;


    private GameObject m_minigame;
    private Minigame m_minigameScript;
    public static event Action<bool> OnMinigameEnd;


    private void Start()
    {
        m_LifetimeCountdown.StartCountdown(m_lifetimeLength);
        m_LifetimeCountdown.OnTimesUp += OnLifeTimesUp;

    }

    private void OnLifeTimesUp()
    {
        m_lifeTimeCounterOver = true;
        if (m_minigame == null)
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
        if (minigameMusic != null)
        {
            minigameMusic.Play();
        }

        m_minigame = Instantiate(m_minigamePrefab);
        m_minigame.SetActive(true);


        if (m_minigame.TryGetComponent(out Minigame minigameScript))
        {
            m_minigameScript = minigameScript;
            m_minigameScript.MinigameLenth = MinigameLength;
            m_minigameScript.OnGameEnded += MinigameEnded;
        }
        PlayerControls.Instance.DisablePlayerMovement(false, true);

    }

    private void MinigameEnded(bool isWon)
    {
        CloseMinigame();

        if (isWon)
        {
            GameManager.Instance.UpdateXp(true);
            Debug.Log("won");
            Destroy(gameObject);
        }
        else
        {
            if (m_lifeTimeCounterOver)
            {
                GameManager.Instance.UpdateXp(false);
                Debug.Log("Lost");
                Destroy(gameObject);
            }
        }
    }

    public void CloseMinigame()
    {

        Destroy(m_minigame);
        m_minigameScript.OnGameEnded -= MinigameEnded;
        PlayerControls.Instance.EnablePlayerMovement(false);

        //Resume main music
        if (minigameMusic != null)
        {
            minigameMusic.Stop();
        }
        if (MainMusic.Instance.AudioSourceComponent != null)
        {
            MainMusic.Instance.AudioSourceComponent.Play();
            MainMusic.Instance.CrossFade();
        }
    }

    private void OnDestroy()
    {
        if(transform.parent.TryGetComponent(out SpawnPoint spawnPoint))
        {
            spawnPoint.isOccupied = false;
        }
        m_LifetimeCountdown.OnTimesUp -= OnLifeTimesUp;

    }
}
