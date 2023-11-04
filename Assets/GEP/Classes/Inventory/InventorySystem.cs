using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    bool inventory_open = false;
    int next_empty_slot = 0;
    int selected_slot = 0;
    const int inventory_row_length = 9;
    const int inventory_size = 36;
    float select_block_text_disappear_timer;
    bool chest_open = false;
    bool in_chest = false;
    int[,] item_ids = new int[36, 2];
    Chest current_chest = null;
    Vector3 block_placement_offset = new Vector3(0.0f, 0.5f, 1.0f);


    [SerializeField] int max_stack;
    [SerializeField] Transform player_transform;
    [SerializeField] Canvas inventory_panel;
    [SerializeField] RectTransform inventory_slots;
    [SerializeField] RectTransform inventory_numbers;
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
    [SerializeField] GameObject place_block_text;
    [SerializeField] TMP_Text selected_item_text;

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

            if (place_block_text != null)
            {
                place_block_text.SetActive(true);
            }
        }
        else
        {
            inventory_panel.gameObject.SetActive(false);
        }
    }

    public void enterChest(Chest chest)
    {
        chest_open = true;

        inventory_panel.transform.GetChild(0).localPosition = new Vector3(0, -220f, 0);
        selected_item_text.gameObject.SetActive(false);
        current_chest = chest;
        current_chest.enterChest();
    }

    public void exitChest()
    {
        chest_open = false;
        in_chest = false;
        slot_selector.gameObject.SetActive(true);

        inventory_panel.transform.GetChild(0).localPosition = new Vector3(0, -35f, 0);
        selected_item_text.gameObject.SetActive(true);
        current_chest.exitChest();
        current_chest = null;
    }

    public void chestSwitch()
    {
        in_chest = !in_chest;

        if (!in_chest)
        {
            slot_selector.gameObject.SetActive(true);
        }
        else
        {
            slot_selector.gameObject.SetActive(false);
        }

        current_chest.chestSwitch();
    }

    public bool addItem(int item_id)
    {
        for (int i = 0; i < item_ids.GetLength(0); i++)
        {
            if (item_id == item_ids[i, 0] && item_ids[i, 1] < max_stack)
            {
                item_ids[i, 1]++;

                GameObject item_count = inventory_numbers.GetChild(i).gameObject;

                if (!item_count.activeSelf)
                {
                    item_count.SetActive(true);
                }

                item_count.GetComponent<TMP_Text>().text = item_ids[i, 1].ToString();
                return true;
            }
        }


        if (next_empty_slot < item_ids.Length)
        {
            item_ids[next_empty_slot, 0] = item_id;
            item_ids[next_empty_slot, 1]++;
            inventory_numbers.GetChild(next_empty_slot).GetComponent<TMP_Text>().text = item_ids[next_empty_slot, 1].ToString();
            
            updateInventory();

            if (selected_slot == next_empty_slot)
            {
                updateSelectedItemText();
            }

            updateNextEmptySlot();

            return true;
        }

        return false;
    }

    void updateInventory()
    {
        for (int i = 0; i < item_ids.GetLength(0); i++)
        {
            switch (item_ids[i, 0])
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

    void updateSelectedItemText()
    {
        switch (item_ids[selected_slot, 0])
        {
            case 1:
                selected_item_text.text = "Grass:\nIt's one of the most common plants in the world";
                break;
            case 2:
                selected_item_text.text = "Dirt:\nIt's made up of sand, silt and clay";
                break;
            case 3:
                selected_item_text.text = "Stone:\nMost ancient monuments use this resource";
                break;
            case 4:
                selected_item_text.text = "Iron:\nIt's symbol is Fe, it's combined with carbon to form steel";
                break;
            case 5:
                selected_item_text.text = "Gold:\nIt's symbol is Au, it's precious but delicate";
                break;
            case 6:
                selected_item_text.text = "Diamond:\nThe strongest and most precious resource";
                break;
            default:
                selected_item_text.text = "";
                break;
        }
    }

    void updateNextEmptySlot()
    {
        bool set_empty_slot = false;

        for (int i = 0; i < item_ids.GetLength(0); i++)
        {
            if (item_ids[i, 0] == 0)
            {
                next_empty_slot = i;
                set_empty_slot = true;
                break;
            }
        }

        if (!set_empty_slot)
        {
            next_empty_slot = item_ids.Length;
        }
    }

    public bool getInventoryState()
    {
        return inventory_open;
    }

    public void moveSlotSelector(int direction)
    {
        if (!in_chest)
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
                    if (selected_slot < inventory_size - inventory_row_length)
                    {
                        selected_slot += inventory_row_length;
                    }
                    break;
            }

            updateSlotSelector();
            updateSelectedItemText();
        }
        else
        {
            current_chest.moveSlotSelector(direction);
        }
    }

    public void transferItem()
    {
        int current_ID = item_ids[selected_slot, 0];

        if (item_ids[selected_slot, 1] > 0)
        {
            item_ids[selected_slot, 1]--;
            GameObject item_count = inventory_numbers.GetChild(selected_slot).gameObject;
            item_count.GetComponent<TMP_Text>().text = item_ids[selected_slot, 1].ToString();

            if (item_ids[selected_slot, 1] == 1)
            {
                item_count.SetActive(false);
            }
            else if (item_ids[selected_slot, 1] <= 0)
            {
                item_ids[selected_slot, 0] = 0;
                updateSelectedItemText();
            }
        }

        updateInventory();
        updateNextEmptySlot();

        if (!chest_open)
        {
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
        }
        else
        {
            if (!in_chest)
            {
                current_chest.addItem(current_ID);
            }
        }

        if (place_block_text != null)
        {
            Destroy(place_block_text);
        }
    }

    public bool getChestOpen()
    {
        return chest_open;
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
