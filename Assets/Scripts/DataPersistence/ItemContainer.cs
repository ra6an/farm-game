using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class ItemSlot
{
    public int item = -1;
    public int quantity;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        quantity = slot.quantity;
    }

    public void Set(int itemId, int quantity)
    {
        this.item = itemId;
        this.quantity = quantity;
    }

    public void Clear()
    {
        item = -1;
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

    public void Add(int itemId, int quantity = 1)
    {
        isDirty = true;

        ItemList itemsList = GameManager.instance.itemsDB;
        Item item = itemsList.GetItemById(itemId);

        if (item.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemId);
            if (itemSlot != null)
            {
                itemSlot.quantity += quantity;
            }
            else
            {
                itemSlot = slots.Find(x => x.item < 0);
                if (itemSlot != null)
                {
                    itemSlot.item = itemId;
                    itemSlot.quantity = quantity;
                }
            }
        }
        else
        {
            ItemSlot itemSlot = slots.Find(x => x.item < 0);

            if (itemSlot != null)
            {
                itemSlot.item = itemId;
            }
        }
    }

    public void Remove(int itemId, int count = 1)
    {
        isDirty = true;

        ItemList itemsList = GameManager.instance.itemsDB;
        Item itemToRemove = itemsList.GetItemById(itemId);

        if (itemToRemove.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemId);

            if (itemSlot == null) return;

            itemSlot.quantity -= count;

            if (itemSlot.quantity <= 0)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            while (count > 0)
            {
                count -= 1;

                ItemSlot itemSlot = slots.Find(x => x.item == itemId);

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
            if (slots[i].item < 0) return true;
        }

        return false;
    }

    internal int CheckHowManyFreeSlots()
    {
        int freeSlots = 0;

        for(int i = 0; i < slots.Count; i++)
        {
            freeSlots = slots[i].item < 0 ? freeSlots + 1 : freeSlots;
        }

        return freeSlots;
    }

    internal bool CheckItem(ItemSlot checkingItem)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == checkingItem.item);

        if(itemSlot == null) return false;

        ItemList itemsList = GameManager.instance.itemsDB;
        Item item = itemsList.GetItemById(checkingItem.item);

        if (item != null && item.stackable)
        {
            return itemSlot.quantity >= checkingItem.quantity;
        }

        return true;
    }

    public ItemSlot GetItemSlot(int itemId)
    {
        ItemSlot inventoryItem = slots.Find(x => x.item == itemId);

        return (inventoryItem == null) ? null : inventoryItem;
    }
}
