using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] int health = 20;
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] int dropCount;
    [SerializeField] int itemCountInOneDrop;
    [SerializeField] float spread = 0.7f;
    [SerializeField] Item item;
    [SerializeField] ResourceNodeType nodeType;

    private void Awake()
    {
        dropCount = Random.Range(3, 5);
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z);
    }
    public override void Hit(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            while (dropCount > 0)
            {
                itemCountInOneDrop = Random.Range(1, 3);
                dropCount -= 1;

                Vector3 position = transform.position;
                position.x += spread * Random.value - spread / 2;
                position.y += spread * Random.value - spread / 2;

                ItemSpawnManager.instance.SpawnItem(position, item, itemCountInOneDrop);
            }

            Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        base.Interact();
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        //Debug.Log(canBeHit.Contains(nodeType));
        return canBeHit.Contains(nodeType);
    }
}
