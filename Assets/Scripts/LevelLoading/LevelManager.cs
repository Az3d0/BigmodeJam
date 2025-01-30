using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] public LevelList levelList;
    [SerializeField] public int currentLevelIndex = 0;
    [SerializeField] public string currentLevelName = "Level1";
    public Level currentLevel;

    Scene currentScene;

    public static event Action<Level> OnLevelLoaded;

    void Start()
    {
        Debug.Log("Level manager created!");
        Debug.Log("Active scene on manager start: " + SceneManager.GetActiveScene().name);
        currentLevel = levelList.levels[currentLevelIndex];

#if UNITY_EDITOR        
        //if playtesting from  a specific level, manually update level manager variables
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            Debug.Log("Playing from specific level!");
            currentScene = SceneManager.GetActiveScene();
            currentLevelName = currentScene.name;
            currentLevelIndex = levelList.levels.FindIndex(level => level.name.Equals(currentLevelName));
            currentLevel = levelList.levels[currentLevelIndex];
            if (currentLevelIndex == -1) 
            {
                Debug.Log("Current level not found in level list!");
            }
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {

        //LOAD NEXT LEVEL
        //Make sure this isn't already the last level before trying to load next level
        if (currentLevelIndex >= levelList.levels.Count - 1)
        {
            Debug.Log("Game over! This is the last level");
            return;
        }

        StartCoroutine(LoadLevel(currentLevelIndex + 1, true));
    }

    private IEnumerator LoadLevel(int index, bool unloadCurrentLevel)
    {
        //ACTIVATE LOADING SCREEN HERE

        //UNLOAD CURRENT LEVEL
        if (unloadCurrentLevel) 
        {
            UnloadCurrentLevel();
        }


        //LOAD NEW LEVEL
        currentLevelIndex = index;
        currentLevelName = levelList.levels[currentLevelIndex].sceneName.ToString();
        currentLevel = levelList.levels[currentLevelIndex];
        Debug.Log("Loading level: " + currentLevelName);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentLevelName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnLevelLoaded?.Invoke(currentLevel);
        currentScene = SceneManager.GetSceneByName(currentLevelName);
        SceneManager.SetActiveScene(currentScene);

        //DEACTIVATE LOADING SCREEN HERE
    }

    private void UnloadCurrentLevel()
    {
        Debug.Log("Unloading level: " + levelList.levels[currentLevelIndex]);
        SceneManager.UnloadSceneAsync(levelList.levels[currentLevelIndex].sceneName);
    }

    

}
