using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private const float HURT_DURATION = 1f;
    private const float BLINK_DURATION = 0.1f;
    private const float VERTICLE_VELOCITY_TRESHHOLD = 0.25f;
    private const int COINS_PER_EXTRA_HEALTH = 50;
    private const int RESPAWN_SECONDS = 3;

    private const string RUNNING = "IsRunning";
    private const string GOING_UP = "IsGoingUp";
    private const string GOING_DOWN = "IsGoingDown";
    private const string SLIDING = "IsSliding";
    private const string DEAD = "IsDead";

    private int coins = 0;
    private int health = 3;
    private int maxHealth = 3;

    private Vector2 moveVector;
    private Vector2 spawnLocation;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    [SerializeField] int lives = 3;
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpHeight = 20f;

    [SerializeField] GameObject coinsDisplayGameObject;
    [SerializeField] GameObject livesDisplayGameObject;
    [SerializeField] GameObject coinToExplode;

    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip multipleCoinsSFX;
    [SerializeField] AudioClip bounceSFX;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip kickSFX;

    private TextMeshProUGUI coinsDisplay;
    private TextMeshProUGUI livesDisplay;

    private HeartsDisplay heartsDisplay;
    private CharacterHead characterHead;

    private float originalRunSpeed;
    private bool canDoubleJump = false;

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        this.coinsDisplay = this.coinsDisplayGameObject.GetComponent<TextMeshProUGUI>();
        this.livesDisplay = this.livesDisplayGameObject.GetComponent<TextMeshProUGUI>();

        this.heartsDisplay = FindObjectOfType<HeartsDisplay>();
        this.characterHead = FindObjectOfType<CharacterHead>();

        this.heartsDisplay.UpdateHealthDisplay(this.health);
        this.originalRunSpeed = this.runSpeed;
        this.spawnLocation = new Vector2(transform.position.x, transform.position.y);
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

    public static Color HurtColor
    {
        get
        {
            return new Color(1f, 0.6f, 0.6f);
        }
    }

    public bool Running { get { return this.animator.GetBool(RUNNING); } }
    public bool Sliding { get { return this.animator.GetBool(SLIDING); } }
    public bool GoingUp { get { return this.animator.GetBool(GOING_UP); } }
    public bool GoingDown { get { return this.animator.GetBool(GOING_DOWN); } }
    public bool Airbourne { get { return this.animator.GetBool(GOING_DOWN) || this.animator.GetBool(GOING_UP); } }
    public bool IsDead { get; set; }

    private void Move()
    {
        if (IsDead)
            return;

        if (IsSliding())
        { /* Nothing, slowly stop moving */ }
        else if (IsMoveVectorSet())
        {
            this.Run();
        }
        else
            StopMoving();

        if (IsOnTheGround())
            ResetDoubleJump();

        if (IsOnTheGround() && ShouldJump())
            Jump();
        else if (!IsOnTheGround() && ShouldDoubleJump())
            DoubleJump();
    }

    private void Animate()
    {
        ChangeAnimationSpeed(1.0f * this.runSpeed / this.originalRunSpeed);

        if (IsDead)
            SetAnimation(DEAD);
        else if (IsGoingUp())
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
            }
            else
                ResetAllAnimations();
        }
        else
            ResetAllAnimations();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DieCollider dieCollider = collision.gameObject.GetComponent<DieCollider>();

        if (dieCollider != null)
            this.Die();
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
        if (IsDead)
            return;

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

    private bool IsSliding()
    {
        return Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
    }

    private bool ShouldJump()
    {
        return Keyboard.current.spaceKey.wasPressedThisFrame;
    }

    private bool ShouldDoubleJump()
    {
        return this.ShouldJump() && this.canDoubleJump; 
    }

    private void Jump()
    {
        GiveJumpVelocity(1.0f);
    }

    private void DoubleJump()
    {
        this.canDoubleJump = false;

        GiveJumpVelocity(0.75f);
    }

    public void BounceUp()
    {
        AudioSource.PlayClipAtPoint(this.bounceSFX, transform.position);

        GiveJumpVelocity(0.75f);
    }

    public void Kick()
    {
        AudioSource.PlayClipAtPoint(this.kickSFX, transform.position);
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

    public void AddCoins(int numberOfCoins)
    {
        this.coins += numberOfCoins;
        this.ExplodeCoins(numberOfCoins);

        if (numberOfCoins > 1)
            AudioSource.PlayClipAtPoint(this.multipleCoinsSFX, transform.position);

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
        if (!IsDead)
        {
            StartCoroutine(SetDeadFlag());

            this.lives--;

            this.UpdateLifeDisplay();

            AudioSource.PlayClipAtPoint(this.deathSFX, transform.position);

            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(RESPAWN_SECONDS);

        this.DestroyAllBees();

        this.transform.position = this.spawnLocation;
        this.rigidBody.velocity = new Vector2(0, 0);

        this.health = this.maxHealth;
        this.heartsDisplay.UpdateHealthDisplay(this.health);


    }

    private IEnumerator SetDeadFlag()
    {
        this.IsDead = true;

        yield return new WaitForSeconds(RESPAWN_SECONDS);

        this.IsDead = false;
    }

    private void RestoreHealth()
    {
        if (this.health < this.maxHealth)
        {
            this.health++;

            this.heartsDisplay.UpdateHealthDisplay(this.health);
        }
        else
        {
            this.AddCoins(COINS_PER_EXTRA_HEALTH);
        }
    }

    public void LoseHealth()
    {
        if (!this.IsHurting)
        {
            AudioSource.PlayClipAtPoint(this.hurtSFX, transform.position);

            this.Hurt();

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

    private void Hurt()
    {
        this.characterHead.Hurt(HURT_DURATION);
        this.BlinkPlayer();
        StartCoroutine(SlowDown());
    }

    private IEnumerator SlowDown()
    {
        this.runSpeed /= 2;

        yield return new WaitForSeconds(HURT_DURATION);

        this.runSpeed = this.originalRunSpeed;
    }

    private void BlinkPlayer()
    {
        int numberOfBlinks = Mathf.RoundToInt(HURT_DURATION / BLINK_DURATION);

        StartCoroutine(DoBlinks(numberOfBlinks, BLINK_DURATION));
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        this.spriteRenderer.color = HurtColor;

        for (int i = 0; i < numBlinks; i++)
        {
            this.spriteRenderer.enabled = !this.spriteRenderer.enabled;
            this.animator.enabled = !this.animator.enabled;

            yield return new WaitForSeconds(seconds);
        }

        this.spriteRenderer.color = Color.white;
        this.spriteRenderer.enabled = true;
        this.animator.enabled = true;
    }

    private void ExplodeCoins(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject coin = Instantiate(
                original: this.coinToExplode,
                position: this.transform.position,
                rotation: Quaternion.identity
            );

            Destroy(coin, 3f);
        }
    }

    private void ResetDoubleJump()
    {
        this.canDoubleJump = true;
    }

    private void DestroyAllBees()
    {
        foreach (Bee bee in FindObjectsOfType<Bee>())
            Destroy(bee.gameObject);
    }
}
