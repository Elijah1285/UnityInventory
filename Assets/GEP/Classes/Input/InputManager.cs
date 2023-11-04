using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] GameObject toggle_inv_text;
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

        if (toggle_inv_text != null)
        {
            Destroy(toggle_inv_text);
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

    public void OnInventoryTransfer(InputValue value)
    {
        InventorySystem.instance.transferItem();
    }

    public void OnInventoryTransferAll(InputValue value)
    {
        InventorySystem.instance.transferAll();
    }

    public void OnChestSwitch(InputValue value)
    {
        InventorySystem.instance.chestSwitch();
    }

    public void OnChestExit(InputValue value)
    {
        InventorySystem.instance.exitChest();
    }
}
