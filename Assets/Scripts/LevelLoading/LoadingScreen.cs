using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    public Image screenFade;
    public Image playerSprite;
    public float fadeDuration = 1f;
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

            DisplayPromotionSplashArt(LevelManager.currentLevel);
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
        playerSprite.sprite = currentLevel.playerSprite;
        playerSprite.transform.DOScale(2, 1)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => StartCoroutine(Deactivate())); 
    }
    private IEnumerator Activate()
    {
        yield return screenFade.DOFade(1, fadeDuration).WaitForCompletion();
    }    

    
    private IEnumerator Deactivate()
    {
        yield return screenFade.DOFade(0, fadeDuration).WaitForCompletion();
    }
}
