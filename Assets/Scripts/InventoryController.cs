using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject statusPanel;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject inventoryContainer;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //panel.SetActive(!panel.activeInHierarchy);
            //statusPanel.SetActive(!statusPanel.activeInHierarchy);
            //toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
            inventoryContainer.SetActive(!inventoryContainer.activeInHierarchy);
        }
    }
}
