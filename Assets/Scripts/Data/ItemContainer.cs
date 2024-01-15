using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int quantity;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        quantity = slot.quantity;
    }

    public void Set(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}

[CreateAssetMenu(menuName ="Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;
    public bool isDirty;

    public int slotsLocked;

    internal void Init()
    {
        slots = new List<ItemSlot>();
        for(int i = 0; i < 30; i++)
        {
            slots.Add(new ItemSlot());
        }
    }

    public void Add(Item item, int quantity = 1)
    {
        isDirty = true;

        if(item.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == item);
            if(itemSlot != null)
            {
                itemSlot.quantity += quantity;
            } else
            {
                itemSlot = slots.Find(x => x.item == null);
                if(itemSlot != null)
                {
                    itemSlot.item = item;
                    itemSlot.quantity = quantity;
                }
            }
        } else
        {
            //non stackable item
            ItemSlot itemSlot = slots.Find(x => x.item == null);

            if(itemSlot != null)
            {
                itemSlot.item = item;
            }
        }
    }

    public void Remove(Item itemToRemove, int count = 1) {
        isDirty = true;

        if (itemToRemove.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);

            if (itemSlot == null) return;

            itemSlot.quantity -= count;

            if(itemSlot.quantity <= 0)
            {
                itemSlot.Clear();
            }
        } else
        {
            while (count > 0)
            {
                count -= 1;

                ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);

                if (itemSlot == null) return;

                itemSlot.Clear();
            }
        }
    }

    public void RemoveAt(int i)
    {
        isDirty = true;
        slots[i].Clear();
    }

    internal bool CheckFreeSpace()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null) return true;
        }

        return false;
    }

    internal int CheckHowManyFreeSlots()
    {
        int freeSlots = 0;

        for(int i = 0; i < slots.Count; i++)
        {
            freeSlots = slots[i].item == null ? freeSlots + 1 : freeSlots;
        }

        return freeSlots;
    }

    internal bool CheckItem(ItemSlot checkingItem)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == checkingItem.item);

        if(itemSlot == null) return false;

        if(checkingItem.item.stackable)
        {
            return itemSlot.quantity >= checkingItem.quantity;
        }

        return true;
    }

    public ItemSlot GetItemSlot(Item checkingItem)
    {
        ItemSlot inventoryItem = slots.Find(x => x.item == checkingItem);

        return (inventoryItem == null) ? null : inventoryItem;
    }
}
