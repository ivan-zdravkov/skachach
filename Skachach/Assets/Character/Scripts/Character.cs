using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private const float VERTICLE_VELOCITY_TRESHHOLD = 0.1f;

    private const string RUNNING = "IsRunning";
    private const string GOING_UP = "IsGoingUp";
    private const string GOING_DOWN = "IsGoingDown";
    private const string SLIDING = "IsSliding";
    private const string DEAD = "IsDead";

    private Vector2 moveVector;
    private Animator animator;
    private Rigidbody2D rigidBody;

    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpHeight = 0.01f;

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        this.moveVector = context.ReadValue<Vector2>();
    }

    public bool Running { get { return this.animator.GetBool(RUNNING); } }
    public bool Sliding { get { return this.animator.GetBool(SLIDING); } }
    public bool GoingUp { get { return this.animator.GetBool(GOING_UP); } }
    public bool GoingDown { get { return this.animator.GetBool(GOING_DOWN); } }
    public bool Airbourne { get { return this.animator.GetBool(GOING_DOWN) || this.animator.GetBool(GOING_UP); } }

    private void HandleMovement()
    {
        if (IsSliding())
            { /* Nothing, slowly stop moving */ }
        else if (IsMoveVectorSet())
            Move();
        else
            StopMoving();

        if (IsOnTheGround() && ShouldJump())
            Jump();

        if (IsGoingUp())
            SetAnimation(GOING_UP);
        else if (IsGoingDown())
            SetAnimation(GOING_DOWN);
        else if (IsOnTheGround())
        {
            if (IsSliding() && IsMovingLeftOrRight())
                SetAnimation(SLIDING);
            else if (IsMovingLeftOrRight())
                SetAnimation(RUNNING);
            else
                ResetAllAnimations();
        }
        else
            ResetAllAnimations();
    }

    private bool IsOnTheGround()
    {
        return Math.Abs(this.rigidBody.velocity.y) <= VERTICLE_VELOCITY_TRESHHOLD;
    }

    private bool IsMovingLeftOrRight()
    {
        return this.rigidBody.velocity.x != 0;
    }

    private bool IsGoingUp()
    {
        return this.rigidBody.velocity.y > VERTICLE_VELOCITY_TRESHHOLD;
    }

    private bool IsGoingDown()
    {
        return this.rigidBody.velocity.y < -VERTICLE_VELOCITY_TRESHHOLD;
    }

    private void Move()
    {
        this.rigidBody.velocity = new Vector2(this.moveVector.x * this.runSpeed, this.rigidBody.velocity.y);

        if (this.moveVector.x > 0)
            transform.localScale = new Vector3(1, 1, 1); //Face Left
        else
            transform.localScale = new Vector3(-1, 1, 1); //Face Right
    }

    private void StopMoving()
    {
        this.rigidBody.velocity = new Vector2(0, this.rigidBody.velocity.y);
    }

    private bool IsMoveVectorSet()
    {
        return this.moveVector != null && this.moveVector.x != 0;
    }

    private bool IsSliding()
    {
        return Keyboard.current.leftShiftKey.isPressed;
    }

    private bool ShouldJump()
    {
        return Keyboard.current.spaceKey.wasPressedThisFrame;
    }

    private void Jump()
    {
        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.jumpHeight);
    }

    private void ResetAllAnimations()
    {
        this.animator.SetBool(RUNNING, false);
        this.animator.SetBool(GOING_UP, false);
        this.animator.SetBool(GOING_DOWN, false);
        this.animator.SetBool(SLIDING, false);
        this.animator.SetBool(DEAD, false);

    }

    private void SetAnimation(string animation)
    {
        this.ResetAllAnimations();

        this.animator.SetBool(animation, true);
    }
}
