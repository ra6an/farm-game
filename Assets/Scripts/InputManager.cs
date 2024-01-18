using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] private Keybindings keybindings;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public KeyCode GetKeyForAction(KeybindingActions keybindingAction)
    {
        foreach(KeybindingCheck keybindingCheck in keybindings.keybindingChecks)
        {
            if (keybindingCheck.keybindingAction == keybindingAction) return keybindingCheck.keyCode;
        }

        return KeyCode.None;
    }

    public bool GetKey(KeybindingActions key)
    {
        foreach (KeybindingCheck keybindingCheck in keybindings.keybindingChecks)
        {
            if (keybindingCheck.keybindingAction == key) return Input.GetKey(keybindingCheck.keyCode);
        }

        return false;
    }

    public bool GetKeyUp(KeybindingActions key)
    {
        foreach (KeybindingCheck keybindingCheck in keybindings.keybindingChecks)
        {
            if (keybindingCheck.keybindingAction == key) return Input.GetKeyUp(keybindingCheck.keyCode);
        }

        return false;
    }

    public bool GetKeyDown(KeybindingActions key)
    {
        foreach (KeybindingCheck keybindingCheck in keybindings.keybindingChecks)
        {
            if (keybindingCheck.keybindingAction == key) return Input.GetKeyDown(keybindingCheck.keyCode);
        }

        return false;
    }

    public void Rebind(KeybindingCheck newBind)
    {
        for(int i = 0; i < keybindings.keybindingChecks.Count; i++)
        {
            if (keybindings.keybindingChecks[i].keybindingAction == newBind.keybindingAction)
            {
                keybindings.keybindingChecks[i].keyCode = newBind.keyCode;
            }
        }
    }
}
