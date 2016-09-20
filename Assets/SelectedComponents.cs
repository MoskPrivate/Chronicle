using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedComponents : MonoBehaviour {


    public InvDroppable firstSlot;

    InvDroppable selectedSlot;
    InvDroppable lastSelectedSlot;

    [Header("Colors")]
    public Color selectedSlotColor;
    public Color defaultSlotColor;

    void Awake()
    {
        SelectSlot(firstSlot);
    }

    public void SelectSlot(InvDroppable slot)
    {
        selectedSlot = slot;
        selectedSlot.GetComponent<Image>().color = selectedSlotColor;
        if(lastSelectedSlot != null && lastSelectedSlot != selectedSlot)
        {
            lastSelectedSlot.GetComponent<Image>().color = defaultSlotColor;
        }
        lastSelectedSlot = selectedSlot;
    }
}
