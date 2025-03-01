using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Image screenFade;
    public Image playerSprite;
    PlayerControls playerControls;
    public float fadeDuration = 1f;
    public float playerSpriteZoom = 1.5f;
    public static event Action OnLoadingScreenDeactivated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (LevelManager.currentLevel.sceneName.Equals("MainMenu"))
        {
            StartCoroutine(Deactivate());
        }
#if UNITY_EDITOR
        //if playtesting from  a specific level, manually update level manager variables
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            Debug.Log("Loading Screen: Playing from specific level!");

            //DisplayPromotionSplashArt(LevelManager.currentLevel);
        }
#endif
    }

    private void OnEnable()
    {
        //LevelManager.BeforeLevelUnloaded += FadeToBlack;
        LevelManager.BeforeLevelUnloaded += Activate;
        LevelManager.OnLevelLoaded += DisplayPromotionSplashArt;
    }

    private void OnDisable()
    {
        //LevelManager.BeforeLevelUnloaded -= FadeToBlack;
        LevelManager.BeforeLevelUnloaded -= Activate;
        LevelManager.OnLevelLoaded -= DisplayPromotionSplashArt;
    }

    private void DisplayPromotionSplashArt(Level currentLevel)
    {
        if (currentLevel.playerSprite != null)
        {
            playerSprite.sprite = currentLevel.playerSprite;
            playerSprite.transform.DOBlendableRotateBy(new Vector3(0, 0, -360), 1, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(4, LoopType.Restart);
            playerSprite.transform.DOScale(playerSpriteZoom, 1.2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => StartCoroutine(Deactivate()));
        }
        else
        {
            StartCoroutine(Deactivate());
        }
    }
    private IEnumerator Activate()
    {
        if (playerControls == null)
        {
            playerControls = PlayerControls.Instance;
        }
        if (playerControls != null)
        {
            Debug.Log("fade transition starting!");
            playerControls.DisablePlayerMovement(true, false);
        }
        yield return screenFade.DOFade(1, fadeDuration).WaitForCompletion();
    }


    private IEnumerator Deactivate()
    {
        if (playerControls == null)
        {
            //GameObject pc = GameObject.Find("Player");
            //playerControls = (pc != null) ? pc.GetComponent<PlayerControls>() : null;
            playerControls = PlayerControls.Instance;
        }
        yield return screenFade.DOFade(0, fadeDuration).OnComplete(() =>
        {
            if (playerControls != null)
            {
                playerControls.EnablePlayerMovement(true);
            }
            OnLoadingScreenDeactivated?.Invoke();
        });
    }
}
