using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingTabsManager : MonoBehaviour {

    GameObject selectedTabIcon;
    GameObject previouslySelectedTabIcon;

    GameObject selectedPage;
    GameObject previouslySelectedPage;

    int selectedPageNumber;

    List<GameObject> tabsPageList;
    
    void Awake()
    {
        CraftingTabsReferances referances = GetComponent<CraftingTabsReferances>();
        tabsPageList = referances.craftingTabsList;
    }

    public void SelectTab(int number, GameObject tabIcon)
    {
        selectedPageNumber = number;
        //Tab Icons
        if(previouslySelectedTabIcon != null)
        {
            previouslySelectedTabIcon.GetComponent<Image>().color = previouslySelectedTabIcon.GetComponent<InventoryTab>().normalColor;
        }
        selectedTabIcon = tabIcon;
        selectedTabIcon.GetComponent<Image>().color = selectedTabIcon.GetComponent<InventoryTab>().selectedColor;
        previouslySelectedTabIcon = selectedTabIcon;

        //Tab Pages

        if(previouslySelectedPage != null)
        {
            previouslySelectedPage.SetActive(false);
        }
        selectedPage = tabsPageList[number];
        selectedPage.SetActive(true);
        previouslySelectedPage = selectedPage;
    }
    public int GetSelectedTabNumber()
    {
        return selectedPageNumber;
    }
}
