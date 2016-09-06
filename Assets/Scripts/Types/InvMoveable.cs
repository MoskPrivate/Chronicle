using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvMoveable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform returnParent = null;
    public Transform originalParent = null;
    public int slot;
    public bool interactable = true;
	//Inventory inventory;

    void Start()
    {
        
		//inventory = FindObjectOfType<Inventory>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
		Debug.Log(interactable);
        if (interactable)
        {
            returnParent = this.transform.parent;
			originalParent = this.transform.parent;
            
            this.transform.SetParent(returnParent.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
			Debug.Log("BeginDrag");
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
		
        if (interactable)
        {
			Debug.Log("OnDrag");
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
}

