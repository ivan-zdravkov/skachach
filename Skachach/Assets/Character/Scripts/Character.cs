using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private const float HURT_DURATION = 2f;
    private const float VERTICLE_VELOCITY_TRESHHOLD = 0.25f;

    private const string RUNNING = "IsRunning";
    private const string GOING_UP = "IsGoingUp";
    private const string GOING_DOWN = "IsGoingDown";
    private const string SLIDING = "IsSliding";
    private const string DEAD = "IsDead";

    private int coins = 0;
    private int health = 3;
    private int maxHealth = 3;

    private Vector2 moveVector;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    [SerializeField] int lives = 3;
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpHeight = 0.01f;
    [SerializeField] float sprintModifier = 1.5f;

    [SerializeField] GameObject coinsDisplayGameObject;
    [SerializeField] GameObject livesDisplayGameObject;

    private TextMeshProUGUI coinsDisplay;
    private TextMeshProUGUI livesDisplay;

    private HeartsDisplay heartsDisplay;
    private CharacterHead characterHead;

    private float originalRunSpeed;

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.heartsDisplay = FindObjectOfType<HeartsDisplay>();
        this.characterHead = FindObjectOfType<CharacterHead>();
        this.spriteRenderer = FindObjectOfType<SpriteRenderer>();
        this.coinsDisplay = this.coinsDisplayGameObject.GetComponent<TextMeshProUGUI>();
        this.livesDisplay = this.livesDisplayGameObject.GetComponent<TextMeshProUGUI>();

        this.heartsDisplay.UpdateHealthDisplay(this.health);

        this.originalRunSpeed = this.runSpeed;
    }

    void Update()
    {
        Move();
        Face();
        Animate();
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

    private void Move()
    {
        if (IsSliding())
        { /* Nothing, slowly stop moving */ }
        else if (IsMoveVectorSet())
        {
            if (this.IsSprinting())
                this.Sprint();
            else
                this.Run();
        }
        else
            StopMoving();

        if (IsOnTheGround() && ShouldJump())
            Jump();
    }

    private void Animate()
    {
        ChangeAnimationSpeed(1.0f * this.runSpeed / this.originalRunSpeed);

        if (IsGoingUp())
            SetAnimation(GOING_UP);
        else if (IsGoingDown())
            SetAnimation(GOING_DOWN);
        else if (IsOnTheGround())
        {
            if (IsSliding() && IsMovingLeftOrRight())
                SetAnimation(SLIDING);
            else if (IsMovingLeftOrRight())
            {
                SetAnimation(RUNNING);

                if (this.IsSprinting())
                    ChangeAnimationSpeed(this.sprintModifier * this.runSpeed / this.originalRunSpeed);
            }
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

    public void Sprint()
    {
        this.GiveVelocity(this.sprintModifier);
    }

    public void Run()
    {
        this.GiveVelocity();
    }

    private void GiveVelocity(float modifier = 1.0f)
    {
        this.rigidBody.velocity = new Vector2(this.moveVector.x * this.runSpeed * modifier, this.rigidBody.velocity.y);
    }

    private void Face()
    {
        if (!this.IsSliding() && this.moveVector.x != 0)
        {
            transform.localScale = new Vector3(Math.Sign(this.moveVector.x), 1, 1);
        }
    }

    private void StopMoving()
    {
        this.rigidBody.velocity = new Vector2(0, this.rigidBody.velocity.y);
    }

    private bool IsMoveVectorSet()
    {
        return this.moveVector != null && this.moveVector.x != 0;
    }

    private bool IsSprinting()
    {
        return this.CanSprint() && (Keyboard.current.leftShiftKey.isPressed || 
            Keyboard.current.rightShiftKey.isPressed);
    }

    private bool IsSliding()
    {
        return Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
    }

    private bool CanSprint()
    {
        return true;// this.IsOnTheGround(); // ToDo: Also determine if he has enough energy to sprint
    }

    private bool ShouldJump()
    {
        return Keyboard.current.spaceKey.wasPressedThisFrame;
    }

    private void Jump()
    {
        if (Sliding)
            GiveJumpVelocity(1 / this.sprintModifier);
        else if (IsSprinting())
            GiveJumpVelocity(this.sprintModifier);
        else
            GiveJumpVelocity();
    }

    private void GiveJumpVelocity(float modifier = 1.0f)
    {
        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.jumpHeight * modifier);
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

    private void ChangeAnimationSpeed(float sprintModifier)
    {
        this.animator.speed = sprintModifier;
    }

    public void AddCoin()
    {
        this.coins++;

        if (this.coins >= 100)
        {
            this.AddALife();

            this.coins -= 100;
        }

        this.UpdateCoinsDisplay();
    }

    private void UpdateCoinsDisplay()
    {
        this.coinsDisplay.text = this.coins.ToString();
    }

    private void AddALife()
    {
        this.lives++;

        this.UpdateLifeDisplay();
    }

    private void Die()
    {
        this.lives--;

        this.UpdateLifeDisplay();

        this.health = this.maxHealth;
    }

    private void RestoreHealth()
    {
        if (this.health < this.maxHealth)
        {
            this.health++;

            this.heartsDisplay.UpdateHealthDisplay(this.health);
        }
        else
            this.coins += 50;
    }

    private void LoseHealth()
    {
        if (!this.IsHurting)
        {
            this.characterHead.Hurt(HURT_DURATION);
            this.Hurt(HURT_DURATION);

            this.health--;

            if (this.health < 0)
                this.Die();

            this.heartsDisplay.UpdateHealthDisplay(this.health);
        }

    }

    private void UpdateLifeDisplay()
    {
        this.livesDisplay.text = $"x{this.lives}";
    }

    public bool IsHurting
    {
        get
        {
            return this.runSpeed != this.originalRunSpeed;
        }
    }

    private void Hurt(float hurtDuration)
    {
        StartCoroutine(SlowDown(hurtDuration));
    }

    private IEnumerator SlowDown(float hurtDuration)
    {
        this.runSpeed /= 2;

        yield return new WaitForSeconds(hurtDuration);

        this.runSpeed = this.originalRunSpeed;
    }
}
