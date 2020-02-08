using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 moveVector;
    private float flySpeed = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Fly();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        CoinAnimation coin = collision.gameObject.GetComponent<CoinAnimation>();

        if (character)
            character.LoseHealth();

        if (!coin)
            Destroy(this.gameObject);
    }

    public void FlyLeft()
    {
        this.moveVector = Vector2.left;
    }

    public void FlyRight()
    {
        this.moveVector = Vector2.right;
    }

    private void Fly()
    {
        this.rigidBody.velocity = new Vector2(this.moveVector.x * this.flySpeed, this.rigidBody.velocity.y);
    }
}