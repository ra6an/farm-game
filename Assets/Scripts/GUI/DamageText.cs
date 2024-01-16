using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public string message;
    [SerializeField] Color colorNonCritical;
    [SerializeField] Color colorOutlineNonCritical;
    [SerializeField] Color colorCritical;
    [SerializeField] Color colorOutlineCritical;


    public void SetText(string text, bool isCritical)
    {
        TextMeshPro tmp = this.GetComponent<TextMeshPro>();
        if(isCritical)
        {
            //message = text + " !";
            message = text;
            tmp.text = message;
            tmp.color = colorCritical;
            tmp.fontSize = 3.4f;
            tmp.outlineWidth = 0.3f;
            tmp.outlineColor = colorOutlineCritical;
        } else
        {
            message = text;
            tmp.text = message;
            tmp.color = colorNonCritical;
            tmp.fontSize = 3f;
            tmp.outlineWidth = 0.3f;
            tmp.outlineColor = colorOutlineNonCritical;
        }
    }
}
