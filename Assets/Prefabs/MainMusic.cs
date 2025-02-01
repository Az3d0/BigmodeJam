using UnityEngine;

public class MainMusic : MonoBehaviour
{

    public static MainMusic Instance;
    public AudioSource AudioSourceComponent;

    private void Awake()
    {
        Instance = this;
        if (gameObject.TryGetComponent(out AudioSource AS))
        {
            AudioSourceComponent = AS;
        }
    }
}
