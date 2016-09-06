using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class Crafter : MonoBehaviour
{
    public int resultId;
    public List<CraftSetting> itemsNeeded = new List<CraftSetting>();


    GameObject sidebarPanel;
    public static bool isPressed = false;
    ItemManager itemManager;
    Image image;

    void Awake()
    {
        itemManager = FindObjectOfType<ItemManager>();
        image = gameObject.transform.GetChild(0).GetComponent<Image>();
        RetrieveInfo();
    }

    void RetrieveInfo()
    {
        image.sprite = itemManager.itemList[resultId].itemSprite;
    }
}
[System.Serializable]
public class CraftSetting {
    public int itemId;
    public int amount;

    public CraftSetting(int _itemId, int _amount)
    {
        itemId = _itemId;
        amount = _amount;
    }
}
