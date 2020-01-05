using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5.0f;
    [SerializeField] float jumpHeight = 150f;

    private Vector2 moveVector;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    public void Move(InputAction.CallbackContext context)
    {
        this.moveVector = context.ReadValue<Vector2>();

        this.Jump();
    }

    private void Run()
    {
        if (this.moveVector != null && this.moveVector.x != 0)
        {
            transform.position = transform.position + new Vector3(
                x: this.moveVector.x * this.movementSpeed * Time.deltaTime,
                y: 0f,
                z: 0f
            );
        }
    }

    private void Jump()
    {
        if(this.moveVector != null && this.moveVector.y > 0)
        {
            transform.position = transform.position + new Vector3(
                x: 0f,
                y: this.moveVector.y * this.jumpHeight * Time.deltaTime,
                z: 0f
            );
        }
    }
}
