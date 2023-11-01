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

    public void OnInventoryLeft(InputValue value)
    {
        if (InventorySystem.instance.getInventoryState())
        {
            InventorySystem.instance.moveSlotSelector(1);
        }
    }

    public void OnInventoryRight(InputValue value)
    {
        if (InventorySystem.instance.getInventoryState())
        {
            InventorySystem.instance.moveSlotSelector(2);
        }
    }

    public void OnInventoryUp(InputValue value)
    {
        if (InventorySystem.instance.getInventoryState())
        {
            InventorySystem.instance.moveSlotSelector(3);
        }
    }

    public void OnInventoryDown(InputValue value)
    {
        if (InventorySystem.instance.getInventoryState())
        {
            InventorySystem.instance.moveSlotSelector(4);
        }
    }

    public void OnInventoryPlace(InputValue value)
    {
        InventorySystem.instance.placeItem();
    }
}
