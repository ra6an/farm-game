using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
[CreateAssetMenu]
public class ItemList : ScriptableObject
{
    public List<Item> items;

    public int GetItemId(Item item)
    {
        int itemId = -1;

        for (int i = 0; i < items.Count; i++)
        {
            if(item == items[i])
            {
                itemId = i;
                break;
            }
        }

        return itemId;
    }

    public Item GetItemById(int placedItem)
    {
        if(placedItem == -1) return null;
        return items[placedItem];
    }
}
