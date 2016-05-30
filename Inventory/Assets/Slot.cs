using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public SlotData data;
    public Inventory inventory;

	public void OnDrop(PointerEventData eventData){
		GameObject droppedGameObject = eventData.pointerDrag;
		Item DroppedItem = droppedGameObject.gameObject.GetComponent<Item>();
		if(data.itemData == null){

			//move into tis slot
			DroppedItem.slotData.itemData = null;
			DroppedItem.slotData = data;
		}else{ // the slot is not empty
			/// get thecurrent item that occupies the slot
			GameObject currentItem = data.itemData.gameObject;
			//get the item script attached to that item
			Item curItem = currentItem.GetComponent<Item>();
			//set the items slot to the dropped items slot
			curItem.slotData = DroppedItem.slotData;
			//set the current item to the dropped item
			curItem.transform.SetParent(DroppedItem.slotData.gameObject.transform);

			//set the position to the new parent
			curItem.transform.position = DroppedItem.slotData.gameObject.transform.position;
			//set values inside of dropped item
			//set slot to the new slot
			DroppedItem.slotData = data;

			DroppedItem.transform.SetParent(transform);

			droppedGameObject.transform.position = transform.position;
		}
	}
}
