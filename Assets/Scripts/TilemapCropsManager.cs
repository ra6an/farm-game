using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCropsManager : TimeAgent, IDataPersistant
{
    [SerializeField] string sceneName;
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
        //VisualizeMap();
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
                //Debug.Log("I'm done growing!");
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

    // Save and Load System
    [Serializable]
    public class SeriazableCropTile
    {
        public int growTimer;
        public int growStage;
        public int crop;
        //public SpriteRenderer renderer;
        public float damage;
        public Vector3Int position;
        public bool watered;
    }

    [Serializable]
    public class CropsListToSave
    {
        public List<SeriazableCropTile> crops;

        public void Init()
        {
            crops = new List<SeriazableCropTile>();
        }
    }

    public void SaveData(ref GameData data)
    {
        bool containerExistsInGameData = false;
        
        CropsListToSave cropsListToSave = new();
        cropsListToSave.Init();

        foreach (CropTile crop in container.crops)
        {
            SeriazableCropTile ct = new()
            {
                growTimer = crop.growTimer,
                growStage = crop.growStage,
                damage = crop.damage,
                position = crop.position,
                watered = crop.watered,
            };

            int cropId = GameManager.instance.cropsDB.GetCropId(crop.crop);
            ct.crop = cropId;

            cropsListToSave.crops.Add(ct);
        }

        string serializedCropsList = JsonUtility.ToJson(cropsListToSave);
        
        foreach (Crops crops in data.cropsContainers)
        {
            if (crops.sceneName == sceneName)
            {
                containerExistsInGameData = true;

                crops.container = serializedCropsList;
                break;
            }
        }

        if (!containerExistsInGameData)
        {
            Crops newCrops = new();

            newCrops.sceneName = sceneName;
            newCrops.container = serializedCropsList;
            data.cropsContainers.Add(newCrops);
        }
    }

    public void LoadData(GameData data)
    {
        container = (CropsContainer)ScriptableObject.CreateInstance(typeof(CropsContainer));
        container.Init();

        bool loadDataExist = false;
        string serializedCropsList = "";

        foreach(Crops crops in data.cropsContainers)
        {
            if(crops.sceneName == sceneName)
            {
                loadDataExist = true;
                serializedCropsList = crops.container;
                break;
            }
        }

        if (!loadDataExist) return;

        CropsListToSave deserializedCropsListToSave = JsonUtility.FromJson<CropsListToSave>(serializedCropsList);

        foreach(SeriazableCropTile sct in deserializedCropsListToSave.crops)
        {
            CropTile cropTileToAdd = new()
            {
                growTimer = sct.growTimer,
                growStage = sct.growStage,
                damage = sct.damage,
                position = sct.position,
                watered = sct.watered,
            };

            Crop crop = GameManager.instance.cropsDB.GetCropById(sct.crop);
            cropTileToAdd.crop = crop;
            
            container.crops.Add(cropTileToAdd);
        }

        if (container.crops.Count > 0) VisualizeMap();
    }
}
