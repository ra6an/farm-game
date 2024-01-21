using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsPanel;
    //[SerializeField] GameObject LoadGamePanel;

    private void Start()
    {
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        if (inputManager == null) inputManager = InputManager.instance;

        bool allPanelsClosed = GameManager.instance.player.GetComponent<ShowPanelsController>().AllPanelsAreClosed();

        if(allPanelsClosed && inputManager != null && inputManager.GetKeyUp(KeybindingActions.Main_Menu))
        {
            if(settingsPanel.activeInHierarchy)
            {
                settingsPanel.GetComponent<SettingsPanel>().HidePanel();
                return;
            }
            mainMenu.SetActive(!mainMenu.activeInHierarchy);
        }
    }
}
