using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    [Serializable]
    public class KeybindingCheck
    {
        public KeybindingActions keybindingAction;
        public KeyCode keyCode;
        public bool isCustomizable;
    }

    public KeybindingCheck[] keybindingChecks;
}
