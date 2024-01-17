using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSettingsPanel : MonoBehaviour
{
    [SerializeField] Keybindings keybindings;
    [SerializeField] GameObject keybindContainer;
    [SerializeField] GameObject singleKeybindPrefab;

    private bool isDirty = true;

    private void Start()
    {
        Show();
    }

    private void Update()
    {
        if(isDirty)
        {
            ShowNewKeybindings();
        }
    }

    public void Show()
    {
        ClearContainer();
    }

    private void ShowNewKeybindings()
    {
        Debug.Log(keybindings.keybindingChecks.Count);
        int i = 0;
        foreach( KeybindingCheck bind in keybindings.keybindingChecks )
        {
            if(bind.isCustomizable)
            {
                GameObject go = Instantiate(singleKeybindPrefab);
                go.GetComponent<SingleKeybind>().SetKeybind(bind);
                go.transform.SetParent(keybindContainer.transform);
                //Debug.Log(i + ", " + go.name);
                i++;
            }
        }
        //foreach (KeybindingCheck bind in keybindings.keybindingChecks)
        //{
        //    if (bind.isCustomizable)
        //    {
        //        GameObject go = Instantiate(singleKeybindPrefab);
        //        go.transform.SetParent(keybindContainer.transform);
        //        go.GetComponent<SingleKeybind>().SetKeybind(bind);
        //    }
        //}
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
}
