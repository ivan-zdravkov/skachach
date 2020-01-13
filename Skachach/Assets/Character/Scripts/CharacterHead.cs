using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHead : MonoBehaviour
{
    private const float HURT_DURATION = 2;
    private const float SHAKE_PERIOD = 0.05f;
    private const float SHAKE_AMOUNT = 0.02f;

    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprite sprite;
    [SerializeField] Sprite hurtSprite;

    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
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

    public bool IsHurting
    {
        get
        {
            return this.spriteRenderer.color == Color.red;
        }
    }

    private IEnumerator HurtSprite()
    {
        this.spriteRenderer.sprite = this.hurtSprite;

        yield return new WaitForSeconds(HURT_DURATION);

        this.spriteRenderer.sprite = this.sprite;
    }

    private void StartShaking()
    {
        transform.position = this.transform.position + UnityEngine.Random.insideUnitSphere * SHAKE_AMOUNT;
    }

    private void StopShaking()
    {
        CancelInvoke("StartShaking");

        transform.position = new Vector3(
            x: Mathf.Round(transform.position.x * 2f) * 0.5f,
            y: Mathf.Round(transform.position.y * 2f) * 0.5f,
            z: transform.position.z
        );
    }

    private IEnumerator TurnRed()
    {
        this.spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(HURT_DURATION);

        this.spriteRenderer.color = Color.white;
    }
}
