using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    const int chest_row_length = 9;
    int next_empty_slot = 0;
    int selected_slot = 0;
    bool chest_open = false;
    bool in_chest = false;
    int[,] item_ids = new int[18, 2];
    const int chest_size = 18;

    [SerializeField] Canvas chest_panel;
    [SerializeField] RectTransform chest_numbers;
    [SerializeField] RectTransform chest_slots;
    [SerializeField] RectTransform slot_selector;
    [SerializeField] Sprite grass_icon;
    [SerializeField] Sprite dirt_icon;
    [SerializeField] Sprite stone_icon;
    [SerializeField] Sprite iron_icon;
    [SerializeField] Sprite gold_icon;
    [SerializeField] Sprite diamond_icon;
    [SerializeField] Sprite snowball_icon;
    [SerializeField] Sprite sword_icon;

    public void enterChest()
    {
        chest_open = true;

        chest_panel.gameObject.SetActive(true);
    }

    public void exitChest()
    {
        chest_open = false;
        in_chest = false;
        slot_selector.gameObject.SetActive(false);

        chest_panel.gameObject.SetActive(false);
    }

    public void addItem(int item_id, int max_stack)
    {
        for (int i = 0; i < item_ids.GetLength(0); i++)
        {
            if (item_id == item_ids[i, 0] && item_ids[i, 1] < max_stack)
            {
                item_ids[i, 1]++;

                GameObject item_count = chest_numbers.GetChild(i).gameObject;

                if (!item_count.activeSelf)
                {
                    item_count.SetActive(true);
                }

                item_count.GetComponent<TMP_Text>().text = item_ids[i, 1].ToString();
                return;
            }
        }


        if (next_empty_slot < item_ids.Length)
        {
            item_ids[next_empty_slot, 0] = item_id;
            item_ids[next_empty_slot, 1]++;
            chest_numbers.GetChild(next_empty_slot).GetComponent<TMP_Text>().text = item_ids[next_empty_slot, 1].ToString();

            updateChest();

            updateNextEmptySlot();
        }
    }

    public void transferItem()
    {
        if (item_ids[selected_slot, 1] > 0)
        {
            int current_ID = item_ids[selected_slot, 0];

            if (InventorySystem.instance.checkIfFreeSpace(current_ID))
            {
                item_ids[selected_slot, 1]--;
                GameObject item_count = chest_numbers.GetChild(selected_slot).gameObject;
                item_count.GetComponent<TMP_Text>().text = item_ids[selected_slot, 1].ToString();

                if (item_ids[selected_slot, 1] == 1)
                {
                    item_count.SetActive(false);
                }
                else if (item_ids[selected_slot, 1] <= 0)
                {
                    item_ids[selected_slot, 0] = 0;
                }


                updateChest();
                updateNextEmptySlot();

                InventorySystem.instance.addItem(current_ID);
            }
            else
            {
                Debug.Log("inventory full");
            }
        }
        else
        {
            InventorySystem.instance.selectItemWarning();
        }
    }

    public void transferAll()
    {
        int item_count = item_ids[selected_slot, 1];

        if (item_count > 0)
        {
            for (int i = 0; i < item_count; i++)
            {
                transferItem();
            }
        }
        else
        {
            InventorySystem.instance.selectItemWarning();
        }

        updateChest();
    }

    void updateChest()
    {
        for (int i = 0; i < item_ids.GetLength(0); i++)
        {
            switch (item_ids[i, 0])
            {
                case 1:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = grass_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 2:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = dirt_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 3:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = stone_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 4:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = iron_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 5:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = gold_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 6:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = diamond_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 7:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = snowball_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                case 8:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = sword_icon;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                default:
                    chest_slots.GetChild(i).GetComponent<Image>().sprite = null;
                    chest_slots.GetChild(i).GetComponent<Image>().enabled = false;
                    break;
            }
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

    public void chestSwitch()
    {
        in_chest = !in_chest;

        if (in_chest)
        {
            slot_selector.gameObject.SetActive(true);
        }
        else
        {
            slot_selector.gameObject.SetActive(false);
        }
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

                if (selected_slot < chest_size - 1)
                {
                    selected_slot++;
                }

                break;
            case 3:
                if (selected_slot > chest_row_length - 1)
                {
                    selected_slot -= chest_row_length;
                }
                break;
            case 4:
                if (selected_slot < chest_size - chest_row_length)
                {
                    selected_slot += chest_row_length;
                }
                break;
        }

        updateSlotSelector();
    }

    void updateSlotSelector()
    {
        slot_selector.position = chest_slots.GetChild(selected_slot).position;
    }

    public bool checkIfFreeSpace(int item_id, int max_stack)
    {
        bool free_space = false;

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
}
