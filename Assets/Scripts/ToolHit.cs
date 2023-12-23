using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHit : MonoBehaviour
{
    public virtual void Hit(int dmg)
    {

    }

    public virtual void Interact()
    {

    }

    public virtual bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return true;
    }
}
