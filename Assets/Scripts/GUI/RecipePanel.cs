using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePanel : ItemPanel
{
    [SerializeField] RecipeList recipeList;
    [SerializeField] Crafting crafting;
    [SerializeField] GameObject recipeContainer;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject itemDetails;
    public int currentId;

    public override void Show()
    {
        foreach(Transform t in recipeContainer.transform)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < recipeList.recipes.Count; i++)
        {
            GameObject go = Instantiate(buttonPrefab, transform);
            go.GetComponent<CraftingItemButton>().Set(recipeList.recipes[i]);
            go.GetComponent<CraftingItemButton>().SetIndex(i);

            go.transform.SetParent(recipeContainer.transform);
            currentId = i;
        }
        //for(int i = 0; i < buttons.Count && i < recipeList.recipes.Count; i++)
        //{
        //    buttons[i].Set(recipeList.recipes[i].output);
        //}
    }

    public override void OnClick(int id)
    {
        if (id >= recipeList.recipes.Count) return;

        itemDetails.GetComponent<WorkingBenchItemDetails>().SetActiveRecipe(recipeList.recipes[id]);
        //crafting.Craft(recipeList.recipes[id]);
    }
}
