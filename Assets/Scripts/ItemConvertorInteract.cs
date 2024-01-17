using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class ItemConvertorData
{
    public ItemSlot itemSlot;
    public int timer;
    public ItemSlot outputItem;

    public ItemConvertorData()
    {
        itemSlot = new ItemSlot();
        outputItem = new ItemSlot();
    }
}

[RequireComponent(typeof(TimeAgent))]
public class ItemConvertorInteract : Interactable, IPersistant
{
    [SerializeField] List<ConvertableItem> convertableItems;

    Animator animator;

    ItemConvertorData data;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += ItemConvertProcess;

        if(data == null)
        {
            data = new ItemConvertorData();
        }

        animator = GetComponent<Animator>();
        Animate();
    }

    private void ItemConvertProcess()
    {
        if (data.itemSlot == null) { return; }

        if (data.timer > 0)
        {
            data.timer -= 1;

            if (data.timer <= 0)
            {
                CompleteItemConversion();
            }
        }
    }

    private void CompleteItemConversion()
    {
        Animate();
    }

    public override void Interact(Character character)
    {
        if(data.itemSlot.item == null)
        {
            ItemSlot toolbarSlot = character.GetComponent<ToolbarController>().GetItemSlot;
            ConvertableItem recipe = convertableItems.Find(x => x.input.item == toolbarSlot.item);

            if(recipe != null)
            {
                StartItemProcessing(toolbarSlot, recipe);
                return;
            }
        } 

        if(data.itemSlot.item != null && data.timer <= 0)
        {
            GameManager.instance.inventoryContainer.Add(data.outputItem.item, data.outputItem.quantity);
            data.itemSlot.Clear();
            data.outputItem.Clear();
        }
    }

    private void StartItemProcessing(ItemSlot toProcess, ConvertableItem recipe)
    {
        data.itemSlot.Copy(toProcess);
        data.itemSlot.quantity = recipe.input.quantity;
        data.timer = recipe.timeToConvert;
        data.outputItem.item = recipe.output.item;
        data.outputItem.quantity = recipe.output.quantity;

        if (toProcess.item.stackable)
        {
            GameManager.instance.inventoryContainer.Remove(recipe.input.item, recipe.input.quantity);
        }
        else
        {
            GameManager.instance.inventoryContainer.Remove(recipe.input.item);
        }

        Animate();
    }

    private void Animate()
    {
        animator.SetBool("Working", data.timer > 0);
    }

    public string Read()
    {
        return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<ItemConvertorData>(jsonString);
    }
}
