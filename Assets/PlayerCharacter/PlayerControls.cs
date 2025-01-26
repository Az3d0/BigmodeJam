using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerControls : MonoBehaviour
{
    private InputSystem_Actions m_inputs;
    private Rigidbody2D m_rigidbody;
    private Vector3 m_inputDirection;
    private GameObject m_interactableObject;

    public static PlayerControls Instance;

    [SerializeField] private float m_speed = 1f;

    public event Action<GameObject> UpdateInteractable;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        m_inputs = new InputSystem_Actions();
        m_inputs.Enable();
        m_inputs.Player.Move.performed += OnPlayerMoved;
        m_inputs.Player.Move.canceled += OnPlayerMoved;
        m_inputs.Player.Interact.performed += OnPlayerInteract;

        m_rigidbody = GetComponent<Rigidbody2D>();
        UpdateInteractable += OnInteractableUpdated;
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(m_inputDirection * m_speed);
    }
    private void OnInteractableUpdated(GameObject go)
    {
        m_interactableObject = go;
        Debug.Log(m_interactableObject);
    }

    private void OnPlayerInteract(InputAction.CallbackContext context)
    {

        if (m_interactableObject != null)
        {
            if(m_interactableObject.TryGetComponent<MinigameOpener>(out MinigameOpener minigameOpener))
            {
                minigameOpener.OpenMinigame();
                DisablePlayerMovement();
            }
            else
            {
                Debug.Log("interacting");
            }
        }
    }

    private void OnPlayerMoved(InputAction.CallbackContext context)
    {
        m_inputDirection = new Vector3 (context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
    }

    public void DisablePlayerMovement()
    {
        m_inputs.Player.Disable();
    }

    public void EnablePlayerMovement()
    {
        m_inputs.Player.Enable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Interactable")
        {
            UpdateInteractable.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == m_interactableObject)
        {
            UpdateInteractable.Invoke(null);
        }
    }
}
