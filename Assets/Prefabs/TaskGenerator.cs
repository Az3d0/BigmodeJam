using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TaskGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_randomSpawnPointsParent;
    private List<GameObject> m_randomSpawnPointGOS = new List<GameObject>();
    private List<SpawnPoint> m_randomSpawnPoint = new List<SpawnPoint>();

    [SerializeField] private List<GameObject> m_randomTaskAssets = new List<GameObject>();
    /// <summary>
    /// First gameobject is spawnpoint, Second is TaskAsset
    /// </summary>
    [SerializeField] private List<TasksAndSpawnpoints> m_tasksAndSpawnpoints = new List<TasksAndSpawnpoints>();

    private void Awake()
    {
        AddAllSpawnPointsToList();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    private GameObject RandomlySelectedTask()
    {
        int random = UnityEngine.Random.Range(0, m_randomTaskAssets.Count);

        return m_randomTaskAssets[random];
    }
    private void SpawnAtRandomSelectedSpawnpoint(GameObject randomTaskAsset)
    {
        List<GameObject> unoccupiedSpawnpoints = new List<GameObject>();
        for(int i = 0;  i < m_randomSpawnPoint.Count; i++)
        {
            if (!m_randomSpawnPoint[i].isOccupied)
            {
                unoccupiedSpawnpoints.Add(m_randomSpawnPointGOS[i]);
            }
        }
        if (unoccupiedSpawnpoints.Count <= 0) return;
        int random = UnityEngine.Random.Range(0, unoccupiedSpawnpoints.Count);
        unoccupiedSpawnpoints[random].GetComponent<SpawnPoint>().isOccupied = true;

        //it's important to set the task's parent to the spawnpoint, otherwise resetting the isOccupied state will not happen.
        GameObject taskAsset = Instantiate(randomTaskAsset);
        taskAsset.transform.parent = unoccupiedSpawnpoints[random].transform;
        taskAsset.transform.position = unoccupiedSpawnpoints[random].transform.position;
    }
    void AddAllSpawnPointsToList()
    {
        m_randomSpawnPointGOS.Clear();
        m_randomSpawnPoint.Clear();

        for (int i = 0; i < m_randomSpawnPointsParent.transform.childCount; i++)
        {
            m_randomSpawnPointGOS.Add(m_randomSpawnPointsParent.transform.GetChild(i).gameObject);
            m_randomSpawnPoint.Add(m_randomSpawnPointsParent.transform.GetChild(i).gameObject.GetComponent<SpawnPoint>());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5);

        while (true) // Infinite loop to keep spawning
        {
            if (!PlayerControls.Instance.isPaused)
            {
                SpawnAtRandomSelectedSpawnpoint(RandomlySelectedTask());
                int randomTimeInterval = UnityEngine.Random.Range(1, 5);
                yield return new WaitForSeconds(randomTimeInterval);
            }
            yield return new WaitForSeconds(2f);
        }
    }

}


[Serializable]
public struct TasksAndSpawnpoints
{
    public GameObject TaskAsset;
    public List<GameObject> FixedSpawnPoints;

}
