﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CrafterSelect : MonoBehaviour {

    [Header("Crafter List")]
    public List<Crafter> crafterList;
    public Crafter selectedCrafter;
    public Crafter previouslySelectedCrafter;


    [Header("Other Items")]
    public ItemManager itemManager;
    public Inventory inventory;
    public Image sideBarImage;
    public Text sideBarTitle;
    public Text sideBarDescription;
    public GameObject craftNeedElement;
    public GameObject craftResourcesPanel;
    public GameObject craftButton;
    public Color craftButtonColorGreen;
    public Color craftButtonColorRed;
    public Color originalCrafterColor;
    


    int crafterResultId;


    public void OnClick(Crafter _crafter)
    {
        selectedCrafter = _crafter;
        if(selectedCrafter != null)
        {
            Crafter.isPressed = true;
        }
        UpdateGraphicsSideBar(_crafter);
        UpdateCrafterGraphics();
    }
    public void UpdateCrafterGraphics()
    {
        
        if (selectedCrafter != null)
        {
            Image crafterImage = selectedCrafter.GetComponent<Image>();
            if(crafterImage != null)
            {
                crafterImage.color = craftButtonColorRed;
                //crafterImage.GetComponent<Button>().transition = Selectable.Transition.None;
            }
            if (previouslySelectedCrafter != null && selectedCrafter != previouslySelectedCrafter)
            {
                previouslySelectedCrafter.GetComponent<Image>().color = originalCrafterColor;
                //previouslySelectedCrafter.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
            }
        }

        previouslySelectedCrafter = selectedCrafter;
    }
    public void ResetCrafterGraphics()
    {
        if (previouslySelectedCrafter != null)
        {
            previouslySelectedCrafter.GetComponent<Image>().color = originalCrafterColor;
            //previouslySelectedCrafter.GetComponent<Button>().transition = Selectable.Transition.ColorTint;

        }
    }
    public void UpdateGraphicsSideBar(Crafter _crafter)
    {
        if (_crafter.canCraft)
        {
            craftButton.GetComponent<Image>().color = craftButtonColorGreen;
        }
        else
        {
            craftButton.GetComponent<Image>().color = craftButtonColorRed;
        }
        CraftingManager.currentCrafter = _crafter;
        crafterResultId = selectedCrafter.resultId;
        sideBarTitle.text = itemManager.itemList[crafterResultId].name;
        sideBarDescription.text = itemManager.itemList[crafterResultId].description;
        sideBarImage.sprite = itemManager.itemList[crafterResultId].itemSprite;
        int itemNeededCount = selectedCrafter.itemsNeeded.Count;

        int childCount = craftResourcesPanel.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(craftResourcesPanel.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < itemNeededCount; i++)
        {
            
            GameObject content = (GameObject)Instantiate(craftNeedElement,craftResourcesPanel.transform.position,Quaternion.identity);
            content.transform.SetParent(craftResourcesPanel.transform,false);
            NeedItem needItem = content.GetComponent<NeedItem>();
            needItem.resourceImage.sprite = itemManager.itemList[_crafter.itemsNeeded[i].itemId].itemSprite;
            needItem.resourceName.text = itemManager.itemList[_crafter.itemsNeeded[i].itemId].name;
            needItem.need.text = _crafter.itemsNeeded[i].amount.ToString();

        }
        
        

    }
}
