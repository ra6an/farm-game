using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCraftingItemDetailsPanel : MonoBehaviour
{
    [SerializeField] Text itemName;
    [SerializeField] Image icon;
    [SerializeField] Text quantity;
    [SerializeField] GameObject elementsPanel;

    [SerializeField] Text totalQuantity;

    CraftingRecipe activeRecipe;

    private void Start()
    {
        
        //elementsPanel = transform.Find("ElementsPanel").gameObject;
    }

    internal void SetActiveRecipe(CraftingRecipe recipe, int mltp = 1)
    {
        foreach (Transform go in elementsPanel.transform)
        {
           Destroy(go.gameObject);
        }

        if (recipe == null) return;

        Item item = GameManager.instance.itemsDB.GetItemById(recipe.output.item);

        if(item == null) return;

        itemName.text = item.name;
        icon.sprite = item.icon;
        activeRecipe = recipe;

        elementsPanel.GetComponent<InventoryElementsPanel>().SetElementsDetails(recipe.elements, mltp);
    }

    public void OnClick()
    {
        //GameManager.instance.player.GetComponent<CraftingStationContainerInteractController>().CraftItem(activeRecipe);
    }

    public void OnMultiplierChange(CraftingRecipe recipe, int mltp, bool canChange = true)
    {
        elementsPanel.GetComponent<InventoryElementsPanel>().OnMultiplierChange(mltp, canChange);
    }

    public void OnClickDecrement()
    {
        //GameManager.instance.player.GetComponent<CraftingStationContainerInteractController>().DecrementQuantity(activeRecipe);
    }
}
