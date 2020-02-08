using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    private const string DEAD = "IsDead";

    [SerializeField] bool canBeDamagedFromAbove = true;
    [SerializeField] bool canBeDamagedFromTheSides = true;
    [SerializeField] bool canBeDamagedFromSliding = true;
    [SerializeField] bool fallOffScreenWhenDead = false;

    private Character character;
    private Animator animator;

    private void Start()
    {
        this.character = FindObjectOfType<Character>();
        this.animator = GetComponent<Animator>();
    }

    public bool IsDead
    {
        get
        {
            return this.animator.GetBool(DEAD);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character && !IsDead && ShouldCollide(collision))
        {
            HitDirection hitFrom = this.GetHitDirection(character);

            switch (hitFrom)
            {
                case HitDirection.Above:
                    if (this.canBeDamagedFromAbove)
                    {
                        this.character.BounceUp();
                        this.Die();
                    }
                    else
                        this.character.BounceUp();
                    break;
                case HitDirection.Side:
                    if (this.canBeDamagedFromTheSides)
                        this.Die();
                    else
                        this.DamagePlayer();
                    break;
                case HitDirection.SideAndSlide:
                    if (this.canBeDamagedFromSliding)
                    {
                        this.character.Kick();
                        this.Die();
                    }
                    else
                        this.DamagePlayer();
                    break;
                case HitDirection.Below:
                    this.DamagePlayer();
                    break;
            }
        }
    }

    private bool ShouldCollide(Collider2D collision)
    {
        CapsuleCollider2D capsuleCollider = this.GetComponent<CapsuleCollider2D>();

        return capsuleCollider && collision.IsTouching(capsuleCollider);
    }

    private void DamagePlayer()
    {
        this.character.LoseHealth();
    }

    private void Die()
    {
        this.animator.SetBool(DEAD, true);
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.15f);

        if (this.fallOffScreenWhenDead)
        {
            this.animator.enabled = false;

            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();

            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.SetRotation(90);

            Destroy(this.gameObject, 5f);
        }
    }

    private HitDirection GetHitDirection(Character character)
    {
        if (character.GoingDown)
            return HitDirection.Above;
        else if (character.GoingUp)
            return HitDirection.Below;
        else if (character.Sliding)
            return HitDirection.SideAndSlide;
        else
            return HitDirection.Side;
    }

    private enum HitDirection
    {
        Above,
        Side,
        SideAndSlide,
        Below
    }
}
