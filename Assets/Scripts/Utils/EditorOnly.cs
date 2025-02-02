using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    void Awake()
    {
#if UNITY_EDITOR
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false); 
#endif
    }
}
