using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class PlaceableObjectManager : MonoBehaviour
{
    [SerializeField] PlaceableObjectContainer placeableObjects;
    [SerializeField] Tilemap targetTilemap;

    private void Start()
    {
        GameManager.instance.GetComponent<PlaceableObjectsReferenceManager>().placeableObjectManager = this;
        VisualizeMap();
    }

    private void OnDestroy()
    {
        for(int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            placeableObjects.placeableObjects[i].targetObject = null;
        }
    }

    private void VisualizeMap()
    {
        for(int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            VisualizeItem(placeableObjects.placeableObjects[i]);
        }
    }
    internal void PickUp(Vector3Int gridPosition)
    {
        PlaceableObject placedObject = (PlaceableObject)placeableObjects.Get(gridPosition);

        if(placedObject == null)
        {
            return;
        }

        ItemSpawnManager.instance.SpawnItem
            (
            targetTilemap.CellToWorld(gridPosition) + targetTilemap.cellSize / 2, 
            placedObject.placedItem, 1
            );

        Destroy(placedObject.targetObject.gameObject);

        placeableObjects.Remove(placedObject);
    }

    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(placeableObject.placedItem.itemPrefab);
        Vector3 position = targetTilemap.CellToWorld(placeableObject.positionOnGrid) + targetTilemap.cellSize / 2;
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        placeableObject.targetObject = go.transform;
    }

    public bool Check(Vector3Int position)
    {
        return placeableObjects.Get(position) != null;
    }

    public void Place(Item item, Vector3Int positionOnGrid)
    {
        if (Check(positionOnGrid)) return;

        PlaceableObject placeableObject = new(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeableObjects.placeableObjects.Add(placeableObject);
    }

}
