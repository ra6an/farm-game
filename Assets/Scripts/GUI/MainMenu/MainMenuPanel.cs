using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    public void NewGame()
    {
        DataPersistentManager.instance.NewGame();
    }

    public void LoadGame()
    {
        DataPersistentManager.instance.LoadGame();
    }

    public void Settings()
    {
        settingsPanel.GetComponent<SettingsPanel>().ShowPanel();
    }

    public void ExitGame()
    {
        
    }
}
