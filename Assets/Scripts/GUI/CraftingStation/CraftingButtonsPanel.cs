using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingButtonsPanel : MonoBehaviour
{
    [SerializeField] GameObject prefabItemButton;

    int currentActive;
    List<CraftingRecipe> recipes;

    private void Start()
    {
        currentActive = 0;
    }

    public void ShowCraftingList(CraftingStationRecipeList recipeList, Transform character)
    {
        currentActive = 0;
        recipes = recipeList.recipes;
        for (int i = 0; i < recipeList.recipes.Count; i++)
        {
            GameObject go = Instantiate(prefabItemButton, transform);
            go.GetComponent<CraftingItemButton>().Set(recipeList.recipes[i]);
            go.GetComponent<CraftingItemButton>().SetIndex(i);
        }

        ChangeActiveButton(currentActive);
        character.GetComponent<CraftingStationContainerInteractController>().ShowItemDetails(recipeList.recipes[currentActive]);
    }
    //
    public void DestroyRecipeButtonsList()
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        recipes = new List<CraftingRecipe>();
    }

    public void ChangeActiveButton(int cur)
    {
        transform.GetChild(currentActive).GetComponent<CraftingItemButton>().InactiveButton();
        currentActive = cur;
        transform.GetChild(currentActive).GetComponent<CraftingItemButton>().ActiveButton();
    }
}
