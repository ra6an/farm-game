using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    public void MainMenu()
    {

    }

    public void Settings()
    {
        settingsPanel.GetComponent<SettingsPanel>().ShowPanel();
    }

    public void SaveGame()
    {
        DataPersistentManager.instance.SaveGame();
    }

    public void ExitGame()
    {

    }
}
