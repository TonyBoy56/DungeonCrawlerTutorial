using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    // type specification
    private Animator animator;
    private int food;

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


    void Update()
    {
        // if *it is* the players turn, we will follow through with the rest of the code block. If not, exit the statement.
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }
        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    // reload the level if the player collides with the exit object.
    private void Restart()
    {
        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(sceneName: "Main:");
    }

    // note: create modification to also check GameOver on current life-count/health
    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food -= 1;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }
}