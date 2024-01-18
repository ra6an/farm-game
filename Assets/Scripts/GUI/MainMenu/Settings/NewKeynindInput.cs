using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewKeynindInput : MonoBehaviour
{
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
    [SerializeField] Text keyPressed;
    [SerializeField] GameObject confirm;
    [SerializeField] GameObject decline;

    private KeyCode key;

    private KeybindingCheck keybindingCheck;

    private void Awake()
    {
        keybindingCheck = new KeybindingCheck();
        key = KeyCode.None;
    }

    void Update()
    {
        if (!this.gameObject.activeInHierarchy) return;

        if (key != KeyCode.None) return;
        
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in keyCodes)
            {
                if (Input.GetKey(keyCode))
                {
                    if (keyCode == KeyCode.Escape) return;

                    Debug.Log("KeyCode down: " + keyCode);
                    keyPressed.text = keyCode.ToString();
                    keybindingCheck.keyCode = keyCode;
                    key = keyCode;

                    confirm.SetActive(true);
                    decline.SetActive(true);
                    break;
                }
            }
        }
    }

    public void SetNewKeybindInput(KeybindingActions action)
    {
        this.gameObject.SetActive(true);
        keybindingCheck.keybindingAction = action;
        keybindingCheck.keyCode = KeyCode.None;
        keyPressed.text = keybindingCheck.keyCode.ToString();
        key = KeyCode.None;
    }

    public void ConfirmNewInput()
    {
        GameObject controlsSettingsPanel = GameObject.Find("ControllsSettingsPanel");
        if (controlsSettingsPanel == null) return;

        this.gameObject.SetActive(false);

        InputManager.instance.Rebind(keybindingCheck);
        key = KeyCode.None;

        confirm.SetActive(false);
        decline.SetActive(false);

        controlsSettingsPanel.GetComponent<ControlsSettingsPanel>().ShowControls();
    }

    public void DeclineNewInput()
    {
        this.gameObject.SetActive(false);

        keybindingCheck = new KeybindingCheck();
        key = KeyCode.None;

        confirm.SetActive(false);
        decline.SetActive(false);
    }
}
