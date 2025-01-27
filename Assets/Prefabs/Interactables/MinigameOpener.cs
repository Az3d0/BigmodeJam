using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinigameOpener : MonoBehaviour
{
    [SerializeField] private GameObject m_minigamePrefab;
    [SerializeField] private float minigameLength = 10;

    private GameObject m_minigame;
    public void OpenMinigame()
    {

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
    }
}
