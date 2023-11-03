using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRadius : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InventorySystem inventory_system = InventorySystem.instance;

            if (inventory_system.getChestOpen())
            {
                inventory_system.exitChest();
            }
        }
    }
}
