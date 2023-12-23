using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] ItemContainer inventory;

    public void Craft(CraftingRecipe recipe)
    {
        if (!inventory.CheckFreeSpace())
        {
            Debug.Log("Not enough space to fit the item after crafting!");
            return;
        }

        for (int i = 0; i < recipe.elements.Count; i++)
        {
            if(!inventory.CheckItem(recipe.elements[i]))
            {
                Debug.Log("Crafting recipe elements are not present in the inventory!");
                return;
            }
        }

        for(int i = 0;i < recipe.elements.Count;i++)
        {
            inventory.Remove(recipe.elements[i].item, recipe.elements[i].quantity);
        }

        inventory.Add(recipe.output.item, recipe.output.quantity);
    }
}
