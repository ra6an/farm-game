using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Damageable : MonoBehaviour
{
    IDamageable damageable;

    [SerializeField] Color toColor = new Color(255f, 255f, 255f, 255f);
    [SerializeField] Color fromColor = new Color(255f, 1f, 1f, 255f);

    internal void TakeDamage(int damage)
    {
        if (damageable == null)
        {
            damageable = GetComponent<IDamageable>();
        }

        damageable.CalculateDamage(ref damage);
        damageable.ApplyDamage(damage);
        GameManager.instance.messageSystem.PostMessage(transform.position, damage.ToString());
        damageable.CheckState();

        LeanTween.value(gameObject, setColorCallback, fromColor, toColor, 0.16f);
    }

    private void setColorCallback(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;

        Color tempColor = gameObject.GetComponent<SpriteRenderer>().color;
        tempColor.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = tempColor;
    }
}
