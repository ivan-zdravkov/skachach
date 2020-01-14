using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomOnTouch : MonoBehaviour
{
    private const string BLOOM = "HasBloomed";
    private const int COINS_TO_ADD = 10;

    Animator animator;

    private bool containsCoins = true;

    private void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character != null && this.containsCoins)
        {
            this.animator.SetBool(BLOOM, true);
            character.AddCoins(COINS_TO_ADD);
            this.containsCoins = false;
        }
    }
}
