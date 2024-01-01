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
    public Vector3Int positionOnGrid;
    public string objectState;

    public SpawnedNode(NodeType node, Vector3Int position)
    {
        this.node = node;
        positionOnGrid = position;
    }
}

[CreateAssetMenu(menuName = "Data/Spawned Node Container")]
public class SpawnedNodeContainer : ScriptableObject
{
    public List<SpawnedNode> spawnedNodes;
    
    internal object Get(Vector3Int position) 
    {
        return spawnedNodes.Find(x => x.positionOnGrid == position);
    }

    internal void Remove(SpawnedNode spawnedNode)
    {
        spawnedNodes.Remove(spawnedNode);
    }
}
