using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

    private static ItemManager _itemManager;

    public static ItemManager itemManager
    {
        get;private set;
    }
    List<Item> itemList_;
    public List<Item> itemList = new List<Item>();

    
}
