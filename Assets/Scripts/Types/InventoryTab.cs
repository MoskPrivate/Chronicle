using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryTab : MonoBehaviour , IPointerClickHandler {

    public Color normalColor;
    public Color selectedColor;
    public int pageNumber;
    public string tabName;

    public Text tabNameText;
    CraftingTabsManager tabsManager;
    CraftingManager craftingManager;
    CraftingTabsReferances craftingTabsReferances;
    CrafterSelect crafterSelect;

    void Awake()
    {
        crafterSelect = FindObjectOfType<CrafterSelect>();
        tabNameText = GameObject.FindGameObjectWithTag("TabName").GetComponent<Text>();
        tabsManager = FindObjectOfType<CraftingTabsManager>();
        craftingManager = tabsManager.GetComponent<CraftingManager>();
        craftingTabsReferances = tabsManager.GetComponent<CraftingTabsReferances>();
        if (pageNumber == 0)
        {
            tabsManager.SelectTab(pageNumber, this.gameObject);
        }
        if(tabName == "All")
        {
            CreateTheAllPage();
        }
        craftingManager.UpdateCraftingAvailability(0);
        if(crafterSelect.selectedCrafter != null)
        {
            
            
            crafterSelect.UpdateGraphicsSideBar(crafterSelect.selectedCrafter);
            
        }
    }

    void CreateTheAllPage()
    {
        for (int i = 0; i < craftingTabsReferances.craftingTabsList.Count; i++)
        {
            for (int j = 0; j < craftingTabsReferances.craftingTabsList[i].transform.childCount; j++)
            {
                GameObject instCrafter = Instantiate(craftingTabsReferances.craftingTabsList[i].transform.GetChild(j).gameObject);
                instCrafter.transform.SetParent(craftingTabsReferances.craftingTabsList[0].transform,true);
                instCrafter.transform.localScale = Vector2.one;
                
            }
            
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        crafterSelect.ResetCrafterGraphics();
        craftingManager.UpdateCraftingAvailability(pageNumber);
        tabsManager.SelectTab(pageNumber, this.gameObject);
        Crafter.isPressed = false;
        tabNameText.text = tabName;
        
    }
}
