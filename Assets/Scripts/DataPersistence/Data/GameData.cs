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
    public string container;
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
    public int playerLevel; //Done
    public float playerExperience; //Done
    public float playerHealth; //Done
    public float playerMana; //Done
    public Vector3 playerPosition; //Done
    public string equipedItems; //Done

    // Inventory
    public string inventory; //Done

    // Crafting
    public string characterRecipeList; //Done
    public string workingBenchRecipeList; //Done

    // Cropse Managers
    public List<Crops> cropsContainers;

    // PlaceableObjects Containers
    public List<PlaceableObjects> placeableObjectsContainers; //Done

    // Spawned Nodes Containers
    public List<SpawnedNodes> spawnedNodesContainers; //Done

}
