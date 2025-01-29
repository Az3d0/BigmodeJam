using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class DragableObject : MonoBehaviour
{

    private bool m_isBeingDragged = false;
    private Rigidbody2D m_rigidBody;
    [SerializeField] private float m_speed;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(m_isBeingDragged)
        {
            //transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);
            m_rigidBody.AddForce(Input.mousePositionDelta * m_speed);
            Debug.Log("drag");
        }
    }
    public void SetIsBeingDragged(bool state)
    {
        m_isBeingDragged = state;
        m_rigidBody.freezeRotation = state;
    }
}
