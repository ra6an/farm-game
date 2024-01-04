using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : Interactable
{
    private bool isFruitTree;
    private Item fruit;
    private int count;
    public bool hasFruits = false;

    private void Start()
    {
        fruit = this.GetComponent<TreeController>().itemFruit;
        count = this.GetComponent<TreeController>().count;
        isFruitTree = this.GetComponent<TreeController>().isFruitTree;
    }

    public override void Interact(Character character)
    {
        if(isFruitTree && hasFruits)
        {
            if (GameManager.instance.inventoryContainer.CheckFreeSpace()) 
            {
                GameManager.instance.inventoryContainer.Add(fruit, count);
                this.GetComponent<TreeController>().isHarvested = true;
            } else
            {
                //DODATI DA JE INV PUN
            }
        }
    }
}
