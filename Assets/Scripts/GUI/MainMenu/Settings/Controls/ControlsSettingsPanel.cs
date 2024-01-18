using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSettingsPanel : MonoBehaviour
{
    [SerializeField] Keybindings keybindings;
    [SerializeField] GameObject keybindContainer;
    [SerializeField] GameObject singleKeybindPrefab;
    [SerializeField] GameObject newKeybindInputPanel;

    private bool isDirty = true;

    private void Start()
    {
        ShowControls();
    }

    private void Update()
    {
        if(isDirty)
        {
            ShowNewKeybindings();
        }
    }

    public void ShowControls()
    {
        ClearContainer();
    }

    private void ShowNewKeybindings()
    {
        int i = 0;
        foreach( KeybindingCheck bind in keybindings.keybindingChecks )
        {
            if(bind.isCustomizable)
            {
                GameObject go = Instantiate(singleKeybindPrefab);
                go.GetComponent<SingleKeybind>().SetKeybind(bind);
                go.transform.SetParent(keybindContainer.transform);
                i++;
            }
        }
        isDirty = false;
    }

    private void ClearContainer()
    {
        if (keybindContainer.transform.childCount == 0) return;

        for (int i = 0; i < keybindContainer.transform.childCount; i++)
        {
            Destroy(keybindContainer.transform.GetChild(i).gameObject);
        }
        isDirty = true;
    }

    public void ShowNewKeybindInputPanel(KeybindingCheck keybindingCheck)
    {
        newKeybindInputPanel.GetComponent<NewKeynindInput>().SetNewKeybindInput(keybindingCheck.keybindingAction);
    }
}
