using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{


    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////

    Transform player;
    [SerializeField] float speed = 2.5f;
    [SerializeField] float pickUpDistance = 1.5f;
    [SerializeField] float ttl = 10f;

    public Item item;
    public int count = 1;

    private void Awake()
    {
        player = GameManager.instance.player.transform;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }

    private void Update()
    {
        ttl -= Time.deltaTime;
        if(ttl < 0)
        {
            Destroy(gameObject);
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if(distance > pickUpDistance)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position, 
            player.position, 
            speed * Time.deltaTime
            );

        if(distance < 0.1f)
        {
            if(GameManager.instance.inventoryContainer != null) {
                int itemId = GameManager.instance.itemsDB.GetItemId(item);
                if (itemId < 0) return;

                GameManager.instance.inventoryContainer.Add(itemId, count);
            } else
            {
                Debug.LogWarning("No inventory container attached to the game manager!");
            }

            Destroy(gameObject);
        }
    }
}
