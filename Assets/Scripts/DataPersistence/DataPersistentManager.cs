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

    public bool gameStarted = false;

    public static DataPersistentManager instance { get; private set; }
    private FileDataHandler dataHandler;

    private string selectedProfileId = "Slot 1";

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistent Manager in the same sceen!");
        }
        instance = this;

        this.dataHandler = new(Application.persistentDataPath, fileName, useEncryption);
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;

        GetLoadData();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.Init();
    }

    public void GetLoadData()
    {
        // Load any saved data from a file.
        this.gameData = dataHandler.Load(selectedProfileId);

        //if no data can be loaded, initialize to a new game.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
    }

    public void LoadGame()
    {
        Debug.Log(gameData.activeSceneName);
        Debug.Log(gameData.playerPosition);

        dataPersistenceObjects = FindAllDataPersistenceObjects();

        if(dataPersistenceObjects.Count <= 0)
        {
            Debug.LogError("Something went wrong with finding persistent objects!");
        }

        //push the loaded data to all other scripts that need it
        foreach (IDataPersistant dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log("Loading this obj");
            dataPersistenceObj.LoadData(gameData);
        }

        gameStarted = true;
    }

    public void SaveGame()
    {
        // pass data to other scripts so they can update it.
        foreach (IDataPersistant dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // save that data to a file using data handler
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "MainMenuScene") return;

        dataPersistenceObjects = FindAllDataPersistenceObjects();
        SaveGame();
    }

    private List<IDataPersistant> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistant> _dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistant>();
        return new List<IDataPersistant>(_dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
