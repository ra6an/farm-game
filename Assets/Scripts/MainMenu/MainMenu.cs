using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private GameObject LoadGameSlots;
    [SerializeField] private SlotsPanel slotsPanel;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitGameButton;

    private bool dataIsLoaded = false;

    private void Start()
    {
        Dictionary<string, GameData> savedData = DataPersistentManager.instance.GetAllProfilesGameData();
        if(savedData.Count == 0)
        {
            loadGameButton.interactable = false;
        }
        if(!DataPersistentManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
    }

    private void Update()
    {
        if(dataIsLoaded)
        {
            GameData gd = DataPersistentManager.instance.gameData;
            GameSceneManager.instance.OnGameStartTransition(gd.activeSceneName);
            DataPersistentManager.instance.LoadGame();
            dataIsLoaded = false;
        }
    }

    public void OnNewGameClicked()
    {
        slotsPanel.ActivateMenu(false);
    }

    public void OnLoadGameClicked()
    {
        slotsPanel.ActivateMenu(true);
    }

    public void OnSettingsClicked()
    {
        DisableMenuButtons();
        Debug.Log("Settings clicked");
    }

    public void OnExitGameClicked()
    {
        DisableMenuButtons();
        Debug.Log("Exit clicked");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        settingsButton.interactable = false;
        exitGameButton.interactable = false;
    }
}
