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
    // Start is called before the first frame update
    protected override void Start()
    {
        // grab stored component reference to the animator
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
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


    protected override void OnCantMove<T>(T component)
    {
        throw new System.NotImplementedException();
    }
}
