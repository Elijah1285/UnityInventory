using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    bool inventory_open = false;
    [SerializeField] int[] item_ids;
    int next_empty_slot = 0;

    [SerializeField] Canvas inventory_panel;

    private void Awake()
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

    public void toggleInventory()
    {
        inventory_open = !inventory_open;

        if (inventory_open)
        {
            inventory_panel.gameObject.SetActive(true);
        }
        else
        {
            inventory_panel.gameObject.SetActive(false);
        }
    }

    public void addItem(int item_ID)
    {
        item_ids[next_empty_slot] = item_ID;
    }
}
