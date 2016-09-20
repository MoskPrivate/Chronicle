using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InvMoveable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public Transform returnParent = null;
    public Transform originalParent = null;
    public int slot;
    public bool interactable = true;
    SelectedComponents selectedComp;
	//Inventory inventory;

    void Start()
    {
        selectedComp = FindObjectOfType<SelectedComponents>();
		//inventory = FindObjectOfType<Inventory>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (interactable)
        {
            returnParent = this.transform.parent;
			originalParent = this.transform.parent;
            
            this.transform.SetParent(returnParent.parent.parent);

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

