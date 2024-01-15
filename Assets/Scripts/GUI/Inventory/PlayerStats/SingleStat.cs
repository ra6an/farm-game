using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatOutputType
{
    Flat,
    Decimal,
    Percent
}

public class SingleStat : MonoBehaviour
{
    [SerializeField] Text statName;
    [SerializeField] Text statValue;

    public void Set(float value, StatOutputType type = StatOutputType.Flat)
    {
        if (type == StatOutputType.Decimal)
        {
            statValue.text = value.ToString("0.00");
        } 
        else if (type == StatOutputType.Percent)
        {
            statValue.text = value.ToString("0.00") + "%";
        }
        else
        {
            statValue.text = value.ToString("0");
        }
    }
}
