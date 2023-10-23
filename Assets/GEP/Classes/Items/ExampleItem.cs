using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ExampleItem : MonoBehaviour, IPickupable
{
    int item_ID = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This is where you will want to add your own implementation for your own systems.
    /// </summary>
    public void Pickup()
    {
        InventorySystem.instance.addItem(item_ID);
        Destroy(gameObject);
    }
}
