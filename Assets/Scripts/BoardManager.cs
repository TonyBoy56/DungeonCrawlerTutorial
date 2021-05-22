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
     // Establish grid format of actual gameboard
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    // use arrays to pass in multiple tiles to parse through and appear on the gameboard
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    // Keep clean hierarchy. Will be spawning in multiple game objects as children of boardHolder;
    private Transform boardHolder;
    // Track all possible positions on the gameboard. Track if object can be spawned on that position or not.
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();

        // Create list of possible positions to place walls, enemies, or pickups
        // Looping from 1 to -1 so that there can be a border of floor tiles directly within the outer walls. Creates levels that aren't completely impassable
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; x < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f))
;           }
        }
    }

    void BoardSetup ()
    {
        boardHolder = new GameObject("Board").transform;

        // Used to build an edge of the playable board using the outerwall objects
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
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
