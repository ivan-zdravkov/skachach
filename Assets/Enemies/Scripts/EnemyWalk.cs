using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    [Range(1f, 20f)][SerializeField] float walkSpeed = 5f;
    [SerializeField] bool shouldTurn = true;

    private Vector2 moveVector;
    private Rigidbody2D rigidBody;
    private EnemyDamageHandler enemyDamageHandler;

    void Start()
    {
        this.moveVector = Vector2.left;
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.enemyDamageHandler = this.gameObject.GetComponent<EnemyDamageHandler>();
    }

    void Update()
    {
        if (!enemyDamageHandler.IsDead)
            Move();
        else
            StopMoving();
    }

    public float Speed
    {
        get
        {
            return this.walkSpeed;
        }
        set
        {
            this.walkSpeed = value;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyDirectionTurner turner = collision.gameObject.GetComponent<EnemyDirectionTurner>();

        if (this.shouldTurn && turner && collision is CircleCollider2D)
            this.Turn();
     }

    private void Turn()
    {
        this.moveVector.x *= -1;
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }

    private void Move()
    {
        this.rigidBody.velocity = new Vector2(this.moveVector.x * this.walkSpeed, this.rigidBody.velocity.y);
    }

    private void StopMoving()
    {
        this.rigidBody.velocity = Vector2.zero;
    }
}
