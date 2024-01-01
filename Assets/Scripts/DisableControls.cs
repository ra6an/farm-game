using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableControls : MonoBehaviour
{
    CharacterController2D characterController;
    ToolsCharacterController toolsCharacter;
    InventoryController inventoryController;
    ToolbarController toolbarController;
    ItemContainerInteractController itemContainerInteractController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        toolbarController = GetComponent<ToolbarController>();
        toolsCharacter = GetComponent<ToolsCharacterController>();
        inventoryController = GetComponent<InventoryController>();
        itemContainerInteractController = GetComponent<ItemContainerInteractController>();
    }

    public void DisableControl()
    {
        characterController.enabled = false;
        toolbarController.enabled = false;
        toolsCharacter.enabled = false;
        inventoryController.enabled = false;
        itemContainerInteractController.enabled = false;
    }

    public void EnableControls()
    {
        characterController.enabled = true;
        toolbarController.enabled = true;
        toolsCharacter.enabled = true;
        inventoryController.enabled = true;
        itemContainerInteractController.enabled = true;
    }
}
