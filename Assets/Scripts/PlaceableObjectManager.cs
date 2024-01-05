using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class PlaceableObjectManager : MonoBehaviour
{
    [SerializeField] PlaceableObjectContainer placeableObjects;
    [SerializeField] SpawnedNodeContainer spawnedNodes;
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
            if (placeableObjects.placeableObjects[i].targetObject == null) { continue; }

            IPersistant persistant = placeableObjects.placeableObjects[i].targetObject.GetComponent<IPersistant>();

            if(persistant != null)
            {
                string jsonString = persistant.Read();
                placeableObjects.placeableObjects[i].objectState = jsonString;
            }

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
        List<Vector3Int> gridPosList = new List<Vector3Int>();
        gridPosList.Add(gridPosition);
        PlaceableObject placedObject = (PlaceableObject)placeableObjects.Get(gridPosList);

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
        go.transform.parent = transform;

        int posOnGridX = placeableObject.positionOnGrid[0].x;
        int posOnGridY = placeableObject.positionOnGrid[0].y;
        int itemWidth = placeableObject.placedItem.width;
        int itemHeight = placeableObject.placedItem.height;
        float xPosition = 0f;
        float yPosition = 0f;
        float xAddition = 0f;
        float yAddition = 0f;

        if (itemWidth == 1) 
        {
            xPosition = posOnGridX;
            xAddition = 0.5f;
        } 
        else if (itemWidth > 1 && itemWidth % 2 != 0)
        {
            xPosition = posOnGridX + ((float)itemWidth / 2 + 0.5f);
            xAddition = -0.5f;
        } 
        else if(itemWidth > 1 && itemWidth % 2 == 0)
        {
            xPosition = (posOnGridX + (float)itemWidth / 2);
            xAddition = 0f;
        }

        if(itemHeight == 1)
        {
            yPosition = posOnGridY;
            yAddition = 0.5f;
        }
        else if(itemHeight > 1 && itemHeight % 2 != 0)
        {
            yPosition = posOnGridY + ((float)itemHeight / 2 + 0.5f);
            yAddition = -0.5f;
        }
        else if (itemHeight > 1 && itemHeight % 2 == 0)
        {
            Debug.Log("Height: 2");
            yPosition = (posOnGridY + (float)itemHeight / 2);
            yAddition = 0f;
        }

        Vector3Int posForWorld = new Vector3Int(
            (int)xPosition, (int)yPosition, placeableObject.positionOnGrid[0].z
            );

        Vector3 position = targetTilemap.CellToWorld(posForWorld) + new Vector3(xAddition, yAddition, 0);
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistant persistant = go.GetComponent<IPersistant>();
        if (persistant != null)
        {
            persistant.Load(placeableObject.objectState);
        }

        placeableObject.targetObject = go.transform;
    }

    public bool Check(List<Vector3Int> positions)
    {
        return placeableObjects.Get(positions) != null || spawnedNodes.Get(positions) != null;
    }

    public void Place(Item item, List<Vector3Int> positionOnGrid)
    {
        if (Check(positionOnGrid)) return;
        
        PlaceableObject placeableObject = new(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeableObjects.placeableObjects.Add(placeableObject);
        
    }
}
