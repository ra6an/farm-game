using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] List<Button> buttons;
    [SerializeField] List<GameObject> panels;
    [SerializeField] GameObject newKeybindInput;

    private void Start()
    {
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        if (inputManager.GetKeyUp(KeybindingActions.Main_Menu))
        {
            transform.gameObject.SetActive(false);
            for (int i = 0; i < panels.Count; i++)
            {
                if (i == 0)
                {
                    panels[i].SetActive(true);
                } else
                {
                    panels[i].SetActive(false);
                }

            }
        }
    }

    public void HidePanel()
    {
        transform.gameObject.SetActive(false);
    }

    public void OnBtnPressShowPanel(int btnIndex)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == btnIndex)
            {
                //buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                //buttons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                //buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                //buttons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == btnIndex);
        }
    }

    public void ShowPanel()
    {
        this.gameObject.SetActive(true);
        newKeybindInput.SetActive(false);
    }
}
