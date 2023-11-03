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

    [SerializeField] int max_stack;
    [SerializeField] int[,] item_ids = new int[18, 2];
    [SerializeField] Canvas chest_panel;
    [SerializeField] RectTransform chest_numbers;

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

    bool addItem(int item_id)
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

    }

    void updateNextEmptySlot()
    {

    }
}
