using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bee bee = collision.gameObject.GetComponent<Bee>();

        if (bee)
            Destroy(collision.gameObject);
    }
}
