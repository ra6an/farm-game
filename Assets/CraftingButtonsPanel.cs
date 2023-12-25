using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingButtonsPanel : MonoBehaviour
{
    [SerializeField] GameObject prefabItemButton;
    //[SerializeField] WorkingBenchRecipeList recipeList;

    int currentActive;

    private void Start()
    {
        currentActive = 0;
    }

    public void ShowCraftingList(WorkingBenchRecipeList recipeList, Transform character)
    {
        currentActive = 0;
        for (int i = 0; i < recipeList.recipes.Count; i++)
        {
            GameObject go = Instantiate(prefabItemButton, transform);
            go.GetComponent<CraftingItemButton>().Set(recipeList.recipes[i]);
            go.GetComponent<CraftingItemButton>().SetIndex(i);
        }
        transform.GetChild(currentActive).GetComponent<CraftingItemButton>().ActiveButton();
        character.GetComponent<WorkingBenchContainerInteractController>().ShowItemDetails(recipeList.recipes[0]);
    }

    public void ChangeActiveButton(int cur)
    {
        transform.GetChild(currentActive).GetComponent<CraftingItemButton>().InactiveButton();
        currentActive = cur;
        transform.GetChild(currentActive).GetComponent<CraftingItemButton>().ActiveButton();
    }
}
