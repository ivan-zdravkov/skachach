using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] float floatHeight = 0.25f;
    [Range(0f, 10f)][SerializeField] float floatSpeed = 1f;
    [SerializeField] AudioClip collectSFX;

    private float topVerticalThreshhold;
    private float bottomVerticalThreshhold;

    private Vector3 verticalMoveVector;

    void Start()
    {
        this.DetermineVerticalThreshholds();
        this.OffsetTurnAnimationFrame();
        this.SetVerticalMoveVector();
        this.OffsetVerticalPosition();
    }

    void Update()
    {
        this.MoveVertically();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetCollected(collision);
    }

    private void GetCollected(Collider2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character)
        {
            character.AddCoins(1);

            AudioSource.PlayClipAtPoint(this.collectSFX, this.transform.position);

            Destroy(this.gameObject);
        }
    }

    private void OffsetVerticalPosition()
    {
        for (int i = 0; i < transform.position.x * 5; i++)
            this.MoveVertically();
    }

    private void DetermineVerticalThreshholds()
    {
        this.topVerticalThreshhold = this.transform.position.y + this.floatHeight;
        this.bottomVerticalThreshhold = this.transform.position.y - this.floatHeight;
    }

    private void OffsetTurnAnimationFrame()
    {
        this.GetComponent<Animator>().SetFloat("cycleOffset", (Mathf.Floor(this.transform.position.x) % 10) / 10);
    }

    private void SetVerticalMoveVector()
    {
        this.verticalMoveVector = new Vector3(0, 1, transform.position.z);
    }

    private void MoveVertically()
    {
        Vector3 moveVector = this.verticalMoveVector * this.floatSpeed * Time.deltaTime;

        bool wouldRaiseOver = moveVector.y + this.transform.position.y > this.topVerticalThreshhold;
        bool wouldFallBellow = moveVector.y + this.transform.position.y < this.bottomVerticalThreshhold;

        if (wouldRaiseOver || wouldFallBellow)
            this.ReverseThePolarityOfTheNeutralFlow();
        else 
            transform.position = transform.position + moveVector;
    }

    private void ReverseThePolarityOfTheNeutralFlow()
    {
        this.verticalMoveVector.y *= -1;
    }
}