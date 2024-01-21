using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public int placedItem;
    public Transform targetObject;
    public List<Vector3Int> positionOnGrid;
    public string objectState; //JSON string

    public PlaceableObject(int item, List<Vector3Int> position)
    {
        placedItem = item;
        positionOnGrid = position;
    }
}

[CreateAssetMenu(menuName ="Data/Placeable Object Container")]
public class PlaceableObjectContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;

    public void Init()
    {
        placeableObjects = new List<PlaceableObject>();
    }

    internal object Get(List<Vector3Int> positions)
    {
        if(positions.Count <= 0) return null;

        object itemOnGrid = null;

        foreach(Vector3Int v in positions)
        {
            PlaceableObject po = CheckInSInglePosition(v);

            if(po != null)
            {
                itemOnGrid = po;
                break;
            }
        }
        return itemOnGrid;
    }

    private PlaceableObject CheckInSInglePosition(Vector3Int p)
    {
        PlaceableObject exists = null;
        foreach (PlaceableObject po in placeableObjects)
        {
            bool ex = po.positionOnGrid.Exists(x => x == p);

            if (ex)
            {
                exists = po;
                break;
            }
        }
        return exists;
    }

    internal void Remove(PlaceableObject placedObject)
    {
        placeableObjects.Remove(placedObject);
    }
}
