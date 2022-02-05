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
    List<IEnumerator> IEnumerators;
    Vector3 vector;
    Vector3 direction;
    int numberDirect;
    int crystalCoefficient;
    int gameDifficulty = 3;
    int layerSprite = 0;
    void Start()
    {
        poolСrystals = new List<Transform>();
        poolTiles = new List<Transform>();
        visibleTiles = new List<Transform>();
        IEnumerators = new List<IEnumerator>();
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
                        tile.localScale = sizeTilse[gameDifficulty - 1];
                        tile.gameObject.SetActive(true);
                        tile.position = vector3;
                        visibleTiles.Add(tile);
                        break;
                    }
                }
            }

        }
        for (int i = 0; i < visibleTiles[visibleTiles.Count - 1].childCount; i++)
        {
            visibleTiles[visibleTiles.Count - 1].GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 0;
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
                layerSprite--;
            }
            else
            {
                direction = Vector3.right * gameDifficulty;
                layerSprite++;
            }
        }
        else
        {
            numberDirect = 0;
            direction = Vector3.right * gameDifficulty;
            layerSprite++;
        }
        vector = visibleTiles[visibleTiles.Count - 1].position + direction;
        foreach (var tile in poolTiles)
        {
            if (tile.gameObject.activeSelf == false)
            {
                tile.position = vector;
                tile.gameObject.SetActive(true);
                visibleTiles.Add(tile);
                tile.localScale = sizeTilse[gameDifficulty - 1];
                for (int i = 0; i < tile.childCount; i++)
                {
                    tile.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = layerSprite;
                }
                var moveTile2 = MoveTileAdd(tile);
                StartCoroutine(moveTile2);
                IEnumerators.Add(moveTile2);
                return;
            }
        }
        Transform newTile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
        newTile.localScale = sizeTilse[gameDifficulty - 1];
        poolTiles.Add(newTile);
        visibleTiles.Add(newTile);
        for (int i = 0; i < newTile.childCount; i++)
        {
            newTile.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = layerSprite;
        }
        var moveTile = MoveTileAdd(newTile);
        StartCoroutine(moveTile);
        IEnumerators.Add(moveTile);
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
        var moveTile = MoveTileRemove(visibleTiles[0]);
        StartCoroutine(moveTile);
        IEnumerators.Add(moveTile);
        visibleTiles.RemoveAt(0);
    }
    IEnumerator MoveTileAdd(Transform tile)
    {
        Vector3 finishVector = tile.position;
        finishVector.y = 0;
        tile.position = finishVector + Vector3.down * 4;
        while (tile.position != finishVector)
        {
            tile.position = Vector3.MoveTowards(tile.position, finishVector, 0.04f);
            yield return new WaitForSeconds(0.01f);
        }
        AddСrystal(tile.position);
    }
    IEnumerator MoveTileRemove(Transform tile)
    {
        Vector3 newVector = tile.position + Vector3.down * 4;
        while (tile.position != newVector)
        {
            tile.position = Vector3.MoveTowards(tile.position, newVector, 0.04f);
            yield return new WaitForSeconds(0.01f);
        }
        tile.gameObject.SetActive(false);
    }
    public void RestartGame()
    {
        foreach (var tile in poolTiles)
        {
            tile.gameObject.SetActive(false);
            for (int i = 0; i < tile.childCount; i++)
            {
                tile.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
        foreach (var crystal in poolСrystals)
        {
            crystal.gameObject.SetActive(false);
        }
        foreach (var item in IEnumerators)
        {
            StopCoroutine(item);
        }
        layerSprite = 0;
        IEnumerators.Clear();
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
