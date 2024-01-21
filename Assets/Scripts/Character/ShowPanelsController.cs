using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanelsController : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject topPanel;
    [SerializeField] GameObject coinPanel;
    [SerializeField] GameObject storagePanel;
    [SerializeField] GameObject workingBenchPanel;
    [SerializeField] GameObject toolbarPanel;

    public bool canOpeninventory = true;
    public bool inventoryOpened = false;
    public bool workingBenchOpened = false;
    public bool storageOpened = false;

    private IEnumerator CanOpenInventory()
    {
        yield return new WaitForSeconds(0.2f);
        canOpeninventory = true;
        yield return null;
    }

    public bool AllPanelsAreClosed()
    {
        return !inventoryOpened && !workingBenchOpened && !storageOpened;
    }
    
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        coinPanel.SetActive(true);
        topPanel.SetActive(true);
        toolbarPanel.SetActive(false);

        inventoryOpened = true;
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        coinPanel.SetActive(false);
        topPanel.SetActive(false);
        toolbarPanel.SetActive(true);

        inventoryOpened = false;
    }

    public void OpenStorage()
    {
        inventoryPanel.SetActive(true);
        storagePanel.SetActive(true);
        toolbarPanel.SetActive(false);

        storageOpened = true;
    }

    public void CloseStorage()
    {
        inventoryPanel.SetActive(false);
        storagePanel.SetActive(false);
        toolbarPanel.SetActive(true);

        storageOpened= false;
    }

    public void OpenWorkingBench() {
        workingBenchPanel.SetActive(true);
        toolbarPanel.SetActive(false);
        workingBenchOpened = true;

        canOpeninventory = false;
    }

    public void CloseWorkingBench()
    {
        workingBenchPanel.SetActive(false);
        toolbarPanel.SetActive(true);
        workingBenchOpened = false;

        StartCoroutine(CanOpenInventory());
    }
}
