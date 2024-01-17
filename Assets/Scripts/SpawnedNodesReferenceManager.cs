using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedNodesReferenceManager : MonoBehaviour
{
    public SpawnedNodesManager spawnedNodesManager;

    public void Place(NodeType nodeName, List<Vector3Int> position)
    {
        if(spawnedNodesManager == null)
        {
            Debug.Log("No spawnedNodesManager reference detected!");
        }

        spawnedNodesManager.Spawn(nodeName, position);
    }

    public bool Check(List<Vector3Int> position)
    {
        if (spawnedNodesManager == null)
        {
            Debug.Log("No spawnedNodesManager reference detected!");
            return false;
        }

        return spawnedNodesManager.Check(position);
    }
}
