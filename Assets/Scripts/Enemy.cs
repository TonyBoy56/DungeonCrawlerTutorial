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
}
