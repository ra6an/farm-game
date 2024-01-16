using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] float offsetDistance = 1.2f;
    [SerializeField] Vector2 attackAreaSize = new Vector2(1f, 1f);

    Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Attack(Item item, Vector2 lastMotionVector)
    {
        Vector2 position = rigidbody2d.position + lastMotionVector * offsetDistance;

        Collider2D[] targets = Physics2D.OverlapBoxAll(position, attackAreaSize, 0f);

        foreach(Collider2D col in targets)
        {
            Damageable damageable = col.GetComponent<Damageable>();

            if (damageable != null && col.tag != "Player")
            {
                //Calculate damage
                float dmg = CalculateDamage(item);
                bool isCritical = false;

                //Calculate if hit is critical
                float critChance = transform.GetComponent<Character>().criticalChance.Value;
                float critDamage = transform.GetComponent<Character>().criticalDamage.Value;
                float random = Random.Range(0, 100);

                if (random <= critChance)
                {
                    dmg += (critDamage / 100) * dmg;
                    isCritical = true;
                }

                dmg = Mathf.Round(dmg);
                damageable.TakeDamage(dmg, isCritical);
            }
        }
    }

    private float CalculateDamage(Item item)
    {
        float dmg = 0f;

        if (!item.isWeapon) return dmg;

        //float toolDmg = item.damage;
        WeaponType type = item.weaponType;
        float characterPhyAtk = transform.GetComponent<Character>().physicalDamage.Value;
        float characterMagAtk = transform.GetComponent<Character>().magicDamage.Value;

        if (type == WeaponType.Mage)
        {
            dmg = characterMagAtk;
        } else
        {
            dmg = characterPhyAtk;
        }

        return dmg;
    }
}
