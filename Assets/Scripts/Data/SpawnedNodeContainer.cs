using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnedNode
{
    //public GameObject spawnedObject;
    public NodeType node;
    public Transform targetObject;
    public List<Vector3Int> positionOnGrid;
    public string objectState;

    public SpawnedNode(NodeType node, List<Vector3Int> position)
    {
        this.node = node;
        positionOnGrid = position;
    }
}

[CreateAssetMenu(menuName = "Data/Spawned Node Container")]
public class SpawnedNodeContainer : ScriptableObject
{
    public List<SpawnedNode> spawnedNodes;
    
    internal object Get(List<Vector3Int> positions) 
    {
        if (positions.Count <= 0) return null;

        object itemOnGrid = null;

        foreach(Vector3Int v in positions)
        {
            SpawnedNode so = CheckInSInglePosition(v);

            if (so != null) 
            {
                itemOnGrid = so;
                break;
            }
        }

        return itemOnGrid;
    }

    private SpawnedNode CheckInSInglePosition(Vector3Int p)
    {
        SpawnedNode exists = null;
        foreach (SpawnedNode so in spawnedNodes)
        {
            bool ex = so.positionOnGrid.Exists(x => x == p);

            if (ex)
            {
                exists = so;
                break;
            }
        }
        return exists;
    }

    internal void Remove(SpawnedNode spawnedNode)
    {
        spawnedNodes.Remove(spawnedNode);
    }
}
