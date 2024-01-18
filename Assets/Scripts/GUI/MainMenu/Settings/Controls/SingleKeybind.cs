using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleKeybind : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Text bindName;
    [SerializeField] Text bindedKey;
    private KeybindingCheck keybindCheck;
    public bool needUpdate = false;

    private void Update()
    {

    }

    public void SetKeybind(KeybindingCheck bind)
    {
        string bn = bind.keybindingAction.ToString().Contains("_") ? bind.keybindingAction.ToString().Replace("_", " ") : bind.keybindingAction.ToString();
        bindName.text = bn;
        if((bind.keyCode.ToString()).Contains("Alpha"))
        {
            bindedKey.text = bind.keyCode.ToString()[5].ToString();
        } else
        {
            bindedKey.text = bind.keyCode.ToString();
        }
        keybindCheck = bind;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject go = GameObject.Find("ControllsSettingsPanel");

        if (go == null) return;
        go.GetComponent<ControlsSettingsPanel>().ShowNewKeybindInputPanel(keybindCheck);
    }
}
