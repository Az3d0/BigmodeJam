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

#if UNITY_EDITOR
        //if playtesting from  a specific level, manually update level manager variables
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            Debug.Log("Loading Screen: Playing from specific level!");

            FadeOutBlack(LevelManager.currentLevel);
        }
#endif
    }

    private void OnEnable()
    {
        LevelManager.BeforeLevelUnloaded += FadeToBlack;
        LevelManager.OnLevelLoaded += FadeOutBlack;
    }

    private void OnDisable()
    {
        LevelManager.BeforeLevelUnloaded -= FadeToBlack;
        LevelManager.OnLevelLoaded -= FadeOutBlack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeToBlackWrapper()
    {
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        Debug.Log("Invoked by levelmanager!");
        yield return StartCoroutine(Activate());
    }

    private void FadeOutBlack(Level currentLevel)
    {
        playerSprite.sprite = currentLevel.playerSprite;
        playerSprite.transform.DOScale(2, 1)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => StartCoroutine(Deactivate())); 
    }
    private IEnumerator Activate()
    {
        Debug.Log("Fading to black");
        float startAlpha = screenFade.color.a;  // Get the current alpha of the image
        Debug.Log("startAlpha: " + startAlpha);
        float timer = 0;

        // Gradually change the alpha to 0 over the fadeDuration
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;  // Increase time as the game progresses
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);  // Lerp between current alpha and 0
            Debug.Log("fading to black ALPHA: " + alpha);
            Color newColor = new Color(screenFade.color.r, screenFade.color.g, screenFade.color.b, alpha);  // Set new color with modified alpha
            screenFade.color = newColor;  // Apply the new color to the image
            yield return null;  // Wait for the next frame
        }

        // Ensure the alpha is set to 1 at the end
        screenFade.color = new Color(screenFade.color.r, screenFade.color.g, screenFade.color.b, 1);
    }    

    
    private IEnumerator Deactivate()
    {
        float startAlpha = screenFade.color.a;  // Get the current alpha of the image
        float timer = 0;

        // Gradually change the alpha to 0 over the fadeDuration
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;  // Increase time as the game progresses
            float alpha = Mathf.Lerp(startAlpha, 0, timer / fadeDuration);  // Lerp between current alpha and 0
            Debug.Log("fading out of black ALPHA: " + alpha);
            Color newColor = new Color(screenFade.color.r, screenFade.color.g, screenFade.color.b, alpha);  // Set new color with modified alpha
            screenFade.color = newColor;  // Apply the new color to the image
            yield return null;  // Wait for the next frame
        }

        // Ensure the alpha is set to 0 at the end
        screenFade.color = new Color(screenFade.color.r, screenFade.color.g, screenFade.color.b, 0);
    }
}
