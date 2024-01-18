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
    float select_item_text_disappear_timer;
    bool chest_open = false;
    bool in_chest = false;
    int[,] item_ids = new int[36, 2];
    Chest current_chest = null;
    Vector3 block_placement_offset = new Vector3(0.0f, 0.5f, 1.0f);


    [SerializeField] int max_stack_A;
    [SerializeField] int max_stack_B;
    [SerializeField] int max_stack_C;
    [SerializeField] int[] ids_A;
    [SerializeField] int[] ids_B;
    [SerializeField] int[] ids_C;
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
    [SerializeField] Sprite snowball_icon;
    [SerializeField] Sprite sword_icon;
    [SerializeField] GameObject grass_block;
    [SerializeField] GameObject dirt_block;
    [SerializeField] GameObject stone_block;
    [SerializeField] GameObject iron_block;
    [SerializeField] GameObject gold_block;
    [SerializeField] GameObject diamond_block;
    [SerializeField] GameObject snowball;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject select_item_text;
    [SerializeField] GameObject transfer_item_text;
    [SerializeField] GameObject chest_switch_text;
    [SerializeField] GameObject transfer_all_text;
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

            if (transfer_item_text != null)
            {
                transfer_item_text.SetActive(true);
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

        if (chest_switch_text != null)
        {
            chest_switch_text.SetActive(true);
        }

        if (transfer_all_text != null)
        {
            transfer_all_text.SetActive(true);
        }
    }

    public void exitChest()
    {
        if (current_chest != null)
        {
            chest_open = false;
            in_chest = false;
            slot_selector.gameObject.SetActive(true);

            inventory_panel.transform.GetChild(0).localPosition = new Vector3(0, -35f, 0);
            selected_item_text.gameObject.SetActive(true);
            current_chest.exitChest();
            current_chest = null;

            if (chest_switch_text != null)
            {
                chest_switch_text.SetActive(false);
            }

            if (transfer_all_text != null)
            {
                transfer_all_text.SetActive(false);
            }
        }
    }

    public void chestSwitch()
    {
        if (chest_open)
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

            if (chest_switch_text != null)
            {
                Destroy(chest_switch_text);
            }
        }
    }

    public bool addItem(int item_id)
    {
        int max_stack = calculateMaxStack(item_id);

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

    public void transferItem()
    {
        int current_ID = item_ids[selected_slot, 0];

        if (!chest_open)
        {
            if (item_ids[selected_slot, 1] > 0)
            {
                removeItem();

                Vector3 item_placement_pos = player_transform.position + block_placement_offset;

                switch (current_ID)
                {
                    case 1:
                        Instantiate(grass_block, item_placement_pos, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(dirt_block, item_placement_pos, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(stone_block, item_placement_pos, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(iron_block, item_placement_pos, Quaternion.identity);
                        break;
                    case 5:
                        Instantiate(gold_block, item_placement_pos, Quaternion.identity);
                        break;
                    case 6:
                        Instantiate(diamond_block, item_placement_pos, Quaternion.identity);
                        break;
                    case 7:
                        Instantiate(snowball, item_placement_pos, Quaternion.identity);
                        break;
                    case 8:
                        Instantiate(sword, item_placement_pos, Quaternion.identity);
                        break;
                }
            }   
        }
        else
        {
            if (in_chest)
            {
                if (checkIfFreeSpace(current_ID))
                {
                    current_chest.transferItem();
                }
                else
                {
                    Debug.Log("inventory full");
                }
            }
            else
            {
                int max_stack = calculateMaxStack(current_ID);

                if (current_chest.checkIfFreeSpace(current_ID, max_stack))
                {
                    removeItem();

                    current_chest.addItem(current_ID, max_stack);
                }
                else
                {
                    Debug.Log("chest full");
                }
            }

            if (!select_item_text.activeSelf)
            {
                select_item_text.SetActive(true);
                select_item_text_disappear_timer = 5.0f;
                StartCoroutine(selectBlockTextDisappearCountdown());
            }
            if (transfer_item_text != null)
            {
                Destroy(transfer_item_text);
            }
        }

        updateInventory();
        updateNextEmptySlot();
    }

    public void transferAll()
    {
        if (chest_open)
        {
            if (!in_chest)
            {
                int item_count = item_ids[selected_slot, 1];

                for (int i = 0; i < item_count; i++)
                {
                    transferItem();
                }
            }
            else
            {
                current_chest.transferAll();
            }

            updateInventory();

            if (transfer_all_text != null)
            {
                Destroy(transfer_all_text);
            }
        }
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
                case 7:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = snowball_icon;
                    inventory_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 8:
                    inventory_slots.GetChild(i).GetComponent<Image>().sprite = sword_icon;
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
            case 7:
                selected_item_text.text = "Snowball:\nGreat for a snowball fight";
                break;
            case 8:
                selected_item_text.text = "Sword:\nGreat for a swordfight";
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

    public bool getChestOpen()
    {
        return chest_open;
    }

    void removeItem()
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

    bool checkIfFreeSpace(int item_id)
    {
        bool free_space = false;
        int max_stack = calculateMaxStack(item_id);

        for (int i = 0; i < item_ids.Length; i++)
        {
            if (item_ids[i, 0] == item_id && item_ids[i, 1] < max_stack)
            {
                free_space = true;
                break;
            }
            else if (item_ids[i, 0] == 0)
            {
                free_space = true;
                break;
            }
        }

        return free_space;
    }

    int calculateMaxStack(int item_id)
    {
        int max_stack = 0;
        bool set_max_stack = false;

        for (int i = 0; i < ids_A.Length; i++)
        {
            if (ids_A[i] == item_id)
            {
                max_stack = max_stack_A;
                set_max_stack = true;
            }
        }

        if (!set_max_stack)
        {
            for (int i = 0; i < ids_B.Length; i++)
            {
                if (ids_B[i] == item_id)
                {
                    max_stack = max_stack_B;
                    set_max_stack = true;
                }
            }
        }

        if (!set_max_stack)
        {
            for (int i = 0; i < ids_C.Length; i++)
            {
                if (ids_C[i] == item_id)
                {
                    max_stack = max_stack_C;
                    set_max_stack = true;
                }
            }
        }

        return max_stack;
    }

    IEnumerator selectBlockTextDisappearCountdown()
    {
        while (select_item_text_disappear_timer > 0)
        {
            select_item_text_disappear_timer -= Time.deltaTime;
            yield return null;
        }

        select_item_text.SetActive(false);
    }
}
