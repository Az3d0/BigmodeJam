using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public int buildIndexOfManagersScene = 1;
    public int buildIndexOfFirstLevelScene = 2;

    public void LoadSceneAndSetActive(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    private IEnumerator LoadSceneCoroutine(int index)
    {
        // Start loading the scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            Debug.Log($"Loading {index}: {asyncLoad.progress * 100}%");
            yield return null; // Wait for the next frame
        }

        // Once loaded, set the scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
        Debug.Log($"Scene '{index}' is now active.");
    }

    public void LoadManagers()
    {

        LoadSceneAndSetActive(buildIndexOfFirstLevelScene);
        SceneManager.LoadSceneAsync(buildIndexOfManagersScene, LoadSceneMode.Additive);
    }
}
