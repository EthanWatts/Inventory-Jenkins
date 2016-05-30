using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IEndDragHandler, IPointerUpHandler	
{
    public ItemData data;
    public SlotData slotData;
    public int amount = 1;
	private Text stackAmount;
	private Transform originalSlot;
	private Vector3 offset;
	private CanvasGroup canvasGroup;

	// Use this for initialization
	void Start ()
    {
		stackAmount = gameObject.GetComponentInChildren<Text>();
		canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(stackAmount != null){
			stackAmount.text = amount.ToString();
		}
	}

	public void OnBeginDrag(PointerEventData eventData){
		//check if it is in the slot
		if(data != null){
			//store the original parent slot 
			originalSlot = transform.parent;
			//set the parent of this to the slotpanel
			transform.SetParent(originalSlot.parent);

			canvasGroup.blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData eventData){
		//check if it is in the slot
		if(data != null){
			//set the position of the item to the event data' position
			transform.position = eventData.position -(Vector2)offset;
		}
	}

	public void OnPointerDown(PointerEventData eventData){
		if(data != null){
			offset = (Vector3)eventData.position - transform.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData){
		
		transform.SetParent(slotData.gameObject.transform);
		transform.position = slotData.gameObject.transform.position;
		canvasGroup.blocksRaycasts = true;
	}

	public void OnPointerUp(PointerEventData eventData){

	}
}
