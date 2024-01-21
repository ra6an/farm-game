using System;
using System.Collections;
using UnityEngine;
//
public class CraftingStationContainerInteract : Interactable
{
    bool isOpened;
    private InputManager inputManager;
    Character character;
    [SerializeField] AudioClip onOpenPlay;
    [SerializeField] AudioClip onClosePlay;

    private void Start()
    {
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        if(isOpened && (inputManager.GetKeyUp(KeybindingActions.Inventory) || inputManager.GetKeyUp(KeybindingActions.Main_Menu)))
        {
            Close(character);
            isOpened = false;
        }
    }

    public override void Interact(Character c)
    {
        character = c;
        if (isOpened)
        {
            Close(c);
            isOpened = false;
        }
        else
        {
            Open(c);
            isOpened = true;
        }
    }

    public void Open(Character character)
    {
        character.GetComponent<CraftingStationContainerInteractController>().Open(transform);
    }

    public void Close(Character character)
    {
        character.GetComponent<CraftingStationContainerInteractController>().Close();
    }
}
