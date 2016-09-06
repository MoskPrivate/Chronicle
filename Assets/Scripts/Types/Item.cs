using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
    public enum itemType { Item, Tool, Other };

    public int id;
    public string name;
    public itemType type;
    public Sprite itemSprite;
    public int stackSize;
    public string description;

    //////CONSTRUCTOR//////
    public Item(int _id, string _name, itemType _type, Sprite _itemSprite, int _stackSize)
    {
        id = _id;
        name = _name;
        type = _type;
        itemSprite = _itemSprite;
        stackSize = _stackSize;
    }
}


[System.Serializable]
public class craftItem
{
    public int itemId;
    public int amount;

    //////CONSTRUCTOR//////
    public craftItem(int _itemId, int _amount)
    {
        itemId = _itemId;
        amount = _amount;
    }
}
