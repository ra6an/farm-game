using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] float hp = 10;

    public void ApplyDamage(float damage)
    {
        hp -= damage;
    }

    public void CalculateDamage(ref float damage)
    {
        
    }

    public void CheckState()
    {
        if (hp <= 0) 
        { 
            Destroy(gameObject);
        }
    }
}
