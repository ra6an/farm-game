using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeybindingCheck
{
    public KeybindingActions keybindingAction;
    public KeyCode keyCode;
    public bool isCustomizable;
}

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    public List<KeybindingCheck> keybindingChecks;
}
