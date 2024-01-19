using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistentManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public GameData gameData;
    private List<IDataPersistant> dataPersistenceObjects;

    public static DataPersistentManager instance { get; private set; }
    private FileDataHandler dataHandler;

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
        this.dataHandler = new(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data from a file.
        this.gameData = dataHandler.Load();

        //if no data can be loaded, initialize to a new game.
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //push the loaded data to all other scripts that need it
        foreach(IDataPersistant dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log(gameData.playerPosition);
    }

    public void SaveGame()
    {
        // pass data to other scripts so they can update it.
        foreach (IDataPersistant dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // save that data to a file using data handler
        dataHandler.Save(gameData);
        //Debug.Log(gameData.playerPosition);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistant> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistant> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistant>();

        return new List<IDataPersistant>(dataPersistenceObjects);
    }
}
