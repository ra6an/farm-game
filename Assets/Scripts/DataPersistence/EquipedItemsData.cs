using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[Serializable]
public class EquipedItem
{
    public int item;
    public string itemState;
    public TypeEquipable type;
}

[CreateAssetMenu(menuName = "Data/Equiped Items Data")]
public class EquipedItemsData : ScriptableObject
{
    public List<EquipedItem> equipedItems;

    public void Init()
    {
        equipedItems = new List<EquipedItem>();

        InitEquipSlot(-1, "", TypeEquipable.Helmet);
        InitEquipSlot(-1, "", TypeEquipable.Chest);
        InitEquipSlot(-1, "", TypeEquipable.Pants);
        InitEquipSlot(-1, "", TypeEquipable.Shoes);
        InitEquipSlot(-1, "", TypeEquipable.Ring);
        InitEquipSlot(-1, "", TypeEquipable.Necklace);
        InitEquipSlot(-1, "", TypeEquipable.Rune);
        InitEquipSlot(-1, "", TypeEquipable.Rune);
        InitEquipSlot(-1, "", TypeEquipable.Rune);
    }

    private void InitEquipSlot(int itemId, string itemState, TypeEquipable type)
    {
        EquipedItem ei = new();
        ei.item = itemId;
        ei.itemState = itemState;
        ei.type = type;

        equipedItems.Add(ei);
    }

    public void SetEquipSlot(int itemId, string itemState, TypeEquipable type)
    {
        foreach (EquipedItem ei in equipedItems)
        {
            if(ei.type == type)
            {
                ei.item = itemId;
                ei.itemState = itemState;
            }
        }
    }
}
