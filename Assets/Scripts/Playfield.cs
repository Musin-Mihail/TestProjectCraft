using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Playfield
{
    List<Vector3> startPositions = new List<Vector3>();
    public static int gameDifficulty;
    AddRemoveObject addRemoveObject;
    float maximumDistanceToTile;
    float maximumDistanceToCrystal = 1.2f;
    public void Initialization()
    {
        addRemoveObject = new AddRemoveObject();
        addRemoveObject.Initialization();
    }
    public void StartGame(int gameDifficulty)
    {
        maximumDistanceToTile = 0.72f * gameDifficulty;
        Playfield.gameDifficulty = gameDifficulty;
        addRemoveObject.StartGame(gameDifficulty);
        CreateStartPosition();
        Generation();
    }
    void CreateStartPosition()
    {
        startPositions.Clear();
        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 3; x++)
            {
                Vector3 vector = new Vector3(x, 0, z) * gameDifficulty;
                startPositions.Add(vector);
            }
        }
        startPositions.Add(new Vector3(1, 0, 3) * gameDifficulty);
        startPositions.Add(new Vector3(1, 0, 4) * gameDifficulty);
    }
    void Generation()
    {
        if (addRemoveObject.poolTiles.Count == 0)
        {
            foreach (var vector3 in startPositions)
            {
                addRemoveObject.CreateTile(vector3);
            }
        }
        else
        {
            foreach (var vector3 in startPositions)
            {
                foreach (var tile in addRemoveObject.poolTiles)
                {
                    if (tile.gameObject.activeSelf == false)
                    {
                        tile.localScale = addRemoveObject.sizeTilse[gameDifficulty - 1];
                        tile.gameObject.SetActive(true);
                        tile.position = vector3;
                        addRemoveObject.visibleTiles.Add(tile);
                        break;
                    }
                }
            }
        }
        addRemoveObject.ChangeOrderSorting(addRemoveObject.visibleTiles[addRemoveObject.visibleTiles.Count - 1], 0);
    }
    public bool CheckingDistanceToTiles(Vector3 playerPosition)
    {
        foreach (var tile in addRemoveObject.poolTiles)
        {
            if (tile.gameObject.activeSelf)
            {
                if (Vector3.Distance(tile.position, playerPosition) < maximumDistanceToTile)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckingDistanceToCrystals(Vector3 playerPosition)
    {
        foreach (var crystal in addRemoveObject.poolСrystals)
        {
            if (crystal.gameObject.activeSelf)
            {
                if (Vector3.Distance(crystal.position, playerPosition) < maximumDistanceToCrystal)
                {
                    crystal.gameObject.SetActive(false);
                    return true;
                }
            }
        }
        return false;
    }
    public void AddNextTile(Vector3 playerPosition)
    {
        if (Vector3.Distance(addRemoveObject.visibleTiles[addRemoveObject.visibleTiles.Count - 1].position, playerPosition) < 10)
        {
            addRemoveObject.AddTile();
            if (addRemoveObject.visibleTiles.Count > 24 / gameDifficulty)
            {
                addRemoveObject.RemoveTile();
            }
        }
    }
}
public class AddRemoveObject
{
    Vector3 directionTile;
    int numberDirect;
    public int layerSprite = 0;
    public List<Transform> visibleTiles = new List<Transform>();
    public List<Transform> poolTiles = new List<Transform>();
    public List<Vector3> sizeTilse = new List<Vector3>();
    public List<IEnumerator> IEnumerators = new List<IEnumerator>();
    public Instantiate Create;
    public int crystalCoefficient;
    public List<Transform> poolСrystals = new List<Transform>();
    public Coroutine coroutine;
    public void StartGame(int gameDifficulty)
    {
        layerSprite = 0;
        crystalCoefficient = 0;
        foreach (var tile in poolTiles)
        {
            tile.gameObject.SetActive(false);
            ChangeOrderSorting(tile, 1);
        }
        foreach (var item in IEnumerators)
        {
            coroutine.stopCoroutine(item);
        }
        foreach (var crystal in poolСrystals)
        {
            crystal.gameObject.SetActive(false);
        }
        IEnumerators.Clear();
        visibleTiles.Clear();
    }
    public void Initialization()
    {
        Create = new GameObject().AddComponent<Instantiate>();
        coroutine = new GameObject().AddComponent<Coroutine>();
        Create.Initialization();
        sizeTilse.Add(new Vector3(1, 1, 1));
        sizeTilse.Add(new Vector3(2, 2, 2));
        sizeTilse.Add(new Vector3(3, 3, 3));
    }
    public void AddTile()
    {
        int randomTile = Random.Range(0, 100);
        if (numberDirect < 5)
        {
            if (randomTile < 80)
            {
                directionTile = Vector3.forward * Playfield.gameDifficulty;
                numberDirect++;
                layerSprite--;
            }
            else
            {
                directionTile = Vector3.right * Playfield.gameDifficulty;
                layerSprite++;
            }
        }
        else
        {
            numberDirect = 0;
            directionTile = Vector3.right * Playfield.gameDifficulty;
            layerSprite++;
        }
        Vector3 vector = visibleTiles[visibleTiles.Count - 1].position + directionTile;
        foreach (var tile in poolTiles)
        {
            if (tile.gameObject.activeSelf == false)
            {
                tile.position = vector;
                tile.gameObject.SetActive(true);
                visibleTiles.Add(tile);
                tile.localScale = sizeTilse[Playfield.gameDifficulty - 1];
                ChangeOrderSorting(tile, layerSprite);
                var moveTile2 = MoveTileAdd(tile);
                coroutine.startCoroutine(moveTile2);
                IEnumerators.Add(moveTile2);
                return;
            }
        }
        Transform newTile = Create.InstantiateNewTile(vector);
        newTile.localScale = sizeTilse[Playfield.gameDifficulty - 1];
        poolTiles.Add(newTile);
        visibleTiles.Add(newTile);
        ChangeOrderSorting(newTile, layerSprite);
        var moveTile = MoveTileAdd(newTile);
        coroutine.startCoroutine(moveTile);
        IEnumerators.Add(moveTile);
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
    public void RemoveTile()
    {
        var moveTile = MoveTileRemove(visibleTiles[0]);
        coroutine.startCoroutine(moveTile);
        IEnumerators.Add(moveTile);
        visibleTiles.RemoveAt(0);
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
    public void AddСrystal(Vector3 newVector)
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
            Transform newCrystal = Create.InstantiateNewCrystal(newVector);
            poolСrystals.Add(newCrystal);
        }
        else
        {
            crystalCoefficient += 10;
        }
    }
    public void ChangeOrderSorting(Transform tile, int layer)
    {
        for (int i = 0; i < tile.childCount; i++)
        {
            tile.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = layer;
        }
    }
    public void CreateTile(Vector3 vector3)
    {
        Transform newTile = Create.InstantiateNewTile(vector3);
        newTile.localScale = sizeTilse[Playfield.gameDifficulty - 1];
        poolTiles.Add(newTile);
        visibleTiles.Add(newTile);
        ChangeOrderSorting(newTile, layerSprite);
    }
}
public class Instantiate : MonoBehaviour
{
    public Transform prefabTile;
    public Transform prefabcrystal;
    public Transform parentTiles;
    public Transform parentCrystals;
    public void Initialization()
    {
        prefabTile = Resources.Load<Transform>("Tile");
        parentTiles = new GameObject().transform;
        prefabcrystal = Resources.Load<Transform>("Crystal");
        parentCrystals = new GameObject().transform;
    }
    public Transform InstantiateNewCrystal(Vector3 newVector)
    {
        return Instantiate(prefabcrystal, newVector + Vector3.up, prefabcrystal.rotation, parentCrystals);
    }
    public Transform InstantiateNewTile(Vector3 newVector)
    {
        return Instantiate(prefabTile, newVector, prefabTile.rotation, parentTiles);
    }
}
public class Coroutine : MonoBehaviour
{
    public void startCoroutine(IEnumerator iEnumerator)
    {
        StartCoroutine(iEnumerator);
    }
    public void stopCoroutine(IEnumerator iEnumerator)
    {
        StopCoroutine(iEnumerator);
    }
}