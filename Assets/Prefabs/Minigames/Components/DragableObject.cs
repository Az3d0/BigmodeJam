using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragableObject : MonoBehaviour
{

    protected bool m_isBeingDragged = false;
    private Rigidbody2D m_rigidBody;
    [SerializeField] private float m_speed;

    protected virtual void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }
    protected virtual void FixedUpdate()
    {
        if (m_isBeingDragged)
        {
            //Divide by 2
            m_rigidBody.AddForce(Input.mousePositionDelta / 2 * m_speed);
        }
    }

    public virtual void OnGrabbed()
    {
        SetIsBeingDragged(true);

    }
    public virtual void OnReleased()
    {
        SetIsBeingDragged(false);
    }
    public void SetIsBeingDragged(bool state)
    {
        m_isBeingDragged = state;
        m_rigidBody.freezeRotation = state;
    }
}
