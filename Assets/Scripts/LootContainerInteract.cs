using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LootContainerInteract : Interactable
{
    [SerializeField] bool opened;
    [SerializeField] AudioClip onOpenChest;
    [SerializeField] AudioClip onCloseChest;
    [SerializeField] Item item;
    [SerializeField] int count;

    //MOJ KOD
    private Animator animator;

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(Character character)
    {
        //if(!opened)
        //{
        //    opened = true;

        //    closedChest.SetActive(false);
        //    openedChest.SetActive(true);

        //    AudioManager.instance.Play(onOpenChest);
        //}
        // novi kod ovdje
        if (!opened)
        {
            opened = true;
            animator.SetTrigger("OpenChest");

            //ItemSpawnManager.instance.SpawnItem(transform.position, item, 1);
            //itemForSpawn.GetComponent<SpriteRenderer>().sprite = item.icon;
            //itemForSpawn.GetComponent<SpawnItem>().item = item;

            //Instantiate(itemForSpawn, transform.position, Quaternion.identity);
            ItemSpawnManager.instance.SpawnItem(transform.position, item, count);

            if (!onOpenChest) return;
            AudioManager.instance.Play(onOpenChest);
        } else
        {
            opened = false;
            animator.SetTrigger("CloseChest");

            if (!onCloseChest) return;
            AudioManager.instance.Play(onCloseChest);
        }
    }
}
