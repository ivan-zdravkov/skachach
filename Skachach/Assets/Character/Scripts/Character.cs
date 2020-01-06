using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private const string AnimatorRunning = "IsRunning";
    private const string AnimatorJumping = "IsJumping";
    private const string AnimatorSliding = "IsSliding";
    private const string AnimatorIsDead = "IsDead";

    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpHeight = 150f;

    private Vector2 moveVector;
    private Animator animator;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
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
        if (IsMoving())
        {
            this.SetAnimation(AnimatorRunning);

            this.rigidBody.velocity = new Vector2(this.moveVector.x * this.runSpeed, this.rigidBody.velocity.y);

            if (this.moveVector.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (IsSliding())
        {
            this.SetAnimation(AnimatorSliding);
        }
        else
        {
            this.rigidBody.velocity = new Vector2(0, this.rigidBody.velocity.y);
            this.ResetAllAnimations();
        }
    }

    private bool IsMoving()
    {
        return this.moveVector != null && this.moveVector.x != 0;
    }

    private bool IsSliding()
    {
        return false;
    }

    private void Jump()
    {
        if (this.moveVector != null && this.moveVector.y > 0)
        {
            transform.position = transform.position + new Vector3(
                x: 0f,
                y: this.moveVector.y * this.jumpHeight * Time.deltaTime,
                z: 0f
            );
        }
    }

    private void ResetAllAnimations()
    {
        this.animator.SetBool(AnimatorRunning, false);
        this.animator.SetBool(AnimatorJumping, false);
        this.animator.SetBool(AnimatorSliding, false);
        this.animator.SetBool(AnimatorIsDead, false);

    }

    private void SetAnimation(string animation)
    {
        this.ResetAllAnimations();

        this.animator.SetBool(animation, true);
    }
}
