using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistentManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject character;

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

        //this.dataHandler = new(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        //LoadGame();
    }

    private void Start()
    {
        this.dataHandler = new(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.Init();
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
    }

    public void SaveGame()
    {
        // pass data to other scripts so they can update it.
        foreach (IDataPersistant dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // save that data to a file using data handler
        dataHandler.Save(gameData);
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

    private void GenerateNewData()
    {
        GameData newGameGameData = new()
        {
            activeSceneName = "",
            playerLevel = 1,
            playerExperience = 0,
            playerHealth = 40,
            playerMana = 10,
            playerPosition = new Vector3Int(0, 0, 0),

            equipedItems = "",
            inventory = "",
            characterRecipeList = "",
            workingBenchRecipeList = "",
            cropsContainers = new(),
            placeableObjectsContainers = new(),
            spawnedNodesContainers = new(),
        };

    }
}
