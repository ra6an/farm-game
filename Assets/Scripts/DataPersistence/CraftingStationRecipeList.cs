using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Working Bench Recipes")]
public class CraftingStationRecipeList : ScriptableObject
{
    public List<CraftingRecipe> recipes;
}
