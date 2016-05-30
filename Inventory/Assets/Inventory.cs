using UnityEngine;
using System.Collections;

// Use generic lists
using System.Collections.Generic;

// Use Unity's UI
using UnityEngine.UI;

// Create a slot class to store it's gameobject 
// and item within
public class SlotData
{
    public GameObject gameObject;
    public ItemData itemData;

    public SlotData(GameObject gameObject, ItemData itemData)
    {
        this.gameObject = gameObject;
        this.itemData = itemData;
    }
}

[RequireComponent(typeof(ItemDatabase))]
public class Inventory : MonoBehaviour
{
    // Public:
    [Header("UI")]
    public int slotAmount = 20;
    public GameObject slotPanel;
    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    [Header("Items / Slots")]
    public List<ItemData> items = new List<ItemData>();
    public List<SlotData> slots = new List<SlotData>();

    // Private:
    private ItemDatabase database;

    // Use this for initialization
    void Start ()
    {
        // Get the item database
        database = GetComponent<ItemDatabase>();

        // Loop through by the slot amount
        for (int i = 0; i < slotAmount; i++)
        {
            // Create all the slots under slotPanel
            GameObject clone = Instantiate(slotPrefab);
            clone.transform.SetParent(slotPanel.transform);

            // Create a new empty slots for our inventory
            SlotData slotData = new SlotData(clone, null);
            
            // Get slot component from list
            Slot slot = clone.GetComponent<Slot>();
            slot.inventory = this;
            slot.data = slotData;

            // Add slot to list
            slots.Add(slotData);
        }
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            AddItemById(0, 1);
        }
	}

    public void AddItemById(int id, int itemAmount = 1)
    {
        // Get an item from database by id
        ItemData newItem = database.GetItemById(id);
        // Get an empty slot in our inventory
        SlotData newSlot = GetEmptySlot();

        // Check if newItem AND newSlot is NOT null
        if(newItem != null && newSlot != null)
        {
			//check if it can be stacked
			if(HasStacked(newItem, itemAmount)){
				return;
			}

            // Set the empty slot
            newSlot.itemData = newItem;

            // Create a new item instance
            GameObject itemGameObject = Instantiate(itemPrefab);

            // Set the item to be in the same position as slot
            itemGameObject.transform.position = newSlot.gameObject.transform.position;

            // Set it's parent in the hierarchy
            itemGameObject.transform.SetParent(newSlot.gameObject.transform);
            itemGameObject.name = newItem.Title;

            // Set the item's gameobject
            newItem.gameObject = itemGameObject;

            // Get the image component of that item and set it
            Image image = itemGameObject.GetComponent<Image>();
            image.sprite = newItem.sprite;

            // Get the item component and set it's data
            Item item = itemGameObject.GetComponent<Item>();
            item.data = newItem;
            item.slotData = newSlot;
        }
    }

    public SlotData GetEmptySlot()
    {
        // Loop through all of our slots
        for (int i = 0; i < slots.Count; i++)
        {
            // Check if the item inside is null
            if (slots[i].itemData == null)
            {
                // Return that slot
                return slots[i];
            }
        }
        // Otherwise, no empty slots were found
        print("No empty slots found!");
        return null;
    }

	bool HasStacked(ItemData itemToADD, int itemAmount = 1){
		if(itemToADD.Stackable){
			SlotData occupiedSlot = GetSlotDataWithItemData(itemToADD);
			if(occupiedSlot != null){
				ItemData itemData = occupiedSlot.itemData;
				Item item = itemData.gameObject.GetComponent<Item>();
				item.amount += itemAmount;
				return true;
			}
		}
		return false;
	}

	SlotData GetSlotDataWithItemData(ItemData item){
		for (int i = 0; i < slots.Count; i++) {
			ItemData currentItem = slots[i].itemData;
			if(currentItem != null && currentItem.Id == item.Id){
				return slots[i];
			}
		}

		return null;
	}
}
