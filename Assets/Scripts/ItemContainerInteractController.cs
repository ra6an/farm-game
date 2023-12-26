using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    InventoryController inventoryController;
    [SerializeField] GameObject storagePanel;
    [SerializeField] ItemContainerPanel itemContainerPanel;
    Transform openedChest;
    [SerializeField] float maxDistance = 0.8f;
    ShowPanelsController showPanelsController;

    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
        showPanelsController = GetComponent<ShowPanelsController>();
    }

    private void Update()
    {
        if(openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            if(distance > maxDistance)
            {
                openedChest.GetComponent<StorageContainerInteract>().Close(GetComponent<Character>());
                showPanelsController.CloseStorage();
            }
        }
    }

    public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        itemContainerPanel.inventory = targetItemContainer;

        showPanelsController.OpenStorage();
        openedChest = _openedChest;
    }

    public void Close()
    {
        showPanelsController.CloseStorage();
        openedChest = null;
    }
}
