using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UIElements;
using System;

public class PlaceableObjectsReferenceManager : MonoBehaviour
{
    public PlaceableObjectManager placeableObjectManager;

    public void Place(Item item, List<Vector3Int> position)
    {
        if(placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectManager reference detected!");
            return;
        }

        placeableObjectManager.Place(item, position);
    }

    public bool Check(List<Vector3Int> position)
    {
        if (placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectManager reference detected!");
            return false;
        }

        return placeableObjectManager.Check(position);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        if (placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectManager reference detected!");
            return;
        }

        placeableObjectManager.PickUp(gridPosition);
    }
}
