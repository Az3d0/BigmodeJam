using DG.Tweening;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent (typeof(Light2D))]
public class FlashingLight : MonoBehaviour
{
    [HideInInspector] public Light2D m_lightSource;

    public float speed = 5;
    public float verticalShift = 5;
    public float amplitude = 5;
    private void Awake()
    {
        m_lightSource = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        m_lightSource.intensity = amplitude * Mathf.Sin(Time.time * speed) + verticalShift;
    }
}
