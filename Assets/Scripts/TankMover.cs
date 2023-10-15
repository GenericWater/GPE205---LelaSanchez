using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    //Variable to hold our rigidbody Component
    private Rigidbody rb;

    private float rotate;
    private float move;
    public float turnSpeed; // Created a variable for the degrees we will rotate in a frame draw.
    public float moveSpeed; // Created a public variable for move speed - will be framereate independent.


    // Start is called before the first frame update
    public override void Start()
    {
        //Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        moveSpeed = 5f;
        turnSpeed = 50f;
    }

    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
    }

    public void Update()
    {
        move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        rotate = Input.GetAxis("Horizontal") * - turnSpeed * Time.deltaTime;
    }

    public void LateUpdate()
    {
        transform.Translate(0f, 0f, move);
        transform.Rotate(0f, rotate, 0f);
    }

    
}
