using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectZone : MonoBehaviour
{
    [SerializeField] private int m_amountToCollect;
    [SerializeField] private CollectionMode m_collectionMode;
    private Collectable m_collectable;
    private InputSystem_Actions m_inputs;

    //add a way to collect a specific object. Ideally without having to create a bunch of tags or specific scripts

    private void Awake()
    {
        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();
        if (m_collectionMode == CollectionMode.OnReleased) m_inputs.Minigame1.Select.canceled += CollectOnRelease;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Collectable collectable))
        {
            if (m_collectable == null) 
                m_collectable = collectable;

            if (m_collectionMode == CollectionMode.OnEnter) 
                Collect(m_collectable);

            else
                m_collectable.EmitFeedback(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_collectable?.EmitFeedback(false);
        m_collectable = null;
    }

    private void Collect(Collectable collectable)
    {
        Destroy(collectable.gameObject);

        m_collectable = null;

    }
    private void CollectOnRelease(InputAction.CallbackContext context)
    {
        Collect(m_collectable);
    }

    private void OnDestroy()
    {
        if (m_collectionMode == CollectionMode.OnReleased) m_inputs.Minigame1.Select.canceled -= CollectOnRelease;
    }
}

[Serializable]
public enum CollectionMode
{
    OnReleased,
    OnEnter
}
