using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class Playfield : MonoBehaviour
    {
        Transform prefabTile;
        Transform prefabcrystal;
        Transform parentTiles;
        Transform parentCrystals;
        List<Vector3> startPositions = new List<Vector3>();
        List<Transform> poolTiles = new List<Transform>();
        List<Transform> poolСrystals = new List<Transform>();
        List<Transform> visibleTiles = new List<Transform>();
        List<Vector3> sizeTilse = new List<Vector3>();
        List<IEnumerator> IEnumerators = new List<IEnumerator>();
        GameState gameState;
        int numberDirect;
        Vector3 directionTile;
        int layerSprite = 0;
        int crystalCoefficient;
        public void AddResources()
        {
            gameState = GetComponent<GameState>();
            prefabTile = Resources.Load<Transform>("Tile");
            prefabcrystal = Resources.Load<Transform>("Crystal");
            parentTiles = new GameObject().transform;
            parentCrystals = new GameObject().transform;
            sizeTilse.Add(new Vector3(1, 1, 1));
            sizeTilse.Add(new Vector3(2, 2, 2));
            sizeTilse.Add(new Vector3(3, 3, 3));
        }
        public void StartGame()
        {
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
            Transform newTile = Instantiate(prefabTile, vector3, prefabTile.rotation, parentTiles);
            newTile.localScale = sizeTilse[gameState.gameDifficulty - 1];
            poolTiles.Add(newTile);
            visibleTiles.Add(newTile);
        }
        void ChangeOrderSorting(Transform tile, int layer)
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
        void DistanceСheck(Vector3 playerPosition)
        {
            if (Vector3.Distance(visibleTiles[visibleTiles.Count - 1].position, playerPosition) < 10)
            {
                AddTile();
                // if (visibleTiles.Count > 24 / gameDifficulty)
                // {
                //     RemoveTile();
                // }
            }
        }
        void AddTile()
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
                    StartCoroutine(moveTile2);
                    IEnumerators.Add(moveTile2);
                    return;
                }
            }
            Transform newTile = Instantiate(prefabTile, vector, prefabTile.rotation, parentTiles);
            newTile.localScale = sizeTilse[gameState.gameDifficulty - 1];
            poolTiles.Add(newTile);
            visibleTiles.Add(newTile);
            ChangeOrderSorting(newTile, layerSprite);
            var moveTile = MoveTileAdd(newTile);
            StartCoroutine(moveTile);
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
    }
}