using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public Item placedItem;
    public Transform targetObject;
    public Vector3Int positionOnGrid;
    public string objectState; //JSON string

    public PlaceableObject(Item item, Vector3Int position)
    {
        placedItem = item;
        positionOnGrid = position;
    }
}

[CreateAssetMenu(menuName ="Data/Placeable Object Container")]
public class PlaceableObjectContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;

    internal object Get(Vector3Int position)
    {
        return placeableObjects.Find(x => x.positionOnGrid == position);
    }

    internal void Remove(PlaceableObject placedObject)
    {
        placeableObjects.Remove(placedObject);
    }
}
