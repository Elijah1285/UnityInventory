using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    [SerializeField] int capacity;
    const int row_length = 9;
    bool chest_open = false;

    [SerializeField] Canvas chest_panel;

    public void toggleChest()
    {
        chest_open = !chest_open;
    }
}
