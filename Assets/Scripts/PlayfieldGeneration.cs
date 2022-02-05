using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayfieldGeneration : MonoBehaviour
{
    public Transform player;
    public Transform prefabTile;
    public Transform prefabcrystal;
    List<Transform> poolTiles;
    List<Transform> poolСrystals;
    public Transform parentTiles;
    public Transform parentCrystals;
    List<Vector3> startPositions;
    List<Vector3> sizeTilse;
    List<Transform> visibleTiles;
    Vector3 vector;
    Vector3 direction;
    int numberDirect;
    int crystalCoefficient;
    int gameDifficulty = 1;
    void Start()
    {
        poolСrystals = new List<Transform>();
        poolTiles = new List<Transform>();
        visibleTiles = new List<Transform>();
        startPositions = new List<Vector3>();
        sizeTilse = new List<Vector3>();
        sizeTilse.Add(new Vector3(1, 1, 1));
        sizeTilse.Add(new Vector3(2, 2, 2));
        sizeTilse.Add(new Vector3(3, 3, 3));
        CreateStartPosition();
        NewGame();
    }
    void CreateStartPosition()
    {
        startPositions.Clear();
        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 3; x++)
            {
                vector = new Vector3(x, 0, z) * gameDifficulty;
                startPositions.Add(vector);
            }
        }
        startPositions.Add(new Vector3(1, 0, 3) * gameDifficulty);
        startPositions.Add(new Vector3(1, 0, 4) * gameDifficulty);
    }
    void NewGame()
    {
        player.position = new Vector3(1, 0, 1.5f) * gameDifficulty;
        PrimaryGeneration();
        StartCoroutine(DistanceСheck());

    }
    void PrimaryGeneration()
    {
        if (poolTiles.Count == 0)
        {
            foreach (var vector3 in startPositions)
            {
                Transform newTile = Instantiate(prefabTile, vector3, prefabTile.rotation, parentTiles);
                newTile.localScale = sizeTilse[gameDifficulty - 1];
                poolTiles.Add(newTile);
                visibleTiles.Add(newTile);
            }
        }
        else
        {
            foreach (var vector3 in startPositions)
            {
                foreach (var tile in poolTiles)
                {
                    if (tile.gameObject.activeSelf == false)
                    {
                        tile.gameObject.SetActive(true);
                        tile.position = vector3;
                        visibleTiles.Add(tile);
                        tile.localScale = sizeTilse[gameDifficulty - 1];
                        break;
                    }
                }
            }
        }
    }
    IEnumerator DistanceСheck()
    {
        while (true)
        {
            if (Vector3.Distance(visibleTiles[visibleTiles.Count - 1].position, player.position) < 10)
            {
                AddTile();
                if (visibleTiles.Count > 24 / gameDifficulty)
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
                direction = Vector3.forward * gameDifficulty;
                numberDirect++;
            }
            else
            {
                direction = Vector3.right * gameDifficulty;
            }
        }
        else
        {
            numberDirect = 0;
            direction = Vector3.right * gameDifficulty;
        }
        vector = visibleTiles[visibleTiles.Count - 1].position + direction;
        AddСrystal(vector);
        foreach (var tile in poolTiles)
        {
            if (tile.gameObject.activeSelf == false)
            {
                tile.position = vector;
                tile.gameObject.SetActive(true);
                visibleTiles.Add(tile);
                tile.localScale = sizeTilse[gameDifficulty - 1];
                return;
            }
        }
        Transform newTile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
        newTile.localScale *= gameDifficulty;
        poolTiles.Add(newTile);
        visibleTiles.Add(newTile);
    }
    void AddСrystal(Vector3 newVector)
    {
        int randomСrystal = Random.Range(0, 100);
        randomСrystal -= crystalCoefficient;
        if (randomСrystal < 10)
        {
            crystalCoefficient = 0;
            foreach (var crystal in poolСrystals)
            {
                if (crystal.gameObject.activeSelf == false)
                {
                    crystal.gameObject.SetActive(true);
                    crystal.position = newVector + Vector3.up;
                    return;
                }
            }
            Transform newCrystal = Instantiate(prefabcrystal, newVector + Vector3.up, prefabcrystal.rotation, parentCrystals);
            poolСrystals.Add(newCrystal);
        }
        else
        {
            crystalCoefficient += 10;
        }
    }
    void RemoveTile()
    {
        visibleTiles[0].gameObject.SetActive(false);
        visibleTiles.RemoveAt(0);
    }
    public void RestartGame()
    {
        foreach (var tile in poolTiles)
        {
            tile.gameObject.SetActive(false);
        }
        foreach (var crystal in poolСrystals)
        {
            crystal.gameObject.SetActive(false);
        }
        visibleTiles.Clear();
        crystalCoefficient = 0;
        NewGame();
    }
    public bool CheckingDistanceToTiles()
    {
        foreach (var tile in poolTiles)
        {
            if (tile.gameObject.activeSelf)
            {
                if (Vector3.Distance(tile.position, player.position) < 0.72f * gameDifficulty)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckingDistanceToCrystals()
    {
        foreach (var crystal in poolСrystals)
        {
            if (crystal.gameObject.activeSelf)
            {
                if (Vector3.Distance(crystal.position, player.position) < 1.2f)
                {
                    crystal.gameObject.SetActive(false);
                    return true;
                }
            }
        }
        return false;
    }
    public void ChangeGameDifficulty(int value)
    {
        gameDifficulty = value;
        CreateStartPosition();
        RestartGame();
    }
}
