using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string activeSceneName;
    public Vector3 playerPosition;
    public EquipedItemsData equipedItems;

    // Inventory
    public ItemContainer inventory;

    // Crafting
    public RecipeList knownRecipes;
    public RecipeList knownWorkingBenchRecipeList;

    // Cropse Managers
    public CropsContainer mainSceneCropsContainer;
    //public CropsContainer islandSceneCropsContainer;

    // PlaceableObjects Containers
    public PlaceableObjectContainer mainScenePlaceableObjectContainer;
    //public PlaceableObjectContainer islandScenePlaceableObjectContainer;

    // Spawned Nodes Containers
    public SpawnedNodeContainer mainSceneSpawnedNodeContainer;
    //public SpawnedNodeContainer islandSceneSpawnedNodeContainer;
    //public SpawnedNodeContainer forestDungeonSpawnedNodeContainer;

}
