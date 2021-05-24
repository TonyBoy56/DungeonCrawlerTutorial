using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // storing current state of Count in physical storage. Will be 'deserialized' when we use it later in memory
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
     // Establish deminsional grid format of actual gameboard
    public int columns = 8;
    public int rows = 8;
    // Specify random range for how many walls we want to spawn in each level
    public Count wallCount = new Count(5, 9);
    // Specify random range for how many food items we want to spawn in each level
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    // use arrays to pass in multiple tiles to parse through and appear on the gameboard
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    // Keep clean hierarchy. Will be spawning in multiple objects as instances of boardHolder;
    private Transform boardHolder;
    // Track all possible positions on the gameboard. Track if object can be spawned on that position or not.
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();

        // Create list of possible positions to place walls, enemies, or pickups
        // Looping from 1 to col/row -1 so that there can be a border of floor tiles directly within the outer walls. Creates levels that aren't completely impassable
        // attribute - 1 because we don't want new tiles to spawn on the outerwall tiles
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
;           }
        }
    }

    // Setup outerwall and floor of the game board
    void BoardSetup ()
    {
        // create new GameObject called 'Board' with transform properties. Reference as 'boardHolder'.
        boardHolder = new GameObject("Board").transform;

        // x = -1 and y = -1 to establish the edge around the active portion of the gameboard using outerwall objects
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                    // Quaternion.identity because we want to instantiate without rotation
                    // as GameObject so that it is passed to a GameObject
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                }
                else
                {
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                }
            }
        }
    }

    Vector3 RandomPosition()
    {
        // generate random index from 0 to available grid positions through gridPositions.Count
        int randomIndex = Random.Range(0, gridPositions.Count);
        // store the randomIndex in a randomPosition as a Vector3
        Vector3 randomPosition = gridPositions[randomIndex];
        // prevents two objects being spawned in the same location.
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    // function to spawn tile at the chosen random position
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // controls how many of a given object will spawn
        int objectCount = Random.Range(minimum, maximum + 1);

        // spawn number of objects specified by objectCount
        for (int i = 0; i < objectCount; i++)
        {
            // 
            Vector3 randomPosition = RandomPosition();
            // assign a random tile to tileChoice as GameObject
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            // create instance of tileChoice at a randomPosition without rotation (Quaternion.identity)
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // only public function of the class
    // will be called GameManager when it's time to setup the board
    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        // generate number of enemies based on the level number. Utilizes logarithmic progression scale. Mathf.Log returns a float, and is now cast to an int
        int enemyCount = (int)Mathf.Log(level, 2f);
        // min and max vals are the same due to not specifying a random range
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        // instance exit. Will always be in the upper right corner in this game.
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);
    }
}
