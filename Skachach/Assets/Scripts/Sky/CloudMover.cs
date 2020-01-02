using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [Range(1, 20)][SerializeField] float minSpeed = 5f;
    [Range(1, 20)][SerializeField] float maxSpeed = 15f;

    float speed;

    void Start()
    {
        if (this.minSpeed > this.maxSpeed)
            throw new System.ArgumentException("minSpeed cannot be larger than maxSpeed.");

        this.speed = Random.Range(this.minSpeed, this.maxSpeed);
    }

    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * this.speed / 10;

        if (transform.position.x < -21f)
            Destroy(this.gameObject);
    }
}
