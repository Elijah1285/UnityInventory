using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

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

    public void OnToggle(InputValue value)
    {
        InventorySystem.instance.toggleInventory();

        if (prompt != null)
        {
            Destroy(prompt.gameObject);
        }
    }


}
