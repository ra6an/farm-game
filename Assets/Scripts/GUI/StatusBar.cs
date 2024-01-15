using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] Text maxValue;
    [SerializeField] Text currValue;
    [SerializeField] Slider bar;

    public void Set(float curr, float max)
    {
        bar.maxValue = max;
        bar.value = curr;

        maxValue.text = max.ToString("0");
        currValue.text = curr.ToString("0");
    }
}
