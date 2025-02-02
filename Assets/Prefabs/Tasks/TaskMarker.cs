using UnityEngine;

public class TaskMarker : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    float height;

    public float speed = 5;
    public float verticalShift = 5;
    public float amplitude = 5;

    private void Awake()
    {
        height = gameObject.transform.localPosition.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        height = amplitude * Mathf.Sin(Time.time * speed) + verticalShift;
        gameObject.transform.localPosition = new Vector3(0, height, 0);
    }
}
