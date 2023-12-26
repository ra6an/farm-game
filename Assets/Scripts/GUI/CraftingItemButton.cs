using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject itemDetailsPanel;
    [SerializeField] Image icon;
    [SerializeField] Sprite activeButton;
    [SerializeField] Sprite inactiveButton;

    CraftingRecipe recipe;
    int myIndex;

    public void SetIndex(int index) 
    {
        myIndex = index;
    }

    public void Set(CraftingRecipe rec)
    {
        if(recipe != null) { return; }
        recipe = rec;
        icon.sprite = rec.output.item.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject character = GameObject.Find("MainCharacter").gameObject;
        character.GetComponent<WorkingBenchContainerInteractController>().ShowItemDetails(recipe);
        transform.parent.GetComponent<CraftingButtonsPanel>().ChangeActiveButton(myIndex);
    }

    public void ActiveButton()
    {
        transform.GetComponent<Image>().sprite = inactiveButton;
    }

    public void InactiveButton()
    {
        transform.GetComponent<Image>().sprite = activeButton;
    }
}
