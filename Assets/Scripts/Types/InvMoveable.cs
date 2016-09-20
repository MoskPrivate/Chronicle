using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class InvMoveable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public Transform returnParent = null;
    public Transform originalParent = null;
    public int slot;
    public bool interactable = true;
    SelectedComponents selectedComp;
    public static bool isBeingSplitted;
    public int splittedAmount;
    public int splittedAmountLeft;
	Inventory inventory;
    GameObject tempInvItem;
    ItemManager itemManager;

    void Start()
    {
        selectedComp = FindObjectOfType<SelectedComponents>();
		inventory = FindObjectOfType<Inventory>();
        itemManager = FindObjectOfType<ItemManager>();
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (interactable)
        {
            returnParent = this.transform.parent;
            originalParent = this.transform.parent;

            this.transform.SetParent(returnParent.parent.parent);
            if (Input.GetMouseButton(1))
            {
                isBeingSplitted = true;
                splittedAmount = inventory.GetAmount(slot)/2;
                splittedAmountLeft = inventory.GetAmount(slot) - splittedAmount;
                inventory.SetAmount(slot, splittedAmountLeft);
                inventory.Split(splittedAmount);
                if(splittedAmount > 0)
                {
                    tempInvItem = (GameObject)Instantiate(inventory.invItemPrefab, originalParent.transform, false);
                    tempInvItem.transform.FindChild("Image").GetComponent<Image>().sprite = itemManager.itemList[inventory.InventoryList[slot].itemId].itemSprite;
                    tempInvItem.transform.FindChild("Image").GetComponent<Image>().color = new Color(255, 255, 255, 255);
                    tempInvItem.transform.FindChild("Text").gameObject.GetComponent<Text>().text = splittedAmount.ToString();
                    if (splittedAmount == 1)
                    {
                        tempInvItem.transform.FindChild("Text").gameObject.GetComponent<Text>().text = "";
                    }
                }
                
            }
            else
            {
                inventory.Split(0);
            }
            

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
		
        if (interactable)
        {
            this.transform.position = eventData.position;
        }
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (interactable)
        {
			
            this.transform.SetParent(returnParent);
            this.transform.position = returnParent.transform.position;

            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        if(returnParent == originalParent && isBeingSplitted)
        {
            inventory.SetAmount(slot, splittedAmount + splittedAmountLeft);
        }
        else
        {

        }
        if(isBeingSplitted && tempInvItem != null)
        {
            Destroy(tempInvItem);
        }
        isBeingSplitted = false;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameObject.transform.parent.gameObject.GetComponent<InvDroppable>() != null)
        {
            InvDroppable slot = gameObject.transform.parent.gameObject.GetComponent<InvDroppable>();
            selectedComp.SelectSlot(slot);
        }
    }
}

