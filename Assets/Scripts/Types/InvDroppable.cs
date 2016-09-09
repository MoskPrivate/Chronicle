using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InvDroppable : MonoBehaviour ,IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	Inventory inventory;

    void Awake()
    {
		inventory = FindObjectOfType<Inventory>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
		
    }
    public void OnPointerExit(PointerEventData eventData)
    {
		
    }

    public void OnDrop(PointerEventData eventData)
    {
        InvMoveable d = eventData.pointerDrag.GetComponent<InvMoveable>();
		InvMoveable my;
		int dSlot = d.slot;
		int temp;

		if(d != null && d.interactable)
        {
            if (transform.childCount > 0)
            {
				my = this.transform.FindChild("ItemImage").GetComponent<InvMoveable>();
                //Debug.Log("Swap : " + my.slot + " with " + d.slot);
                //this.transform.GetChild(0).SetParent(d.returnParent,false);

                my.transform.SetParent(d.originalParent.transform,false);
				d.returnParent = this.transform;
				d.transform.SetParent(d.returnParent,false);


				/*d.transform.SetParent(d.returnParent);
				d.transform.position = d.returnParent.transform.position;
				d.GetComponent<CanvasGroup>().blocksRaycasts = true;*/
				inventory.SwapSlots(my.slot,d.slot);
				temp = my.slot;
				my.slot = d.slot;

				d.slot = temp;

            }
            
        }
    }
}
