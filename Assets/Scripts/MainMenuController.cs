using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] GameObject mainMenu;

    private void Awake()
    {
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        if(inputManager.GetKeyUp(KeybindingActions.MainMenu))
        {
            mainMenu.SetActive(!mainMenu.activeInHierarchy);
        }
    }
}
