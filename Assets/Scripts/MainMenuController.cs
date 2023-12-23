using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            mainMenu.SetActive(!mainMenu.activeInHierarchy);
        }
    }
}
