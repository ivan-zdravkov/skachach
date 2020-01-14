using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsDisplay : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;

    [SerializeField] Sprite filledHeart;
    [SerializeField] Sprite emptyHeart;

    private void Start()
    {
        this.spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        this.UpdateHealthDisplay(3);
    }

    public void UpdateHealthDisplay(int health)
    {
        if (this.spriteRenderers != null && this.spriteRenderers.Length >= 2)
        {
            for (int i = 0; i < 3; i++)
            {
                Sprite sprite = (health <= i) ? this.emptyHeart : this.filledHeart;

                this.spriteRenderers[i].sprite = sprite;
            }
        }
    }
}
