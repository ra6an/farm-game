using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] ResourceNodeType nodeType;
    [SerializeField] int health = 20;
    [SerializeField] GameObject pickUpDrop;
    [Header("Main item to drop")]
    [SerializeField] Item item;
    [SerializeField] int dropCount;
    [SerializeField] int itemCountInOneDrop;
    [SerializeField] float dropProbability = 1.0f;
    [Header("Second item to drop")]
    [SerializeField] Item secondItem;
    [SerializeField] int secondDropCount;
    [SerializeField] int secondItemCountInOneDrop;
    [SerializeField] float secondDropProbability = 1.0f;
    [Header("Drop spread")]
    [SerializeField] float spread = 0.7f;

    private void Awake()
    {
        dropCount = dropCount == 0 ? Random.Range(3, 5) : dropCount;
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z);
    }
    public override void Hit(int dmg)
    {
        health -= dmg;

        if (health > 0) return;

        DropItem(dropCount, item, dropProbability, itemCountInOneDrop);

        if (secondItem != null)
        {
            DropItem(secondDropCount, secondItem, secondDropProbability, secondItemCountInOneDrop);
        }

        Vector3Int positionOnGrid = new Vector3Int((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f), (int)transform.position.z);

        transform.parent.GetComponent<SpawnedNodesManager>().DestroyNode(positionOnGrid);
    }

    public void ChangeHealth(int hp)
    {
        health = hp;
    }

    private void DropItem(int dropCnt, Item i, float probability, int itemCountInDrop = 0)
    {
        int drops;
        drops = dropCnt;

        if (gameObject.CompareTag("Tree") && i.name == "Wood")
        {
            int currGrowth = transform.GetComponent<TreeController>().data.currGrowthStage;
            if (currGrowth == 0) drops = 0;
            if (currGrowth == 1) drops = 1;
            if (currGrowth == 2) drops = 2;
        }
         
        while (drops > 0)
        {
            drops -= 1;
            float randomProbabilityNumber = Random.Range(0f, 1f);

            if (randomProbabilityNumber > probability) { continue; }

            itemCountInOneDrop = itemCountInDrop == 0 ? Random.Range(1, 3) : itemCountInDrop;

            Vector3 position = transform.position;
            position.x += spread * Random.value - spread / 2;
            position.y += spread * Random.value - spread / 2;

            ItemSpawnManager.instance.SpawnItem(position, i, itemCountInOneDrop);
        }
    }

    public override void Interact()
    {
        base.Interact();
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(nodeType);
    }
}
