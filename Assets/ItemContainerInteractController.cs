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

    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        //if (storagePanel.activeInHierarchy)
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab))
        //    {
        //        storagePanel.SetActive(false);
        //    }
        //}
    }

    public void Open(ItemContainer itemContainer)
    {
        targetItemContainer = itemContainer;
        itemContainerPanel.inventory = targetItemContainer;
        storagePanel.gameObject.SetActive(true);

        inventoryController.Open();
    }

    public void Close()
    {
        storagePanel.gameObject.SetActive(false);
        inventoryController.Close();
    }
}
