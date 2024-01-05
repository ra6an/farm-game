using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IconHighlight : MonoBehaviour
{
    public int itemWidth;
    public int itemHeight;
    public Vector3Int cellPosition;
    public Vector3 targetPosition;
    public Color valid;
    public Color invalid;
    [SerializeField] Tilemap targetTilemap;
    SpriteRenderer spriteRenderer;

    bool canSelect;
    bool show;

    public bool CanSelect
    {
        set {
            canSelect = value;
            gameObject.SetActive(canSelect && show);
        }
    }

    public bool Show
    {
        set
        {
            show = value;
            gameObject.SetActive(canSelect && show);
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        targetPosition = targetTilemap.CellToWorld(cellPosition);
        transform.position = targetPosition + new Vector3((float)itemWidth/2, (float)itemHeight/2, 0);
    }

    internal void Set(Sprite icon)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.sprite = icon;
    }

    public void SetColor(bool check)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.color = check ? valid : invalid;
    }
}
