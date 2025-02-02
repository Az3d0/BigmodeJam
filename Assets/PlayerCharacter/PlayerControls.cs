using System;
using System.Collections.Generic;
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

    public Dictionary<string, InputActionMap> MinigameInputActionMaps;

    public static PlayerControls Instance;

    [SerializeField] private float m_speed = 1f;
    [SerializeField] private float stopForceMultiplier = 10f;

    public event Action<GameObject> UpdateInteractable;
    public static event Action OnPause;
    public static event Action OnUnpause;

    private Animator animator;

    private GameObject pauseMenu;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        m_inputs = new InputSystem_Actions();
        //m_inputs.Enable();



        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        DisablePlayerMovement(true, false);
    }

    private void OnEnable()
    {
        m_inputs.Player.Move.performed += OnPlayerMoved;
        m_inputs.Player.Move.canceled += OnPlayerMoved;
        m_inputs.Player.Interact.performed += OnPlayerInteract;
        m_inputs.Player.PauseMenu.performed += OnPauseMenu;
        UpdateInteractable += OnInteractableUpdated;

        animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        m_inputs.Player.Move.performed -= OnPlayerMoved;
        m_inputs.Player.Move.canceled -= OnPlayerMoved;
        m_inputs.Player.Interact.performed -= OnPlayerInteract;
        m_inputs.Player.PauseMenu.performed -= OnPauseMenu;
        UpdateInteractable -= OnInteractableUpdated;
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

    private void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");
        }
        if (pauseMenu != null)
        {
            if (!isPaused)
            {
                OpenPauseMenu();
            }
            else
            {

                ClosePauseMenu();
            }
        }
        else
        {
            Debug.Log("Can't find pause menu!!");
        }
    }

    public void OpenPauseMenu()
    {
        DisablePlayerMovement(false, false);
        pauseMenu.GetComponent<PauseMenu>().pauseBackground.SetActive(true);
        pauseMenu.GetComponents<Tween_Scale>()[1].TriggerScale();
        isPaused = true;
    }

    public void ClosePauseMenu()
    {
        EnablePlayerMovement(false);
        pauseMenu.GetComponent<PauseMenu>().pauseBackground.SetActive(false);
        pauseMenu.GetComponents<Tween_Scale>()[0].TriggerScale();
        isPaused = false;
    }

    private void OnInteractableUpdated(GameObject go)
    {
        m_interactableObject = go;
    }

    private void OnPlayerInteract(InputAction.CallbackContext context)
    {

        if (m_interactableObject != null)
        {
            if (m_interactableObject.TryGetComponent<Task>(out Task minigameOpener))
            {
                minigameOpener.OpenMinigame();
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

    public void DisablePlayerMovement(bool isTransition, bool openingMinigame)
    {
        //Don't pause timers if opening minigame
        if (!openingMinigame)
        {
            //pause timers
            OnPause?.Invoke();
        }
        m_inputs.Player.Move.Disable();
        m_inputs.Player.Interact.Disable();

        //Stop player from opening pause menu during transition
        if (isTransition)
        {
            m_inputs.Player.PauseMenu.Disable();
        }
    }

    public void EnablePlayerMovement(bool isTransition)
    {
        OnUnpause?.Invoke();
        m_inputs.Player.Move.Enable();
        m_inputs.Player.Interact.Enable();
        if (isTransition)
        {
            m_inputs.Player.PauseMenu.Enable();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
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

