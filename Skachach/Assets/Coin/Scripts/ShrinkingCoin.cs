using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingCoin : MonoBehaviour
{
    [Range(10f, 100f)] [SerializeField] float minSpeed = 30f;
    [Range(10f, 100f)] [SerializeField] float maxSpeed = 70f;

    [Range(5f, 50f)] [SerializeField] float minShrink = 50;
    [Range(5f, 50f)] [SerializeField] float maxShrink = 50f;

    private const float speedScale = 0.2f;
    private const float shrinkScale = 0.015f;

    private float speed;
    private float shrink;
    private Vector3 direction;

    private Vector3 destroyVector = new Vector3(0.01f, 0.01f, 0.01f);

    void Start()
    {
        this.speed = UnityEngine.Random.Range(this.minSpeed, this.maxSpeed);
        this.shrink = UnityEngine.Random.Range(this.minShrink, this.maxShrink);
        this.direction = UnityEngine.Random.insideUnitCircle.normalized;

        this.direction.y = Mathf.Abs(this.direction.y);
        this.direction.x = this.direction.x * 1.5f;

        this.FlyAway();
    }

    void Update()
    {
        this.Shrink();

        if (this.ShouldDestroy())
            Destroy(this.gameObject);
    }

    private bool ShouldDestroy()
    {
        return Math.Abs(this.transform.localScale.x) < this.destroyVector.x ||
            Math.Abs(this.transform.localScale.y) < this.destroyVector.y;
    }

    private void FlyAway()
    {
        this.GetComponent<Rigidbody2D>().velocity = this.direction * this.speed * speedScale;
    }

    private void Shrink()
    {
        this.transform.localScale -= new Vector3(1f, 1f, 1f) * this.shrink * shrinkScale * Time.deltaTime;
    }
}