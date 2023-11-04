using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Pickupable : MonoBehaviour, IPickupable
{
    [SerializeField] int item_id;

    /// <summary>
    /// This is where you will want to add your own implementation for your own systems.
    /// </summary>
    public void Pickup()
    {
        if (InventorySystem.instance.addItem(item_id))
        {
            Destroy(gameObject);
        }
    }
}
