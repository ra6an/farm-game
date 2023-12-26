using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject statusPanel;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject coinPanel;
    //[SerializeField] GameObject inventoryContainer;
    ShowPanelsController showPanelsController;

    private void Awake()
    {
        showPanelsController = GetComponent<ShowPanelsController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && showPanelsController.canOpeninventory)
        {
                if(panel.activeInHierarchy)
                {
                    showPanelsController.CloseInventory();
                } else
                {
                    showPanelsController.OpenInventory();
                }
            //if(!panel.activeInHierarchy)
            //{
            //    showPanelsController.OpenInventory();
            //} else
            //{
            //    showPanelsController.CloseInventory();
            //}
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
