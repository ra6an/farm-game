using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
//
public class CraftingStationItemDetails : MonoBehaviour
{
    [SerializeField] Text itemName;
    [SerializeField] Image icon;
    [SerializeField] Text quantity;
    GameObject elementsPanel;

    [SerializeField] Text totalQuantity;

    CraftingRecipe activeRecipe;

    private void Start()
    {
        elementsPanel = transform.Find("ElementsPanel").gameObject;
    }

    internal void SetActiveRecipe(CraftingRecipe recipe)
    {
        if(recipe == null) return;

        Item item = GameManager.instance.itemsDB.GetItemById(recipe.output.item);

        if(item == null) return;

        itemName.text = item.name;
        icon.sprite = item.icon;
        quantity.text = recipe.output.quantity.ToString();
        activeRecipe = recipe;
    }

    public void OnClick()
    {
        GameManager.instance.player.GetComponent<CraftingStationContainerInteractController>().CraftItem(activeRecipe);
    }

    public void OnClickIncrement()
    {
        GameManager.instance.player.GetComponent<CraftingStationContainerInteractController>().IncrementQuantity(activeRecipe);
    }

    public void OnClickDecrement() 
    {
        GameManager.instance.player.GetComponent<CraftingStationContainerInteractController>().DecrementQuantity(activeRecipe);
    }
}
