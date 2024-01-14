using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//
public class RecipePanel : ItemPanel
{
    [SerializeField] RecipeList recipeList;
    [SerializeField] Crafting crafting;
    [SerializeField] GameObject recipeContainer;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject itemDetails;
    [SerializeField] Text craftQuantity;
    public int currentId = 0;
    public int multiplier = 1;

    bool setActiveRecipe = false;

    private void Update()
    {
        if (setActiveRecipe)
        {
            SetActiveRecipe(currentId);
            setActiveRecipe = false;
        }
    }

    public override void Show()
    {
        currentId = 0;

        foreach (Transform t in recipeContainer.transform)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < recipeList.recipes.Count; i++)
        {
            GameObject go = Instantiate(buttonPrefab, transform);
            go.GetComponent<CraftingItemInventoryButton>().Set(recipeList.recipes[i]);
            go.GetComponent<CraftingItemInventoryButton>().SetIndex(i);

            go.transform.SetParent(recipeContainer.transform);
        }

        setActiveRecipe = true;
    }

    public void SetActiveRecipe(int id)
    {
        recipeContainer.transform.GetChild(currentId).GetComponent<CraftingItemInventoryButton>().InactiveButton();
        currentId = id;
        recipeContainer.transform.GetChild(currentId).GetComponent<CraftingItemInventoryButton>().ActiveButton();
        itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().SetActiveRecipe(recipeList.recipes[currentId]);

        if (!CheckInventoryQuantity(recipeList.recipes[currentId], 1))
        {
            craftQuantity.text = "0";
        } else
        {
            craftQuantity.text = "1";
        }
    }

    public bool CheckInventoryQuantity(CraftingRecipe recipe, int mltp)
    {
        bool canCraft = true;
        foreach (ItemSlot itemSlot in recipe.elements)
        {
            ItemSlot inventorySlot = GameManager.instance.inventoryContainer.GetItemSlot(itemSlot.item);
            if (inventorySlot == null || inventorySlot.quantity < itemSlot.quantity * mltp) canCraft = false;
            if (!canCraft) break;
        }
        return canCraft;
    }

    public void OnClickIncrementMultiplier()
    {
        bool check = CheckInventoryQuantity(recipeList.recipes[currentId], multiplier + 1);
        if (check)
        {
            int invFreeSpace = 1;
            if (!recipeList.recipes[currentId].output.item.stackable)
            {
                invFreeSpace = GameManager.instance.inventoryContainer.CheckHowManyFreeSlots();
                Debug.Log(invFreeSpace);
            }

            if (invFreeSpace <= multiplier) return;

            multiplier++;
            itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().OnMultiplierChange(recipeList.recipes[currentId], multiplier, check);
            craftQuantity.text = multiplier.ToString();
        } else
        {
            itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().OnMultiplierChange(recipeList.recipes[currentId], multiplier, check);
        }
    }

    public void OnClickDecrementMultiplier()
    {
        if (!CheckInventoryQuantity(recipeList.recipes[currentId], multiplier)) return;
        if (multiplier - 1 <= 0) return;

        multiplier--;
        itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().OnMultiplierChange(recipeList.recipes[currentId], multiplier, true);
        craftQuantity.text = multiplier.ToString();
    }

    public void OnCraftItem()
    {
        CraftingRecipe currRecipe = recipeList.recipes[currentId];

        if (!GameManager.instance.inventoryContainer.CheckFreeSpace()) return;

        if (!CheckInventoryQuantity(currRecipe, multiplier)) return;

        if(currRecipe.output.item.stackable)
        {
            CraftItem(currRecipe, multiplier);
        }
        else
        {
            for (int i = 0; i < multiplier; i++)
            {
                if (!GameManager.instance.inventoryContainer.CheckFreeSpace()) break;
                CraftItem(currRecipe);
            }
        }

        bool afterCraftItemQuantity = CheckInventoryQuantity(currRecipe, 1);

        multiplier = afterCraftItemQuantity ? 1 : 0;
        craftQuantity.text = multiplier.ToString();
        itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().SetActiveRecipe(recipeList.recipes[currentId]);
    }

    public void CraftItem(CraftingRecipe rec, int mltp = 1)
    {
        foreach (ItemSlot iSlot in rec.elements)
        {
            GameManager.instance.inventoryContainer.Remove(iSlot.item, iSlot.quantity * mltp);
        }

        GameManager.instance.inventoryContainer.Add(rec.output.item, rec.output.quantity * mltp);
    }

    public override void OnClick(int id)
    {
        if (id >= recipeList.recipes.Count) return;
    }
}
