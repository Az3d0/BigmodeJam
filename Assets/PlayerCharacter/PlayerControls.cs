using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerControls : MonoBehaviour
{
    private InputSystem_Actions m_inputs;
    private Rigidbody2D m_rigidbody;
    private Vector2 m_inputDirection;
    private GameObject m_interactableObject;

    public static PlayerControls Instance;

    [SerializeField] private float m_speed = 1f;
    [SerializeField] private float stopForceMultiplier = 10f;

    public event Action<GameObject> UpdateInteractable;

    private Animator animator;

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

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (m_inputDirection != Vector2.zero)
        {
            m_rigidbody.AddForce(m_inputDirection * m_speed);
            animator.SetBool("isWalking", true);
        }
        else
        {
            m_rigidbody.AddForce(-m_rigidbody.linearVelocity * stopForceMultiplier);
            animator.SetBool("isWalking", false);
        }

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
        m_inputDirection = context.ReadValue<Vector2>();

        if (m_inputDirection != Vector2.zero)
        {
            animator.SetFloat("x_input", m_inputDirection.x);
            animator.SetFloat("y_input", m_inputDirection.y);
        }

        if (context.canceled)
        {
            m_inputDirection = Vector2.zero;
        }
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

    //Fixes missing reference exception with animator when reloading level
    //Could potentially move player object into Managers scene instead and disable/enable when loading new levels with specified spawn points
    private void OnDestroy()
    {
        m_inputs.Player.Move.performed -= OnPlayerMoved;
        m_inputs.Player.Move.canceled -= OnPlayerMoved;
        m_inputs.Player.Interact.performed -= OnPlayerInteract;
        UpdateInteractable -= OnInteractableUpdated;
    }
}
