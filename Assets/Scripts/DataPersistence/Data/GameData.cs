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
    public string container;
}

[Serializable]
public class GameData
{
    public string activeSceneName; //Done
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
    public List<Crops> cropsContainers; //Done

    // PlaceableObjects Containers
    public List<PlaceableObjects> placeableObjectsContainers; //Done

    // Spawned Nodes Containers
    public List<SpawnedNodes> spawnedNodesContainers; //Done

    public void Init()
    {
        activeSceneName = "MainScene";
        playerLevel = 1;
        playerExperience = 0;
        playerHealth = 10;
        playerMana = 10;
        playerPosition = new Vector3Int(0, 0, 0);
        equipedItems = "";
        inventory = "";
        characterRecipeList = "";
        workingBenchRecipeList = "";
        cropsContainers = new List<Crops>();
        placeableObjectsContainers = new List<PlaceableObjects>();
        spawnedNodesContainers = new List<SpawnedNodes>();
    }
}
