using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject statusPanel;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject coinPanel;
    ShowPanelsController showPanelsController;

    private void Awake()
    {
        inputManager = InputManager.instance;
        showPanelsController = GetComponent<ShowPanelsController>();
    }

    private void Update()
    {
        if(inputManager.GetKeyDown(KeybindingActions.Inventory) && showPanelsController.canOpeninventory)
        {
                if(panel.activeInHierarchy)
                {
                    showPanelsController.CloseInventory();
                } else
                {
                    showPanelsController.OpenInventory();
                }
        }
    }

    public void Open()
    {
        showPanelsController.OpenInventory();
    }

    public void Close()
    {
        showPanelsController.CloseInventory();
    }

    public void CheckItemInsideInventory(ItemSlot slot)
    {
        //inventoryContainer;
    }
}
