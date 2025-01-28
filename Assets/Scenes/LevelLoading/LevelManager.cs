using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] public LevelList levelList;
    [SerializeField] public int currentLevelIndex = 0;
    [SerializeField] public string currentLevelName = "Level1";

    Scene currentScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Level manager created!");
        Debug.Log("Active scene on manager start: " + SceneManager.GetActiveScene().name);
        
        if (currentLevelIndex != levelList.levelNames.IndexOf(SceneManager.GetActiveScene().name))
        {
            currentScene = SceneManager.GetActiveScene();
            currentLevelName = currentScene.name;
            currentLevelIndex = levelList.levelNames.IndexOf(currentLevelName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {

        //LOAD NEXT LEVEL
        //Make sure this isn't already the last level before trying to load next level
        if (currentLevelIndex >= levelList.levelNames.Count - 1)
        {
            Debug.Log("Game over! This is the last level");
            return;
        }

        LoadLevel(currentLevelIndex + 1, true);
    }

    private void LoadLevel(int index, bool unloadCurrentLevel)
    {
        //ACTIVATE LOADING SCREEN

        //UNLOAD CURRENT LEVEL
        if (unloadCurrentLevel) 
        {
            UnloadCurrentLevel();
        }


        //LOAD NEW LEVEL
        currentLevelIndex = index;
        currentLevelName = levelList.levelNames[currentLevelIndex];
        currentScene = SceneManager.GetSceneByName(currentLevelName);
        Debug.Log("Loading level: " + currentLevelName);
        Debug.Log("Active scene before loading: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync(currentLevelName, LoadSceneMode.Additive);
        if (currentScene.IsValid() && currentScene.isLoaded)
        {
            SceneManager.SetActiveScene(currentScene);
            Debug.Log("Active scene after loading: " + SceneManager.GetActiveScene().name);
        } 
    }

    private void UnloadCurrentLevel()
    {
        Debug.Log("Unloading level: " + levelList.levelNames[currentLevelIndex]);
        SceneManager.UnloadSceneAsync(levelList.levelNames[currentLevelIndex]);
    }

    

}
