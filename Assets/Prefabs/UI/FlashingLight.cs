using DG.Tweening;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent (typeof(Light2D))]
public class FlashingLight : MonoBehaviour
{
    private Light2D m_lightSource;

    [SerializeField] float speed = 5;
    [SerializeField] float verticalShift = 5;
    [SerializeField] float amplitude = 5;
    private void Awake()
    {
        m_lightSource = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        m_lightSource.intensity = amplitude * Mathf.Sin(Time.time * speed) + verticalShift;
    }
}
