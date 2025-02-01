using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    PlayerControls playerControls;
    public GameObject pauseBackground;

    public void ResumeGame()
    {
        if (playerControls == null)
        {
            //GameObject pc = GameObject.Find("Player");
            //playerControls = (pc != null)? pc.GetComponent<PlayerControls>() : null;
            playerControls = PlayerControls.Instance;
        }

        if (playerControls != null)
        {
            playerControls.ClosePauseMenu();
        }
    }
}
