using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    public void addItem(int item_id)
    {
        if (next_empty_slot < item_ids.Length)
        {
            item_ids[next_empty_slot] = item_id;
            next_empty_slot++;
            updateInventory();
        }
    }

    void updateInventory()
    {
        for (int i = 0; i < item_ids.Length; i++)
        {
            switch (item_ids[i])
            {
                case 1:
                    inventory_panel.GetComponent<RectTransform>().GetChild(i).GetComponent<Image>();
                    break;
            }
        }
    }
}
