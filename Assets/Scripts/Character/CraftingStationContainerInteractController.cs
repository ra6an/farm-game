using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//
public class CraftingStationContainerInteractController : MonoBehaviour
{
    [SerializeField] GameObject itemDetailsPanel;
    [SerializeField] GameObject elementsPanel;
    [SerializeField] GameObject buttonsList;
    public RecipeList recipeList;
    [SerializeField] Text craftQuantity;
    private ItemContainer inventory;
    Transform workingBench;
    ShowPanelsController showPanelsController;

    [SerializeField] float maxDistance = 0.8f;

    int multiplier = 1;

    void Awake()
    {
        showPanelsController = GetComponent<ShowPanelsController>();
    }

    private void Start()
    {
        inventory = GameManager.instance.inventoryContainer;
    }

    private void Update()
    {
        if (workingBench != null)
        {
            float distance = Vector2.Distance(workingBench.position, transform.position);
            if (distance > maxDistance)
            {
                workingBench.GetComponent<CraftingStationContainerInteract>().Interact(transform.GetComponent<Character>());
            }
        }
    }

    private IEnumerator Delay(CraftingRecipe recipe)
    {
        yield return new WaitForSeconds(0.01f);
        itemDetailsPanel.GetComponent<CraftingStationItemDetails>().SetActiveRecipe(recipe);
        elementsPanel.GetComponent<ElementsPanel>().SetElementsDetails(recipe.elements);

        bool canCraft = CheckElementsQuantityInInventory(recipe.elements);

        SetElementsValues(recipe.elements, 1);

        if (!canCraft)
        {
            craftQuantity.text = "0";
        }
    }

    public void Open(Transform bench)
    {
        workingBench = bench;
        showPanelsController.OpenWorkingBench();
        buttonsList.GetComponent<CraftingButtonsPanel>().ShowCraftingList(recipeList, transform);
    }

    public void Close()
    {
        showPanelsController.CloseWorkingBench();
        workingBench = null;
        buttonsList.GetComponent<CraftingButtonsPanel>().DestroyRecipeButtonsList();
    }

    public void ShowItemDetails(CraftingRecipe recipe)
    {
        multiplier = 1;
        elementsPanel.GetComponent<ElementsPanel>().ClearPanel();
        StartCoroutine(Delay(recipe));
    }

    private void SetElementsValues(List<ItemSlot> elements, int mltp)
    {
        for (int i = 0; i < elements.Count; i++) 
        {
            ItemSlot slot = new ItemSlot();
            slot.item = elements[i].item;
            //ItemSlot itemInInventory = inventory.GetItemSlot(elements[i].item);
            //int inventoryQuantity = itemInInventory == null ? 0 : itemInInventory.quantity;
            if (elementsPanel.transform.GetChild(i) == null) return;
            elementsPanel.transform.GetChild(i).GetComponent<ElementDetails>().ChangeQuantity(elements[i].quantity * mltp);
            //elementsPanel.transform.GetChild(i).GetComponent<ElementDetails>().ChangeInventoryQuantity(inventoryQuantity);
        }
    }

    public bool CheckElementsQuantityInInventory(List<ItemSlot> elements, int mltp = 1)
    {
        bool returnValue = true;

        for(int i = 0; i < elements.Count; i++)
        {
            ItemSlot slot = new ItemSlot();
            slot.item = elements[i].item;
            slot.quantity = mltp * elements[i].quantity;
            if (!inventory.CheckItem(slot))
            {
                returnValue = false;
            }
        }
        
        return returnValue;
    }

    public void CraftItem(CraftingRecipe recipe)
    {
        bool enoughResource = true;

        for (int i = 0; i < recipe.elements.Count; i++)
        {
            if (!inventory.CheckItem(recipe.elements[i]))
            {
                enoughResource = false;
            }
        }

        if (!enoughResource) return;

        if (!inventory.CheckFreeSpace()) return;

        foreach (ItemSlot slot in recipe.elements)
        {
            inventory.Remove(slot.item, slot.quantity * multiplier);
        }

        inventory.Add(recipe.output.item, recipe.output.quantity * multiplier);

        ShowItemDetails(recipe);
    }

    public void IncrementQuantity(CraftingRecipe recipe)
    {
        if (CheckElementsQuantityInInventory(recipe.elements, multiplier + 1))
        {
            SetElementsValues(recipe.elements, multiplier + 1);
            craftQuantity.text = ((multiplier + 1) * recipe.output.quantity).ToString();
            multiplier++;
        } else
        {
            AnimateElements(recipe.elements, multiplier + 1);
        }
    }

    public void DecrementQuantity(CraftingRecipe recipe)
    {
        if (multiplier <= 1) return;
        multiplier--;
        craftQuantity.text = (multiplier * recipe.output.quantity).ToString();
        SetElementsValues(recipe.elements, multiplier);
        AnimateElements(recipe.elements, multiplier);
    }

    public void AnimateElements(List<ItemSlot> elements, int mltp = 1)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            ItemSlot slot = new ItemSlot();
            slot.item = elements[i].item;
            ItemSlot itemInInventory = inventory.GetItemSlot(elements[i].item);

            if (itemInInventory == null || itemInInventory.quantity < elements[i].quantity * mltp)
            {
                elementsPanel.transform.GetChild(i).GetComponent<ElementDetails>().SetNotEnough();
            } else
            {
                elementsPanel.transform.GetChild(i).GetComponent<ElementDetails>().SetEnough();
            }

        }
    }
}
