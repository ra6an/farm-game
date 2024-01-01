using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedNodesReferenceManager : MonoBehaviour
{
    public SpawnedNodesManager spawnedNodesManager;

    public void Place(NodeType nodeName, Vector3Int position)
    {
        if(spawnedNodesManager == null)
        {
            Debug.Log("No spawnedNodesManager reference detected!");
        }

        spawnedNodesManager.Spawn(nodeName, position);
    }

    public bool Check(Vector3Int position)
    {
        if (spawnedNodesManager == null)
        {
            Debug.Log("No spawnedNodesManager reference detected!");
            return false;
        }

        return spawnedNodesManager.Check(position);
    }

    //internal void PickUp(Vector3Int gridPosition)
    //{
    //    if (placeableObjectManager == null)
    //    {
    //        Debug.Log("No placeableObjectManager reference detected!");
    //        return;
    //    }

    //    placeableObjectManager.PickUp(gridPosition);
    //}
}
