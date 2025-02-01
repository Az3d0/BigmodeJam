using DG.Tweening;
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

    public void CrossFade()
    {
        AudioSourceComponent.DOFade(1, 1);
    }
}
