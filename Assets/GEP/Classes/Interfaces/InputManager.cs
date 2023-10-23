using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public InputAction controls;

    [SerializeField] Canvas prompt;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        if (Input.GetButtonDown("ToggleInv"))
        {
            InventorySystem.instance.toggleInventory();

            if (prompt != null)
            {
                Destroy(prompt.gameObject);
            }
        }
    }
}
