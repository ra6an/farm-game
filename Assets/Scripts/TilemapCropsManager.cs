using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        List<CropTile> tilesToRemove = new();

        foreach (CropTile cropTile in container.crops)
        {
            if (!cropTile.watered)
            {
                cropTile.damage += 0.01f;
            }

            if (cropTile.crop == null && cropTile.damage > 0.4f)
            {
                tilesToRemove.Add(cropTile);
                continue;
            }

            if (cropTile.crop == null ) continue;

            if (cropTile.damage > 1 && cropTile.crop != null)
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

            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
                cropTile.renderer.gameObject.SetActive(true);
                

                if (cropTile.growStage + 1 < cropTile.crop.growthStageTime.Count)
                {
                    cropTile.growStage += 1;
                }
            }
        }

        foreach (CropTile tile in tilesToRemove)
        {
            targetTilemap.SetTile(tile.position, null);
            container.crops.Remove(tile);
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
            tile.damage = 0;
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

        if (growing)
        {
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
        }

        cropTile.renderer.gameObject.SetActive(growing);
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
        //Vector2Int position = (Vector2Int)gridPosition;
        CropTile tile = container.Get(gridPosition);
        if (tile == null) return;

        if (tile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(
                targetTilemap.CellToWorld(gridPosition) + new Vector3(0.5f,0.5f,0),
                tile.crop.yield,
                tile.crop.count
                );

            tile.Harvested();

            VisualizeTile(tile);
        }
    }
}
