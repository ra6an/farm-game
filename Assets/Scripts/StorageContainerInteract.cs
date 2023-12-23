using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Progress;

public class StorageContainerInteract : Interactable
{
    [SerializeField] bool opened;
    [SerializeField] AudioClip onOpenChest;
    [SerializeField] AudioClip onCloseChest;
    Animator animator;
    [SerializeField] ItemContainer itemContainer;
    private Character character;

    private void Update()
    {
        if(opened && Input.GetKeyUp(KeyCode.Tab))
        {
            Close(character);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(Character c)
    {
        character = c;
        if (!opened)
        {
            Open(c);
        }
        else
        {
            Close(c);
        }
    }

    public void Open(Character character)
    {
        opened = true;
        animator.SetTrigger("OpenChest");

        if (!onOpenChest) return;
        AudioManager.instance.Play(onOpenChest);

        character.GetComponent<ItemContainerInteractController>().Open(itemContainer, transform);
    }

    public void Close(Character character)
    {
        opened = false;
        animator.SetTrigger("CloseChest");

        if (!onCloseChest) return;
        AudioManager.instance.Play(onCloseChest);

        character.GetComponent<ItemContainerInteractController>().Close();
    }
}
