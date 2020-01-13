using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsDisplay : MonoBehaviour
{
    private int health = 3;

    SpriteRenderer[] spriteRenderers;

    [SerializeField] Sprite filledHeart;
    [SerializeField] Sprite emptyHeart;

    private void Start()
    {
        this.spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        this.UpdateHealthDisplay();
    }

    public void TakeHealth()
    {
        if (this.health > 0)
            this.health--;

        this.UpdateHealthDisplay();
    }

    public void GainHealth()
    {
        if (this.health < 3)
            this.health++;

        this.UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            Sprite sprite = (this.health <= i) ? this.emptyHeart : this.filledHeart;

            this.spriteRenderers[i].sprite = sprite;
        }
    }
}
