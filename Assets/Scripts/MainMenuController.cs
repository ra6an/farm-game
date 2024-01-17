using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] GameObject mainMenu;

    private void Start()
    {
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        if (inputManager == null) inputManager = InputManager.instance;

        if(inputManager != null && inputManager.GetKeyUp(KeybindingActions.MainMenu))
        {
            mainMenu.SetActive(!mainMenu.activeInHierarchy);
        }
    }
}
