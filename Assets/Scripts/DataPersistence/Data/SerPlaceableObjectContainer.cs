using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerPlaceableObject
{
    public int placedItem;
    public Transform targetObject;
    public List<Vector3Int> positionOnGrid;
    public string objectState;
}

public class SerPlaceableObjectContainer
{
    public List<SerPlaceableObject> placeableObjects;

    public void AddSerObject(PlaceableObject placeableObject)
    {
        ItemList itemsDB = GameManager.instance.itemsDB;

        if (itemsDB == null) return;

        SerPlaceableObject spo = new SerPlaceableObject();

        int itemID = itemsDB.GetItemId(placeableObject.placedItem);
        if (itemID == -1) return;

        spo.placedItem = itemID;
        spo.objectState = placeableObject.objectState;
        spo.positionOnGrid = placeableObject.positionOnGrid;
    }
}
