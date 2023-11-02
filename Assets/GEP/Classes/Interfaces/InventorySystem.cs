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
    const int inventory_row_length = 9;
    const int inventory_size = 36;
    float select_block_text_disappear_timer;

    Vector3 block_placement_offset = new Vector3(0.0f, 0.5f, 1.0f);
    [SerializeField] Transform player_transform;

    [SerializeField] Canvas inventory_panel;
    [SerializeField] RectTransform inventory_slots;
    [SerializeField] RectTransform slot_selector;

    [SerializeField] Sprite grass_icon;
    [SerializeField] Sprite dirt_icon;
    [SerializeField] Sprite stone_icon;
    [SerializeField] Sprite iron_icon;
    [SerializeField] Sprite gold_icon;
    [SerializeField] Sprite diamond_icon;

    [SerializeField] GameObject grass_block;
    [SerializeField] GameObject dirt_block;
    [SerializeField] GameObject stone_block;
    [SerializeField] GameObject iron_block;
    [SerializeField] GameObject gold_block;
    [SerializeField] GameObject diamond_block;

    [SerializeField] GameObject select_block_text;

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
            updateNextEmptySlot();
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
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = iron_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 5:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = gold_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 6:
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

    void updateNextEmptySlot()
    {
        next_empty_slot++;
    }

    public bool getInventoryState()
    {
        return inventory_open;
    }

    public void moveSlotSelector(int direction)
    {
        switch (direction)
        {
            case 1:
                if (selected_slot > 0)
                {
                    selected_slot--;
                }
                break;
            case 2:
                if (selected_slot < inventory_size - 1)
                {
                    selected_slot++;
                }
                break;
            case 3:
                if (selected_slot > inventory_row_length - 1)
                {
                    selected_slot -= inventory_row_length;
                }
                break;
            case 4:
                selected_slot += inventory_row_length;
                break;
        }

        updateSlotSelector();
    }

    public void placeItem()
    {
        int current_ID = item_ids[selected_slot];
        item_ids[selected_slot] = 0;
        //updateNextEmptySlot();

        Vector3 block_placement_pos = player_transform.position + block_placement_offset;

        switch (current_ID)
        {
            case 1:
                Instantiate(grass_block, block_placement_pos, Quaternion.identity);
                break;
            case 2:
                Instantiate(dirt_block, block_placement_pos, Quaternion.identity);
                break;
            case 3:
                Instantiate(stone_block, block_placement_pos, Quaternion.identity);
                break;
            case 4:
                Instantiate(iron_block, block_placement_pos, Quaternion.identity);
                break;
            case 5:
                Instantiate(gold_block, block_placement_pos, Quaternion.identity);
                break;
            case 6:
                Instantiate(diamond_block, block_placement_pos, Quaternion.identity);
                break;
            default:
                if (!select_block_text.activeSelf)
                {
                    select_block_text.SetActive(true);
                    select_block_text_disappear_timer = 5.0f;
                    StartCoroutine(selectBlockTextDisappearCountdown());
                }
                break;
        }

        updateInventory();
    }

    IEnumerator selectBlockTextDisappearCountdown()
    {
        while (select_block_text_disappear_timer > 0)
        {
            select_block_text_disappear_timer -= Time.deltaTime;
            yield return null;
        }

        select_block_text.SetActive(false);
    }
}
