using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Playfield : MonoBehaviour
{
    public static Playfield2 playfield2 = new Playfield2();
    public Playfield2 ttt()
    {
        return playfield2;
    }
}

public class Playfield2
{
    List<Vector3> startPositions = new List<Vector3>();
    protected List<Transform> poolTiles = new List<Transform>();
    protected int layerSprite = 0;
    public GameState gameState;
    protected List<Transform> visibleTiles = new List<Transform>();
    public List<Vector3> sizeTilse = new List<Vector3>();
    protected List<IEnumerator> IEnumerators = new List<IEnumerator>();
    public Transform parentTiles;
    public Transform prefabTile;
    protected int crystalCoefficient;
    protected List<Transform> poolСrystals = new List<Transform>();
    public Transform prefabcrystal;
    public Transform parentCrystals;
    public void StartGame()
    {
        gameState.move = false;
        layerSprite = 0;
        crystalCoefficient = 0;
        foreach (var tile in poolTiles)
        {
            tile.gameObject.SetActive(false);
            ChangeOrderSorting(tile, 1);
        }
        foreach (var item in IEnumerators)
        {
            var coroutine = new Coroutine();
            coroutine.stopCoroutine(item);
        }
        foreach (var crystal in poolСrystals)
        {
            crystal.gameObject.SetActive(false);
        }
        IEnumerators.Clear();
        visibleTiles.Clear();
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
                Vector3 vector = new Vector3(x, 0, z) * gameState.gameDifficulty;
                startPositions.Add(vector);
            }
        }
        startPositions.Add(new Vector3(1, 0, 3) * gameState.gameDifficulty);
        startPositions.Add(new Vector3(1, 0, 4) * gameState.gameDifficulty);
    }
    void Generation()
    {
        if (poolTiles.Count == 0)
        {
            foreach (var vector3 in startPositions)
            {
                CreateTile(vector3);
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
                        tile.localScale = sizeTilse[gameState.gameDifficulty - 1];
                        tile.gameObject.SetActive(true);
                        tile.position = vector3;
                        visibleTiles.Add(tile);
                        break;
                    }
                }
            }

        }
        ChangeOrderSorting(visibleTiles[visibleTiles.Count - 1], 0);
    }
    void CreateTile(Vector3 vector3)
    {
        Transform newTile = Instantiate.CreateTile(vector3);
        newTile.localScale = sizeTilse[gameState.gameDifficulty - 1];
        poolTiles.Add(newTile);
        visibleTiles.Add(newTile);
    }
    protected void ChangeOrderSorting(Transform tile, int layer)
    {
        for (int i = 0; i < tile.childCount; i++)
        {
            tile.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = layer;
        }
    }
    public bool CheckingDistanceToTiles(Vector3 playerPosition)
    {
        foreach (var tile in poolTiles)
        {
            if (tile.gameObject.activeSelf)
            {
                if (Vector3.Distance(tile.position, playerPosition) < 0.72f * gameState.gameDifficulty)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckingDistanceToCrystals(Vector3 playerPosition)
    {
        foreach (var crystal in poolСrystals)
        {
            if (crystal.gameObject.activeSelf)
            {
                if (Vector3.Distance(crystal.position, playerPosition) < 1.2f)
                {
                    crystal.gameObject.SetActive(false);
                    return true;
                }
            }
        }
        return false;
    }
    public void DistanceСheck(Vector3 playerPosition)
    {
        if (Vector3.Distance(visibleTiles[visibleTiles.Count - 1].position, playerPosition) < 10)
        {
            var AddTile = new AddRemoveObject();
            AddTile.AddTile();
            if (visibleTiles.Count > 24 / gameState.gameDifficulty)
            {
                AddTile.RemoveTile();
            }
        }
    }
}
public class AddRemoveObject : Playfield2
{
    Vector3 directionTile;
    int numberDirect;
    public void AddTile()
    {
        int randomTile = Random.Range(0, 100);
        if (numberDirect < 5)
        {
            if (randomTile < 80)
            {
                directionTile = Vector3.forward * gameState.gameDifficulty;
                numberDirect++;
                layerSprite--;
            }
            else
            {
                directionTile = Vector3.right * gameState.gameDifficulty;
                layerSprite++;
            }
        }
        else
        {
            numberDirect = 0;
            directionTile = Vector3.right * gameState.gameDifficulty;
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
                tile.localScale = sizeTilse[gameState.gameDifficulty - 1];
                ChangeOrderSorting(tile, layerSprite);
                var moveTile2 = MoveTileAdd(tile);
                var coroutine2 = new Coroutine();
                coroutine2.startCoroutine(moveTile2);
                IEnumerators.Add(moveTile2);
                return;
            }
        }
        Transform newTile = Instantiate.CreateTile(vector);
        newTile.localScale = sizeTilse[gameState.gameDifficulty - 1];
        poolTiles.Add(newTile);
        visibleTiles.Add(newTile);
        ChangeOrderSorting(newTile, layerSprite);
        var moveTile = MoveTileAdd(newTile);
        var coroutine = new Coroutine();
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
        var coroutine = new Coroutine();
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
            Transform newCrystal = Instantiate.CreateCrystal(newVector);
            poolСrystals.Add(newCrystal);
        }
        else
        {
            crystalCoefficient += 10;
        }
    }
}
public class Instantiate : MonoBehaviour
{
    public static Transform CreateCrystal(Vector3 newVector)
    {
        return Instantiate(Playfield.playfield2.prefabcrystal, newVector + Vector3.up, Playfield.playfield2.prefabcrystal.rotation, Playfield.playfield2.parentCrystals);
    }
    public static Transform CreateTile(Vector3 newVector)
    {
        return Instantiate(Playfield.playfield2.prefabTile, newVector, Playfield.playfield2.prefabTile.rotation, Playfield.playfield2.parentTiles);
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