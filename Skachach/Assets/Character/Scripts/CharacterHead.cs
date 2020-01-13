using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHead : MonoBehaviour
{
    private const float HURT_DURATION = 5f;
    private const float SHAKE_PERIOD = 0.05f;
    private const float SHAKE_AMOUNT = 0.02f;

    private SpriteRenderer spriteRenderer;

    private Vector3 originalPosition;

    [SerializeField] Sprite sprite;
    [SerializeField] Sprite hurtSprite;

    void Start()
    {
        this.originalPosition = this.transform.position;

        this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        this.Hurt();
    }

    void Update()
    {
        
    }

    public void Hurt()
    {
        StartCoroutine(this.TurnRed());
        StartCoroutine(this.HurtSprite());

        this.InvokeRepeating("StartShaking", 0, SHAKE_PERIOD);
        this.Invoke("StopShaking", HURT_DURATION);
    }

    private IEnumerator HurtSprite()
    {
        this.spriteRenderer.sprite = this.hurtSprite;

        yield return new WaitForSeconds(HURT_DURATION);

        this.spriteRenderer.sprite = this.sprite;
    }

    private void StartShaking()
    {
        transform.position = this.originalPosition + UnityEngine.Random.insideUnitSphere * SHAKE_AMOUNT;
    }

    private void StopShaking()
    {
        CancelInvoke("StartShaking");

        transform.position = this.originalPosition;
    }

    private IEnumerator TurnRed()
    {
        this.spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(HURT_DURATION);

        this.spriteRenderer.color = Color.white;
    }
}
