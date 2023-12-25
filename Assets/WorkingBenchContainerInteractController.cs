using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingBenchContainerInteractController : MonoBehaviour
{
    InventoryController inventoryController;
    [SerializeField] GameObject workingBenchPanel;
    [SerializeField] GameObject itemDetailsPanel;
    [SerializeField] GameObject elementsPanel;
    [SerializeField] GameObject ScrollView;
    [SerializeField] GameObject buttonsList;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] WorkingBenchRecipeList recipeList;
    Transform workingBench;

    [SerializeField] float maxDistance = 0.8f;

    void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (workingBench != null) 
        {
            float distance = Vector2.Distance(workingBench.position, transform.position);
            if (distance > maxDistance)
            {
                workingBench.GetComponent<WorkingBenchContainerInteract>().Close(GetComponent<Character>());
            }
        }
    }

    public void Open(Transform bench)
    {
        workingBench = bench;
        workingBenchPanel.SetActive(true);
        toolbarPanel.SetActive(false);
        buttonsList.GetComponent<CraftingButtonsPanel>().ShowCraftingList(recipeList, transform);
    }

    public void Close()
    {
        workingBenchPanel.SetActive(false);
        toolbarPanel.SetActive(true);
        workingBench = null;
    }

    public void ShowItemDetails(CraftingRecipe recipe)
    {
        itemDetailsPanel.GetComponent<WorkingBenchItemDetails>().SetActiveRecipe(recipe);

        elementsPanel.GetComponent<ElementsPanel>().SetElementsDetails(recipe.elements);

    }
}
