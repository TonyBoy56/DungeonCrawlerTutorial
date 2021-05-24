using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton is an object for which there can only be one instance in the game at any given time.
    // static will declare the variable will belong to the class itself rather than an instance of the class.
    public static GameManager instance = null;
    public BoardManager boardScript;

    private int level = 3;

    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
        {
            // create new instance of GameMananger if == null
            instance = this;
        }
        else if (instance != this)
        {
            // if there is an existing instance of GameManager,   we will destroy it upon load of new level to prevent two running instances
            // GameManager is referred to as gameObject
            Destroy(gameObject);
        }

        // when a new scene is loaded, normally all gameObjects in the heirarchy are destroyed. 
        // We want to use the GameManager to keep track of score between scenes
        // DontDestroyOnLoad allows gameObjects to persist between loaded scenes.
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame ()
    {
        boardScript.SetupScene(level);
    }

    // Update is called once per frame
    void Update ()
    {
        
    }
}
