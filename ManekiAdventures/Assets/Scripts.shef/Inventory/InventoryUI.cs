using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public Transform itemsParent;
    public GameObject inventoryUI;
    public GameObject craftingUI;
    public GameObject craftingObject;

    Inventory inventory;

    public InventorySlot[] slots; //creates array of possible items

    InventorySlot[] crafting;


    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //in Unity item manager, used "i", "b", to be defined as "Inventory"
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //craftingUI.SetActive(!craftingUI.activeSelf);
            GameObject.Find("CraftButton").GetComponent<OpenWindow>().WindowOpener();
            if (craftingObject.activeSelf == true)
            {
                craftingObject.GetComponent<PotionCreation>().returnObjectsToInventory();
            }
            craftingObject.SetActive(!craftingObject.activeSelf);
        }
    }

    void UpdateUI() //updates the inventory UI
    {

       for(int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }

            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    void UpdateCraftingUI()
    {
        for (int i = 0; i < crafting.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                crafting[i].AddItem(inventory.items[i]);
            }
            else
            {
                crafting[i].ClearSlot();
            }
        }
    }
}
