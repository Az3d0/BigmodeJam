using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        if (LevelManager.Instance.testMode)
        {
            gameObject.SetActive(true);
        } else
        {
            gameObject.SetActive(false);
        }
#else
        gameObject.SetActive(false); 
#endif
    }
}
