using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WorkingBenchItemDetails : MonoBehaviour
{
    [SerializeField] Text itemName;
    [SerializeField] Image icon;
    GameObject elementsPanel;

    private void Start()
    {
        elementsPanel = transform.Find("ElementsPanel").gameObject;
    }

    internal void SetActiveRecipe(CraftingRecipe recipe)
    {
        if (elementsPanel == null) return;
        if(recipe == null) return;
        itemName.text = recipe.output.item.name;
        icon.sprite = recipe.output.item.icon;
    }
}
