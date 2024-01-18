using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistentManager : MonoBehaviour
{
    public GameData gameData;

    public static DataPersistentManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistent Manager in the same sceen!");
        }
        instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        
    }

    public void LoadGame()
    {
        // Load any saved data from a file.

        //if no data can be loaded, initialize to a new game.
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //push the loaded data to all other scripts that need it
    }

    public void SaveGame()
    {
        // pass data to other scripts so they can update it.

        // save that data to a file using data handler
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
