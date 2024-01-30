using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitGameButton;

    public void OnNewGameClicked()
    {
        //DisableMenuButtons();
        //DataPersistentManager.instance.NewGame();

        //GameManager.instance
        //SceneManager.UnloadSceneAsync("MainMenuScene");
        //SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
    }

    public void OnLoadGameClicked()
    {
        DisableMenuButtons();
        Debug.Log("Load Game");
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
