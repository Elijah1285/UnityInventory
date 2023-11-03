using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    const int row_length = 9;
    int next_empty_slot = 0;
    int selected_slot = 0;
    bool chest_open = false;
    int[,] item_ids = new int[18, 2];

    [SerializeField] int max_stack;
    [SerializeField] Canvas chest_panel;
    [SerializeField] RectTransform chest_numbers;
    [SerializeField] RectTransform chest_slots;
    [SerializeField] Sprite grass_icon;
    [SerializeField] Sprite dirt_icon;
    [SerializeField] Sprite stone_icon;
    [SerializeField] Sprite iron_icon;
    [SerializeField] Sprite gold_icon;
    [SerializeField] Sprite diamond_icon;

    public void toggleChest()
    {
        chest_open = !chest_open;

        if (chest_open)
        {
            chest_panel.gameObject.SetActive(true);
        }
        else
        {
            chest_panel.gameObject.SetActive(false);
        }
    }

    public bool addItem(int item_id)
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
                return true;
            }
        }


        if (next_empty_slot < item_ids.Length)
        {
            item_ids[next_empty_slot, 0] = item_id;
            item_ids[next_empty_slot, 1]++;
            chest_numbers.GetChild(next_empty_slot).GetComponent<TMP_Text>().text = item_ids[next_empty_slot, 1].ToString();

            updateChest();

            updateNextEmptySlot();

            return true;
        }

        return false;
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
}
