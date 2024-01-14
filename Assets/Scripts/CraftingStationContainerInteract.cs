using System;
using System.Collections;
using UnityEngine;
//
public class CraftingStationContainerInteract : Interactable
{
    bool isOpened;
    Animator animator;
    Character character;
    [SerializeField] AudioClip onOpenPlay;
    [SerializeField] AudioClip onClosePlay;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isOpened && Input.GetKeyDown(KeyCode.Tab))
        {
            Close(character);
            isOpened = false;
            //character.GetComponent<InventoryController>().Open();
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
