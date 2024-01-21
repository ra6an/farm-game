using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObjects
{
    public string sceneName;
    public string container;
}

[Serializable]
public class SpawnedNodes
{
    public string sceneName;
    public SpawnedNodeContainer container;
}

[Serializable]
public class Crops
{
    public string sceneName;
    public CropsContainer container;
}

[Serializable]
public class GameData
{
    public string activeSceneName;
    public Vector3 playerPosition;
    public EquipedItemsData equipedItems;

    // Inventory
    public string inventory;

    // Crafting
    public string knownRecipes;
    public string knownWorkingBenchRecipeList;

    // Cropse Managers
    public List<Crops> cropsContainers;

    // PlaceableObjects Containers
    public List<PlaceableObjects> placeableObjectsContainers;

    // Spawned Nodes Containers
    public List<SpawnedNodes> spawnedNodesContainers;

}
