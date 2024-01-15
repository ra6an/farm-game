using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    DisableControls disableControls;
    Character character;
    DayTimeController dayTime;

    private void Awake()
    {
        disableControls = GetComponent<DisableControls>();
        character = GetComponent<Character>();
        dayTime = GameManager.instance.timeController;
    }

    public void DoSleep()
    {
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        ScreenTint screenTint = GameManager.instance.screenTint;

        disableControls.DisableControl();

        screenTint.Tint();
        yield return new WaitForSeconds(2f);

        character.FullHeal();
        //character
        dayTime.SkipToMornin();

        screenTint.Untint();
        yield return new WaitForSeconds(2f);

        disableControls.EnableControls();

        yield return null;
    }
}
