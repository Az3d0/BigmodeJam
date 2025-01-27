using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame1 : MonoBehaviour
{
    private InputSystem_Actions m_inputs;

    private void Awake()
    {
        m_inputs = new InputSystem_Actions();
        m_inputs.Minigame1.Enable();

        m_inputs.Minigame1.Jump.started += Test;
    }

    private void Test(InputAction.CallbackContext context)
    {
        Debug.Log("Space");
    }

    private void OnDestroy()
    {
        m_inputs.Minigame1.Jump.started -= Test;
        m_inputs.Minigame1.Disable();
    }
}
