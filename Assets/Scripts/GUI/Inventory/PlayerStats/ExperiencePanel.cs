using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePanel : MonoBehaviour
{
    [SerializeField] Text expText;

    public void Set(float currExp, float expGoal)
    {
        expText.text = currExp.ToString() + " / " + expGoal.ToString();
    }
}
