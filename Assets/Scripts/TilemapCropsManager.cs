using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = System.Random;

public class TilemapCropsManager : TimeAgent
{
    [SerializeField] CropsContainer container;

    [SerializeField] TileBase watered;
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] Tilemap seedsTilemap;
    Tilemap targetTilemap;

    [SerializeField] GameObject CropsSpritePrefab;

    private void Start()
    {
        GameManager.instance.GetComponent<CropsManager>().cropsManager = this;
        targetTilemap = GetComponent<Tilemap>();
        onTimeTick += Tick;
        Init();
        VisualizeMap();
    }

    private void VisualizeMap()
    {
        for(int i = 0; i < container.crops.Count; i++)
        {
            VisualizeTile(container.crops[i]);
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < container.crops.Count; i++)
        {
            container.crops[i].renderer = null;
        }
    }

    public void Tick()
    {
        if (targetTilemap == null) return;

        foreach (CropTile cropTile in container.crops)
        {
            if(cropTile.crop == null ) continue;

            cropTile.damage += 0.02f;

            if (cropTile.damage > 1)
            {
                cropTile.Harvested();
                targetTilemap.SetTile(cropTile.position, plowed);
                continue;
            }

            if (cropTile.Complete)
            {
                Debug.Log("I'm done growing!");
                continue;
            }

            cropTile.growTimer += 1;

            if(cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                cropTile.growStage += 1;
            }
        }
    }
    internal bool Check(Vector3Int position)
    {
        return container.Get(position) != null;
    }

    internal bool CheckIfWatered(Vector3Int position)
    {
        return container.Get(position).watered;
    }

    public void Plow(Vector3Int position)
    {
        if (Check(position)) return;

        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        CropTile tile = container.Get(position);

        if (tile == null) return;

        if(tile.crop == null)
        {
            tile.crop = toSeed;
        }

        VisualizeTile(tile);
        //cropTile.renderer = go.GetComponent<SpriteRenderer>();

        //Vector3 newPosition = new Vector3(position.x + 0.5f, position.y+0.5f, position.z);

        //targetTilemap.SetTile(position, seeded);
        //seedsTilemap.SetTile(position, seeded);

        tile.crop = toSeed;
    }

    public void Water(Vector3Int position)
    {
        if(CheckIfWatered(position))
        {
            return;
        }

        CropTile tile = container.Get(position);

        if(tile != null)
        {
            tile.watered = true;
            seedsTilemap.SetTile(position, watered);
        }

    }

    public void VisualizeTile(CropTile cropTile)
    {
        if(cropTile.crop)
        {
            targetTilemap.SetTile(cropTile.position, plowed);
        } else
        {
            targetTilemap.SetTile(cropTile.position, plowed);
        }

        if (cropTile.watered)
        {
            seedsTilemap.SetTile(cropTile.position, watered);
        }

        if (cropTile.renderer == null)
        {
            GameObject go = Instantiate(CropsSpritePrefab, transform);
            go.transform.position = targetTilemap.CellToWorld(cropTile.position);
            go.transform.position = new Vector3(go.transform.position.x + 0.5f, go.transform.position.y + 0.5f, go.transform.position.z);
            go.transform.position -= Vector3.forward * 0.01f;
            go.SetActive(false);
            cropTile.renderer = go.GetComponent<SpriteRenderer>();
        }

        bool growing = cropTile.crop != null && cropTile.growStage >= cropTile.crop.growthStageTime[0];

        cropTile.renderer.gameObject.SetActive(growing);

        if(growing)
        {
            //cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage - 1];
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
        }
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        container.Add(crop);

        crop.position = position;

        VisualizeTile(crop);

        targetTilemap.SetTile(position, plowed);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        CropTile tile = container.Get(gridPosition);
        if (tile == null) return;

        if (tile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(
                //targetTilemap.CellToWorld(gridPosition),
                targetTilemap.CellToWorld(gridPosition) + new Vector3(0.5f,0.5f,0),
                tile.crop.yield,
                tile.crop.count
                );

            //seedsTilemap

            tile.Harvested();

            VisualizeTile(tile);
        }
    }
}
