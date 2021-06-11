using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    // number of food points taken from the player when damaged. 
    // Note: account for health in future mods.
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Start()
    {
        // grab stored component reference to the animator
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    // takes generic param <T> of any component type
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // cause the enemy to move every other turn
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        // calc position of the player against position of the enemy
        // return absolute value of f if less than, or roughly the same, as Epsilon
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            // +1 move up, -1 move down
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            // +1 move right, -1 move left
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        // account for health bar here as mod

        hitPlayer.LoseFood(playerDamage);
    }
}
