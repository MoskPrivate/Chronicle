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
            if (transform.childCount > 1)
            {
                
                my = this.transform.FindChild("ItemImager").GetComponent<InvMoveable>();

                if (InvMoveable.isBeingSplitted == false)
                {
                    if(inventory.InventoryList[my.slot].itemId == inventory.InventoryList[d.slot].itemId)
                    {
                        inventory.Stack(my.slot, d.slot);
                    }
                    my.transform.SetParent(d.originalParent.transform, false);
                    d.returnParent = this.transform;
                    d.transform.SetParent(d.returnParent, false);

                    inventory.SwapSlots(my.slot, d.slot);

                    temp = my.slot;
                    my.slot = d.slot;

                    d.slot = temp;
                }
                else
                {
                    if(inventory.InventoryList[my.slot].itemId == -1 || inventory.InventoryList[d.slot].itemId == -1)
                    {
                        my.transform.SetParent(d.originalParent.transform, false);
                        d.returnParent = this.transform;
                        d.transform.SetParent(d.returnParent, false);

                        inventory.SwapSlots(my.slot, d.slot);

                        temp = my.slot;
                        my.slot = d.slot;

                        d.slot = temp;
                    }
                    else
                    {
                        inventory.InventoryList[d.slot].amount = d.splittedAmount + d.splittedAmountLeft;
                    }
                    
                }
                

            }
            
        }
    }
}
