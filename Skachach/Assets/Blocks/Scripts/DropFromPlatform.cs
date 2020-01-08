using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropFromPlatform : MonoBehaviour
{
    private const float SECONDS_TO_DISABLE_COLLIDER_FOR = 0.75f;

    BoxCollider2D boxCollider;

    bool boxColliderDisabled = false;

    private void Start()
    {
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (ShouldDrop())
            Drop();
    }

    private bool ShouldDrop()
    {
        return this.boxCollider.enabled && (
            Keyboard.current.sKey.wasPressedThisFrame ||
            Keyboard.current.downArrowKey.wasPressedThisFrame);
    }

    private void Drop()
    {
        this.boxCollider.enabled = false;

        this.StartCoroutine(ExecuteAfterTime(SECONDS_TO_DISABLE_COLLIDER_FOR));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        this.boxCollider.enabled = true;
    }
}
