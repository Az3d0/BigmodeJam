using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelName : MonoBehaviour
{
    public TextMeshProUGUI levelName;
    public RectTransform levelNameTransform;
    public float showYPos;
    bool isPlayableLevel = false;

    private void OnEnable()
    {
        LoadingScreen.OnLoadingScreenDeactivated += DisplayLevelName;
        LevelManager.OnLevelLoaded += UpdateLevelName;
    }

    private void OnDisable()
    {
        LoadingScreen.OnLoadingScreenDeactivated -= DisplayLevelName;
        LevelManager.OnLevelLoaded -= UpdateLevelName;
    }

    private void UpdateLevelName(Level level)
    {
        if (level.levelName.Equals("Main Menu"))
        {
            isPlayableLevel = false;
        } else
        {
            isPlayableLevel = true;
            levelName.text = level.levelName;
        }
    }


    private void DisplayLevelName()
    {
        if (isPlayableLevel)
        {
            float screenHeight = Screen.height;
            //Start levelname off screen
            levelNameTransform.anchoredPosition = new Vector2 (0, screenHeight);


            levelNameTransform.DOAnchorPosY(showYPos, 1)
                .OnComplete(() =>
                {
                    levelNameTransform.DOAnchorPosY(screenHeight, 1)
                        .SetDelay(2);
                });
        }
    }

}
