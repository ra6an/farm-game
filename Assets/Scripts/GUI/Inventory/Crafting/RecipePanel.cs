using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//
public class RecipePanel : ItemPanel
{
    //private RecipeList recipeList;
    [SerializeField] Crafting crafting;
    [SerializeField] GameObject recipeContainer;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject itemDetails;
    [SerializeField] Text craftQuantity;
    public int currentId = 0;
    public int multiplier = 1;

    private void Start()
    {
        //recipeList = (RecipeList)ScriptableObject.CreateInstance(typeof(RecipeList));
        //recipeList.Init();
        //recipeList.recipes = GameManager.instance.player.GetComponent<Character>().characterRecipeList.recipes;
    }

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

        for (int i = 0; i < crafting.recipeList.recipes.Count; i++)
        {
            GameObject go = Instantiate(buttonPrefab, transform);
            go.GetComponent<CraftingItemInventoryButton>().Set(crafting.recipeList.recipes[i]);
            go.GetComponent<CraftingItemInventoryButton>().SetIndex(i);

            go.transform.SetParent(recipeContainer.transform);
        }

        setActiveRecipe = true;
    }

    public void SetActiveRecipe(int id)
    {
        recipeContainer.transform.GetChild(currentId).GetComponent<CraftingItemInventoryButton>().InactiveButton();
        currentId = id;
        multiplier = 1;
        recipeContainer.transform.GetChild(currentId).GetComponent<CraftingItemInventoryButton>().ActiveButton();
        itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().SetActiveRecipe(crafting.recipeList.recipes[currentId]);

        if (!CheckInventoryQuantity(crafting.recipeList.recipes[currentId], 1))
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
        bool check = CheckInventoryQuantity(crafting.recipeList.recipes[currentId], multiplier + 1);
        if (check)
        {
            Item item = GameManager.instance.itemsDB.GetItemById(crafting.recipeList.recipes[currentId].output.item);
            if (item == null) return;

            int invFreeSpace = 1;
            if (!item.stackable)
            {
                invFreeSpace = GameManager.instance.inventoryContainer.CheckHowManyFreeSlots();
                //Debug.Log(invFreeSpace);
            }

            if (invFreeSpace <= multiplier) return;

            multiplier++;
            itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().OnMultiplierChange(crafting.recipeList.recipes[currentId], multiplier, check);
            craftQuantity.text = multiplier.ToString();
        } else
        {
            itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().OnMultiplierChange(crafting.recipeList.recipes[currentId], multiplier, check);
        }
    }

    public void OnClickDecrementMultiplier()
    {
        if (!CheckInventoryQuantity(crafting.recipeList.recipes[currentId], multiplier)) return;
        if (multiplier - 1 <= 0) return;

        multiplier--;
        itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().OnMultiplierChange(crafting.recipeList.recipes[currentId], multiplier, true);
        craftQuantity.text = multiplier.ToString();
    }

    public void OnCraftItem()
    {
        CraftingRecipe currRecipe = crafting.recipeList.recipes[currentId];

        if (!GameManager.instance.inventoryContainer.CheckFreeSpace()) return;

        if (!CheckInventoryQuantity(currRecipe, multiplier)) return;

        Item item = GameManager.instance.itemsDB.GetItemById(crafting.recipeList.recipes[currentId].output.item);
        if (item == null) return;

        if (item.stackable)
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
        itemDetails.GetComponent<InventoryCraftingItemDetailsPanel>().SetActiveRecipe(crafting.recipeList.recipes[currentId]);
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
        if (id >= crafting.recipeList.recipes.Count) return;
    }
}
