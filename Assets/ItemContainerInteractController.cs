using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    InventoryController inventoryController;
    [SerializeField] GameObject storagePanel;
    [SerializeField] ItemContainerPanel itemContainerPanel;
    Transform openedChest;
    [SerializeField] float maxDistance = 0.8f;

    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        if(openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            if(distance > maxDistance)
            {
                openedChest.GetComponent<StorageContainerInteract>().Close(GetComponent<Character>());
            }
        }
    }

    public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        itemContainerPanel.inventory = targetItemContainer;
        storagePanel.gameObject.SetActive(true);

        inventoryController.Open();
        openedChest = _openedChest;
    }

    public void Close()
    {
        storagePanel.gameObject.SetActive(false);
        inventoryController.Close();
        openedChest = null;
    }
}
