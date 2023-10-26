using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    bool inventory_open = false;
    [SerializeField] int[] item_ids;
    int next_empty_slot = 0;
    int selected_slot = 0;

    [SerializeField] Canvas inventory_panel;
    [SerializeField] RectTransform inventory_slots;
    [SerializeField] RectTransform slot_selector;

    [SerializeField] Sprite grass_icon;
    [SerializeField] Sprite dirt_icon;
    [SerializeField] Sprite stone_icon;
    [SerializeField] Sprite diamond_icon;

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
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = grass_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 2:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = dirt_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 3:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = stone_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 4:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = diamond_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                default:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = null;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = false;
                    break;
            }
        }
    }

    void updateSlotSelector()
    {
        slot_selector.position = inventory_slots.GetChild(selected_slot).position;
    }
}
