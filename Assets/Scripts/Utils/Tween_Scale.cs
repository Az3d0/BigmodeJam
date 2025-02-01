using DG.Tweening;
using UnityEngine;

public class Tween_Scale : MonoBehaviour
{
    public float targetValue;
    public float duration;

    public void TriggerScale()
    {
        this.transform.DOScale(targetValue, duration);
    }
}
