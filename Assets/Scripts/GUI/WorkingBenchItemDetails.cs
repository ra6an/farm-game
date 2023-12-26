using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
//
public class WorkingBenchItemDetails : MonoBehaviour
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
        itemName.text = recipe.output.item.name;
        icon.sprite = recipe.output.item.icon;
        quantity.text = recipe.output.quantity.ToString();
        activeRecipe = recipe;
    }

    public void OnClick()
    {
        GameManager.instance.player.GetComponent<WorkingBenchContainerInteractController>().CraftItem(activeRecipe);
    }

    public void OnClickIncrement()
    {
        GameManager.instance.player.GetComponent<WorkingBenchContainerInteractController>().IncrementQuantity(activeRecipe);
    }

    public void OnClickDecrement() 
    {
        GameManager.instance.player.GetComponent<WorkingBenchContainerInteractController>().DecrementQuantity(activeRecipe);
    }
}
