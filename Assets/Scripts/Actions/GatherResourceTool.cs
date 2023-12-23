using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ResourceNodeType
{
    Undefined,
    Tree,
    Ore
}

[CreateAssetMenu(menuName ="Data/Tool Action/Gather Resource Node")]
public class GatherResourceTool : ToolAction
{
    [SerializeField] float sizeOfInteractableArea = 1f;
    [SerializeField] List<ResourceNodeType> canHitNodesOfType;
    [SerializeField] AudioClip onHitAction;

    public override bool OnApply(Vector2 worldPoint, int dmg)
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(worldPoint, sizeOfInteractableArea);

        foreach (Collider2D c in collider)
        {
            ToolHit hit = c.GetComponent<ToolHit>();
            if (hit != null)
            {
                if (hit.CanBeHit(canHitNodesOfType))
                {
                    hit.Hit(dmg);

                    //Play animation of resource getting hit
                    Animator hitAnimator = hit.GetComponent<Animator>();
                    if (hitAnimator)
                    {
                        hitAnimator.SetTrigger("Hit");
                    }

                    AudioManager.instance.Play(onHitAction);
                    return true;
                }
            }
        }

        return false;
    }
}
