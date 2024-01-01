using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnedNodesManager : MonoBehaviour
{
    [SerializeField] SpawnedNodeContainer spawnedNodes;
    [SerializeField] Tilemap targetTilemap;

    [SerializeField] GameObject[] nodes;

    private void Start()
    {
        GameManager.instance.GetComponent<SpawnedNodesReferenceManager>().spawnedNodesManager = this;
        VisualizeMap();

        //LATER CHANGE TO FIND OBJECT THAT HOLDS ALL OBJECTSPAWNERS AND LOOP THROUGH ALL OF THEM AND SET CAN CHECK TO TRUE!!!
        GameObject go = GameObject.Find("ObjectSpawner");
        if(go != null)
        {
            go.GetComponent<ObjectSpawner>().canCheck = true;
        }
        
    }

    public void VisualizeMap()
    {
        foreach(SpawnedNode node in spawnedNodes.spawnedNodes)
        {
            VisualizeNode(node);
        }
    }

    private void VisualizeNode(SpawnedNode node)
    {
        //GameObject go = Instantiate(node.spawnedObject);
        GameObject go = null;

        foreach(GameObject n in nodes)
        {
            if(n.name == node.node.ToString())
            {
                go = Instantiate(n.gameObject);
            }
        }

        if (go == null) return;

        go.transform.parent = transform;

        Vector3 position = targetTilemap.CellToWorld(node.positionOnGrid) + targetTilemap.cellSize / 2;
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistant persistant = go.GetComponent<IPersistant>();
        if( persistant != null )
        {
            persistant.Load(node.objectState);
        }

        //node.spawnedObject = go;
    }

    public bool Check(Vector3Int position)
    {
        return spawnedNodes.Get(position) != null;
    }

    public void Spawn(NodeType nodeName, Vector3Int position)
    {
        if (Check(position)) return;

        //GameObject node;
        if (nodes.Length <= 0) return;

        foreach(GameObject n in nodes)
        {
            if (n.name == nodeName.ToString())
            {
                SpawnedNode spawnedNode = new SpawnedNode(nodeName, position);

                VisualizeNode(spawnedNode);
                spawnedNodes.spawnedNodes.Add(spawnedNode);
                //node = Instantiate(n.gameObject);
            }
        }

        //SpawnedNode spawnedNode = new SpawnedNode(nodeName, position);

        //VisualizeNode(spawnedNode);
        //spawnedNodes.spawnedNodes.Add(spawnedNode);
    }
}