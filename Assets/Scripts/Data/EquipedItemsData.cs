using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipedItem
{
    public Item item;
    public TypeEquipable type;
}

[CreateAssetMenu(menuName = "Data/Equiped Items Data")]
public class EquipedItemsData : ScriptableObject
{
    public List<EquipedItem> equipedItems;
}
