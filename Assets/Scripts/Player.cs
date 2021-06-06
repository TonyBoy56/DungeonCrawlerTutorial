using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    // type specification
    private Animator animator;
    private int food;

    protected override void OnCantMove<T>(T component)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        // set food to value of playerFoodPoints
        // store value of food in the GM as we change levels
        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    // part of Unity API
    // occurs when the player dies, or is disabled
    private void OnDisable()
    {
        // store value of food into the GameManager as levels change.
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // note: create modification to also check GameOver on current life-count/health
    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
