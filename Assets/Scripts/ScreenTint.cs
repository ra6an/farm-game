using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTint : MonoBehaviour
{
    [SerializeField] Color tintedColor;
    [SerializeField] Color untintedColor;
    float f;
    public float speed = 0.5f;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Tint()
    {
        StopAllCoroutines();
        f = 0f;
        StartCoroutine(TintScreen());
    }

    public void Untint() 
    {
        StopAllCoroutines();
        f = 0f;
        StartCoroutine(UntintScreen());
    }

    private IEnumerator TintScreen()
    {
        while(f < 1f)
        {
            f += Time.deltaTime * speed;
            f = Mathf.Clamp(f, 0, 1f);

            Color c = image.color;
            c = Color.Lerp(untintedColor, tintedColor, f);
            image.color = c;

            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator UntintScreen()
    {
        while (f < 1f)
        {
            f += Time.deltaTime * speed;
            f = Mathf.Clamp(f, 0, 1f);

            Color c = image.color;
            c = Color.Lerp(tintedColor, untintedColor, f);
            image.color = c;

            yield return new WaitForEndOfFrame();
        }
    }
}
