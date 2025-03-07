using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] public LevelList levelList;
    [SerializeField] public int currentLevelIndex = 0;
    [SerializeField] public string currentLevelName = "MainMenu";
    public static Level currentLevel;
    public static LevelManager Instance;
    public bool testMode = true;

    Scene currentScene;

    public static event Action<Level> OnLevelLoaded;
    public static event Func<IEnumerator> BeforeLevelUnloaded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentLevel = levelList.levels[currentLevelIndex];

#if UNITY_EDITOR        
        //if playtesting from  a specific level, manually update level manager variables
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            Debug.Log("LevelManager: Playing from specific level!");
            currentScene = SceneManager.GetActiveScene();
            currentLevelName = currentScene.name;
            Debug.Log("Active level: " + currentLevelName);
            currentLevelIndex = levelList.levels.FindIndex(level => level.name.Equals(currentLevelName));
            if (currentLevelIndex == -1)
            {
                Debug.Log("Current level not found in level list!");
                if (currentLevelName.Equals("Managers"))
                {
                    Debug.Log("If playing from editor, remove Managers scene and reload it in to make sure the Level scene is the active scene");
                }
            } else
            {
                currentLevel = levelList.levels[currentLevelIndex];
            }
            OnLevelLoaded?.Invoke(currentLevel);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool LoadNextLevel()
    {

        //LOAD NEXT LEVEL
        //Make sure this isn't already the last level before trying to load next level
        if (currentLevelIndex >= levelList.levels.Count - 1)
        {
            Debug.Log("Game over! Nothing left to do.");
            return false;
        }
        BeforeLevelUnloaded?.Invoke();
        StartCoroutine(HandleLevelTransition(currentLevelIndex + 1, true));
        return true;
    }

    public bool LoadPreviousLevel()
    {

        //LOAD NEXT LEVEL
        //Make sure this isn't already the first level before trying to load previous level
        if (currentLevelIndex <= 1)
        {
            Debug.Log("Game over! You got fired.");
            //GAME OVER SCREEN
            return false;
        }
        BeforeLevelUnloaded?.Invoke();
        StartCoroutine(HandleLevelTransition(currentLevelIndex - 1, true));
        return true;
    }

    public void RestartGame()
    {
        BeforeLevelUnloaded?.Invoke();
        StartCoroutine(HandleLevelTransition(0, true));
    }

    private IEnumerator HandleLevelTransition(int nextLevelIndex, bool unloadCurrentLevel)
    {
        if (BeforeLevelUnloaded != null)
        {
            foreach (Func<IEnumerator> subscriber in BeforeLevelUnloaded.GetInvocationList())
            {
                yield return StartCoroutine(subscriber());
            }
        }

        yield return StartCoroutine(LoadLevel(nextLevelIndex, unloadCurrentLevel));
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
        //DEACTIVATE LOADING SCREEN HERE
        OnLevelLoaded?.Invoke(currentLevel);
        currentScene = SceneManager.GetSceneByName(currentLevelName);
        SceneManager.SetActiveScene(currentScene);

    }

    private void UnloadCurrentLevel()
    {

        Debug.Log("Unloading level: " + levelList.levels[currentLevelIndex]);
        SceneManager.UnloadSceneAsync(levelList.levels[currentLevelIndex].sceneName);
    }



}
