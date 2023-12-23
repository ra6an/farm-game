using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObjectsReferenceManager : MonoBehaviour
{
    public PlaceableObjectManager placeableObjectManager;

    public void Place(Item item, Vector3Int position)
    {
        if(placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectManager reference detected!");
            return;
        }

        placeableObjectManager.Place(item, position);
    }
}
