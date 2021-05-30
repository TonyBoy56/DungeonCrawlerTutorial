using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract prevents the use of MovingObject class directly.
// other classes can derive from MovingObject without making an object of MovingObject directly.
// this allows classes such as Enemy and Player to inherit from, but not be direct objects of the MovingObject class.
// Enemy and Player will likely have their own subclasses to differentiate from MovingObject.
public abstract class MovingObject : MonoBehaviour
{
    // time for the object to move in secs
    public float moveTime = 0.1f;
    // layer by which we will check collision to determine if a space can be moved into
    public LayerMask blockingLayer;

    // declare boxCollider as a var of type BoxCollider2D. Instance of BoxCollider2D class
    private BoxCollider2D boxCollider;
    // declare rb2D as a var of type Rigidbody2D. Instance of RigidBody2D class
    private Rigidbody2D rb2D;
    // will use later to make movement calcs more efficient
    private float inverseMoveTime;

    // can be overridden by inherited classes (such as Enemy and Player classes)
    protected virtual void Start()
    {
        // get component reference of BoxCollider2D. Generic for later use in other cases
        boxCollider = GetComponent<BoxCollider2D>();
        // get component reference of Rigidbody2D. Generic for later use in other cases
        rb2D = GetComponent<Rigidbody2D>();
        // inverseMoveTime = 10; computationally more effecient for later use
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovementCoroutine(end));
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }
        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    // Coroutine. This is a function that is excecuted in 'intervals'
    // Moves units from one space to the next
    protected IEnumerator SmoothMovementCoroutine(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        // check that the remainingDistance is greater than 'almost-zero'
        while (sqrRemainingDistance > float.Epsilon)
        {
            // find a new position that is proportionally closer to the end based on the move time
            // moves a point in a straight line towards a target point
            // value return by Vector3.MoveTowards 
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            // move to newPosition that we found
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
