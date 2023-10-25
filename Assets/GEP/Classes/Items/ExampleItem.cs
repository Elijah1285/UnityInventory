using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ExampleItem : MonoBehaviour, IPickupable
{
    int item_id = 1;

    /// <summary>
    /// This is where you will want to add your own implementation for your own systems.
    /// </summary>
    public void Pickup()
    {
        InventorySystem.instance.addItem(item_id);
        Destroy(gameObject);
    }
}
