
using UnityEngine;

public class ItemPickup : Interactable
{

    public Item item;


    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {

        Debug.Log("Picking up " + item.name); //shows when an item is picked up
        bool wasPickedUp = Inventory.instance.Add(item);


       if (wasPickedUp) //if the item was picked up and added to the inventory, delete object from the scene
            Destroy(gameObject);

    }

}
