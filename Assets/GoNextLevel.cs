using UnityEngine;

public class GoNextLevel : MonoBehaviour
{

    LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger()
    {
        if (levelManager == null) levelManager = FindFirstObjectByType<LevelManager>();

        levelManager.LoadNextLevel();
    }
}
