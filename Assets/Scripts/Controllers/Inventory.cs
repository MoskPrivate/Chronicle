using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //
    //
    //Inventory Panels
    //Screens
    public GameObject craftPanel;
    public GameObject defaultPanel;
	public GameObject clothingPanel;

    public CraftingManager craftingManager;

    //Other items
    public GameObject craftingIcon;
    public GameObject craftItem;
    public GameObject crafter;
    public GameObject sideBarPanel;
    public GameObject player;
    public GameObject playerCameraObject;


    public int hotBarSlotAmount;
	//public int inventorySlotAmount;
	//public int totalSlotAmount;

    List<invItem> inventoryList = new List<invItem>();
    public List<invItem> InventoryList
    {
        get
        {
            return inventoryList;
        }
    }
    public GameObject hotBar;
	//public GameObject inventoryBar;
    ItemManager itemManager;

    //
    //
    //Starting function
    void Awake()
    {
        List<invItem> inventoryList = new List<invItem>(hotBarSlotAmount);
        itemManager = FindObjectOfType<ItemManager>();
		//totalSlotAmount = hotBarSlotAmount + inventorySlotAmount;
    }
    void Start()
    {
		for (int i = 0; i < hotBarSlotAmount; i++)
        {
            GameObject itemUI = hotBar.transform.GetChild(0).GetChild(0).gameObject;
			if (hotBar.transform.GetChild(i).GetChild(0).gameObject != null)
			{
				itemUI = hotBar.transform.GetChild(i).GetChild(0).gameObject;
				hotBar.transform.GetChild(i).GetChild(0).GetComponent<InvMoveable>().slot = i;

			}
            invItem item = new invItem(i, -1, 0, itemUI);
            inventoryList.Add(item);

        }


        UpdateGraphics();
    }
    void Update()
    {
        PanelActivationCheck();
    }

    //
    //
    //Panel functions
    void PanelActivationCheck()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PanelActivate();
        }
		if(Input.GetKeyDown(KeyCode.E))
		{
			//ClothingPanelActivate();
		}
        if(Crafter.isPressed)
        {
            sideBarPanel.SetActive(true);
        }
        else
        {
            sideBarPanel.SetActive(false);
        }
    }

    public void PanelActivate()
    {
        Crafter.isPressed = false;
        craftingManager.UpdateCraftingAvailability(0);
        craftPanel.SetActive(!craftPanel.activeSelf);
        defaultPanel.SetActive(!craftPanel.activeSelf);
        if(craftPanel.activeSelf)   
        {
            craftingIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
            craftItem.GetComponent<Image>().color = new Color32(255, 255, 255, 200);

           // player.GetComponent<PlayerController>().idle.SetActive(true);
           // player.GetComponent<PlayerController>().moving.SetActive(false);
           // player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<PlayerMovementController>().enabled = false;
            playerCameraObject.GetComponent<CameraController>().enabled = false;
        }
        else
        {
            craftingIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 60);
            craftItem.GetComponent<Image>().color = new Color32(255, 255, 255, 160);
           // player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<PlayerMovementController>().enabled = true;
            playerCameraObject.GetComponent<CameraController>().enabled = true;
        }
    }

    //
    //
    //Public functions
    public void AddItem(int itemId, int amount)
    {
		int slotThatHasIn = CheckIfHasItem(itemId, amount);
        int stackSize = itemManager.itemList[itemId].stackSize;

        if(amount == 0)
        {
            //Debug.Log("AddItem| AmountToAdd is equal to zero");
            return;
        }
        if(slotThatHasIn == -1)
        {
            //Debug.Log("No Spot found");
            int amountToAdd;
            int amountLeft;
            int emptySlot = FindEmptySlot();
            if(emptySlot == -1)
            {
				return;
                //Debug.LogError("Empty Slot not found");
            }
            if (amount > stackSize)
            {
                amountToAdd = stackSize;
                amountLeft = stackSize - amountToAdd;
            }
            else
            {
                amountToAdd = amount;
                amountLeft = 0;
            }
            inventoryList[emptySlot].itemId = itemId;
            inventoryList[emptySlot].amount = amountToAdd;
			UpdateGraphics();
            if (amountLeft > 0)
            {
                AddItem(itemId, amountLeft);

            }


        }
        else
        {
            //Debug.Log("Adding item to the spot");
            int slot = slotThatHasIn;
            int amountToAdd = 0;
            int amountLeft = 0;
            int amountAlreadyThere = inventoryList[slot].amount;
            int spaceAvailable = stackSize - amountAlreadyThere;

            if (amount <= spaceAvailable)
            {
                amountToAdd = amount;
                amountLeft = 0;
            }
            else
            {
                amountToAdd = spaceAvailable;
                amountLeft = amount - spaceAvailable;
            }

            inventoryList[slot].itemId = itemId;
            inventoryList[slot].amount += amountToAdd;
            UpdateGraphics();

            if (amountLeft > 0)
            {
                AddItem(itemId, amountLeft);

            }
        }
       
    }
    public void SwapSlots(int slot1, int slot2)
    {
        /*int slot1Amount = inventoryList[slot1].amount;
        int slot1ItemId = inventoryList[slot1].itemId;

        int slot2Amount = inventoryList[slot2].amount;
		int slot2ItemId = inventoryList[slot2].itemId;

		
		item1.amount = slot2Amount;
		item2.amount = slot1Amount;*/
		invItem item1 = inventoryList[slot1];
		invItem item2 = inventoryList[slot2];


		/*item1.itemId = inventoryList[slot2].itemId;
		item2.itemId = inventoryList[slot1].itemId;*/

		inventoryList[slot1] = item2;
		inventoryList[slot2] = item1;
        UpdateGraphics();

    }

    //Checking if sufficent resources are available(Crafting)
    public bool CheckAvailableForCraft(int itemId, int amount)
    {
        return false;
    }

    //Removing resources from the inventory(crafting)
    public void RemoveItem(int itemId, int amount)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if(inventoryList[i].itemId == itemId)
            {
                if(amount > inventoryList[i].amount)
                {
                    amount -= inventoryList[i].amount;
                    inventoryList[i].amount = 0;
                    if(amount > 0)
                    {
                        RemoveItem(itemId, amount);
                    }
                }
                else
                {
                    inventoryList[i].amount -= amount;
                    return;
                }
            }
        }
    }

    //For clearing the inventory
    public void Clear()
    {

    }

    //
    //
    //Internal functions
	int CheckIfHasItem(int itemId, int amountToAdd)
    {
        int slotId;
        for (int i = 0; i < hotBarSlotAmount; i++)
        {
            if(inventoryList[i].itemId == itemId)
            {
                if(inventoryList[i].amount == itemManager.itemList[itemId].stackSize)
                {
                    continue;
                }
                else
                {
                    //Debug.Log("AmountInSpot" + inventoryList[i].amount);
                    slotId = i;
                    return slotId;
                }
            }
        }
        //Debug.Log("Didn't find spot");
        return -1;
    }
    int FindEmptySlot()
    {
		
        
        for (int i = 0; i < hotBarSlotAmount; i++)
        {
			if(i == 0)
			{

			}
            if(inventoryList[i].itemId == -1)
            {
                return i;
            }
        }
        return -1;
    }


    void UpdateGraphics()
    {
        for (int i = 0; i < hotBarSlotAmount; i++)
        {
            invItem item = inventoryList[i];
            Image itemImage = item.itemUI.GetComponent<Image>();
            Text itemAmountText = item.itemUI.transform.GetChild(0).gameObject.GetComponent<Text>();
            if(item.amount == 0)
            {
                itemImage.color = new Color32(255, 255, 255, 0);
                itemAmountText.color = new Color32(255, 255, 255, 0);
                itemImage.sprite = null;
                itemImage.GetComponent<InvMoveable>().interactable = false;
				//Debug.Log("interactable = false" + itemImage.GetComponent<InvMoveable>().transform.parent.name);	
            }
            else
            {
                itemImage.color = new Color32(255, 255, 255, 255);
                itemAmountText.color = new Color32(255, 255, 255, 255);
                itemAmountText.text = item.amount.ToString();
                if(item.amount == 1)
                {
                    itemAmountText.text = "";
                }
                if(item.amount == 0)
                {
                    itemAmountText.color = new Color32(255, 255, 255, 0);
                    itemImage.GetComponent<InvMoveable>().interactable = false;
					//Debug.Log("interactable = false" + itemImage.GetComponent<InvMoveable>().transform.parent.name);
                }
                if (itemManager.itemList[item.itemId] != null)
                {
                    itemImage.sprite = itemManager.itemList[item.itemId].itemSprite;
                    itemImage.GetComponent<InvMoveable>().interactable = true;
					//Debug.Log("interactable = true" + itemImage.GetComponent<InvMoveable>().transform.parent.name);
                }
                else
                {
                    itemImage.sprite = null;
                }
            }
        }
    }


}

public class invItem
{
    public int slotId;
    public int itemId;
    public int amount;
    public GameObject itemUI;
    
    public invItem(int _slotId,int _itemId, int _amount, GameObject _itemUI)
    {
        slotId = _slotId;
        itemId = _itemId;
        amount = _amount;
        itemUI = _itemUI;
    }

}
