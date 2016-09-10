using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour {

    public Inventory inventory;
    List<GameObject> pages;
    public GameObject craftButton;
    public Color craftButtonColorGreen;
    public Color craftButtonColorRed;

    public static Crafter currentCrafter;
    bool canCraft;

    void Awake()
    {
        CraftingTabsReferances referances = GetComponent<CraftingTabsReferances>();
        pages = referances.craftingTabsList;
    }

    public void UpdateCraftingAvailability(int pageNumber)
    {
        canCraft = false;
        int crafterAmount = pages[pageNumber].transform.childCount;
        for (int i = 0; i < crafterAmount; i++)
        {
            if(pages[pageNumber].transform.GetChild(i).GetComponent<Crafter>() != null)
            {
                if (CheckForResources(pages[pageNumber].transform.GetChild(i).GetComponent<Crafter>()) == true)
                {
                    craftButton.GetComponent<Image>().color = craftButtonColorGreen;
                    canCraft = true;
                }
                else
                {
                    craftButton.GetComponent<Image>().color = craftButtonColorRed;
                    canCraft = false;
                }
            }
           
        }
        
    }
    bool CheckForResources(Crafter crafter)
    {
        for (int i = 0; i < crafter.itemsNeeded.Count; i++)
        {
            int itemId = crafter.itemsNeeded[i].itemId;
            int amount = crafter.itemsNeeded[i].amount;

            int amountInInventory = 0;
            for (int j = 0; j < inventory.hotBarSlotAmount; j++)
            {
                if (inventory.InventoryList[j].itemId == itemId)
                {
                    amountInInventory += inventory.InventoryList[j].amount;
                }
            }
            //Debug.Log(amountInInventory + " " + itemId);
            if(amountInInventory >= amount)
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        
        return true;
    }
    public void CraftItem()
    {
        
        Crafter crafter = currentCrafter;
        if(canCraft && crafter != null)
        {
            
            for (int i = 0; i < crafter.itemsNeeded.Count; i++)
            {
                inventory.RemoveItem(crafter.itemsNeeded[i].itemId, crafter.itemsNeeded[i].amount);
                
            }
            inventory.AddItem(crafter.resultId, crafter.resultAmount);
        }
        UpdateCraftingAvailability(0);
    }
}
