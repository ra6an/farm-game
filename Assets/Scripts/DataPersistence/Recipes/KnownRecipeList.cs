using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnownRecipe
{
    public int recipeId;
    public CraftingRecipe recipe;
}

[CreateAssetMenu(menuName = "Data/Known Recipe List")]
public class KnownRecipeList : ScriptableObject
{
    public List<KnownRecipe> recipes;
}
