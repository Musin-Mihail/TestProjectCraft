using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldGeneration : MonoBehaviour
{
    public Transform prefabTile;
    public Transform prefabcrystal;
    public Transform player;
    public Transform parentTiles;
    public List<Transform> poolTiles;
    List<Transform> visibleTiles;
    public List<Transform> Сrystals;
    Transform lastTile;
    Vector3 vector;
    Vector3 direction;
    int numberDirect;
    void Start()
    {
        PrimaryGeneration();
        StartCoroutine(DistanceСheck());
        Сrystals = new List<Transform>();
    }
    void PrimaryGeneration()
    {
        poolTiles = new List<Transform>();
        visibleTiles = new List<Transform>();
        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                vector = new Vector3(x, 0, z);
                Transform tile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
                poolTiles.Add(tile);
                visibleTiles.Add(tile);
            }
        }
        vector = new Vector3(1, 0, 3);
        lastTile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
        poolTiles.Add(lastTile);
        visibleTiles.Add(lastTile);

        vector = new Vector3(1, 0, 4);
        lastTile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
        poolTiles.Add(lastTile);
        visibleTiles.Add(lastTile);
    }
    IEnumerator DistanceСheck()
    {
        while (true)
        {
            if (Vector3.Distance(lastTile.position, player.position) < 10)
            {
                AddTile();
                if (visibleTiles.Count > 22)
                {
                    RemoveTile();
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    void AddTile()
    {
        int randomTile = Random.Range(0, 100);
        if (numberDirect < 5)
        {
            if (randomTile < 80)
            {
                direction = Vector3.forward;
                numberDirect++;
            }
            else
            {
                direction = Vector3.right;
            }
        }
        else
        {
            numberDirect = 0;
            direction = Vector3.right;
        }
        int randomСrystal = Random.Range(0, 100);
        foreach (var tile in poolTiles)
        {
            if (tile.gameObject.activeSelf == false)
            {
                vector = lastTile.position + direction;
                lastTile = tile;
                tile.position = vector;
                tile.gameObject.SetActive(true);
                visibleTiles.Add(tile);
                if (randomСrystal < 30)
                {
                    AddСrystal(vector);
                }
                return;
            }
        }
        vector = lastTile.position + direction;
        lastTile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
        poolTiles.Add(lastTile);
        visibleTiles.Add(lastTile);
    }
    void AddСrystal(Vector3 newVector)
    {
        foreach (var crystal in Сrystals)
        {
            if (crystal.gameObject.activeSelf == false)
            {
                crystal.gameObject.SetActive(true);
                crystal.position = newVector + Vector3.up;
                return;
            }
        }
        Transform newCrystal = Instantiate(prefabcrystal,newVector + Vector3.up, prefabcrystal.rotation);
        Сrystals.Add(newCrystal);
    }
    void RemoveTile()
    {
        visibleTiles[0].gameObject.SetActive(false);
        visibleTiles.RemoveAt(0);
    }
}
