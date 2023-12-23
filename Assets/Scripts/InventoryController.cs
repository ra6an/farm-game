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
    [SerializeField] GameObject inventoryContainer;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!panel.activeInHierarchy)
            {
                statusPanel.SetActive(true);
                coinPanel.SetActive(true);
                panel.SetActive(true);
                toolbarPanel.SetActive(false);
            } else
            {
                statusPanel.SetActive(false);
                coinPanel.SetActive(false);
                panel.SetActive(false);
                toolbarPanel.SetActive(true);
            }
        }
    }

    public void Open()
    {
        statusPanel.SetActive(false);
        coinPanel.SetActive(true);
        panel.SetActive(true);
        toolbarPanel.SetActive(false);
    }

    public void Close()
    {
        statusPanel.SetActive(false);
        coinPanel.SetActive(false);
        panel.SetActive(false);
        toolbarPanel.SetActive(true);
    }
}
