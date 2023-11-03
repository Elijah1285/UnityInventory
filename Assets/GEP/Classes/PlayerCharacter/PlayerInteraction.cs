using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        IPickupable pickupable = collision.gameObject.GetComponent<IPickupable>();
        if (pickupable != null)
        {
            pickupable.Pickup();
        }

        GameObject collision_object = collision.gameObject;

        if (collision_object.tag == "Chest")
        {
            collision_object.GetComponent<Chest>().toggleChest();
            InventorySystem.instance.toggleChest(collision_object.GetComponent<Chest>());
        }
    }
}
