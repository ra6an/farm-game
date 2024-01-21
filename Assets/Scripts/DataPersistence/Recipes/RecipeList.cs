using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/RecipeList")]
[Serializable]
public class RecipeList : ScriptableObject
{
    public List<CraftingRecipe> recipes;

    public void Init()
    {
        recipes = new List<CraftingRecipe>();
    }

    public int GetRecipeId(CraftingRecipe recipe)
    {
        for(int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i] == recipe) return i;
        }
        return -1;
    }

    public CraftingRecipe GetRecipeById(int id)
    {
        return recipes[id];
    }
}
