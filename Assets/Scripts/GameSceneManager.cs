using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] ScreenTint screenTint;
    [SerializeField] CameraConfiner cameraConfiner;
    [SerializeField] CameraConfiner houseConfiner;
    public string currentScene;
    public bool sceneChanged = false;
    AsyncOperation unload;
    AsyncOperation load;

    bool respawnTransition;
    private List<IDataPersistant> persistantObj;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if(sceneChanged)
        {
            if (IsTransitioning()) return;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
            sceneChanged = false;
        }
    }

    public void OnGameStartTransition(string to)
    {
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unload = SceneManager.UnloadSceneAsync("MainMenuScene");
        currentScene = to;

        sceneChanged = true;
    }

    public void InitSwitchScene(string to, Vector3 targetPosition, string sceneName = null)
    {
        Debug.Log(to);
        StartCoroutine(Transition(to, targetPosition, sceneName));

        sceneChanged = true;
    }

    IEnumerator Transition(string to, Vector3 targetPosition, string sceneName = null)
    {
        persistantObj = FindAllDataPersistenceObjects();
        foreach(IDataPersistant persistant in persistantObj) 
        {
            if (persistant == null) continue;
            if(!persistant.isOneTimeLoader())
            {
                persistant.SaveData(DataPersistentManager.instance.gameData);
            }
        }

        screenTint.Tint();

        yield return new WaitForSeconds(1f / screenTint.speed + 0.1f);
        SwitchScene(to, targetPosition);

        while (load != null && unload != null)
        {
            if (load.isDone) load = null;
            if (unload.isDone) unload = null;

            //persistantObj = FindAllDataPersistenceObjects();
            //foreach (IDataPersistant persistant in persistantObj)
            //{
            //    if (persistant == null) continue;
            //    if (!persistant.isOneTimeLoader())
            //    {
            //        persistant.LoadData(DataPersistentManager.instance.gameData);
            //    }
            //}

            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));

        if(sceneName == null)
        {
            cameraConfiner.UpdateBounds(currentScene);
        } else
        {
            cameraConfiner.UpdateBounds(sceneName);
        }
        screenTint.Untint();
    }

    public void SwitchScene(string to, Vector3 targetPosition)
    {
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unload = SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;
        MoveCharacter(targetPosition);

        DataPersistentManager.instance.SaveGame();
    }

    private void MoveCharacter(Vector3 targetPosition)
    {
        Transform playerTransform = GameManager.instance.player.transform;

        CinemachineBrain currentCamera = Camera.main.GetComponent<CinemachineBrain>();
        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
            playerTransform,
            targetPosition - playerTransform.position
            );

        playerTransform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            playerTransform.position.z
            );

        if (respawnTransition)
        {
            playerTransform.GetComponent<Character>().FullHeal();
            playerTransform.GetComponent<DisableControls>().EnableControls();
            respawnTransition = false;
        }
    }

    internal void Respawn(Vector3 respawnPointPosition, string respawnPointScene)
    {
        respawnTransition = true;

        if(currentScene != respawnPointScene)
        {
            InitSwitchScene(respawnPointScene, respawnPointPosition, "Inside House");
        } else
        {
            MoveCharacter(respawnPointPosition);
        }
    }

    public bool IsTransitioning()
    {
        return load != null || unload != null;
    }

    private List<IDataPersistant> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistant> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistant>();

        return new List<IDataPersistant>(dataPersistenceObjects);
    }
}
