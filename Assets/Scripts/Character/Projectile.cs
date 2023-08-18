using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class animates weapon projectiles, such as arrows and rocks. It's 
/// designed to take the object from its starting point to its target, then
/// make the object disappear upon impact.
/// </summary>
public class Projectile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private int speed = 100;
    private Vector3 target;
    private bool isMoving = false;
    private Action<int> callback;

    // TODO: re-evaluate whether there's a better way to animate projectiles.
    // This is the most inefficient part of this entire battle system.
    public void FixedUpdate()
    {
        if (isMoving) 
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopMotion();
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void Move()
    {
        isMoving = true;
    }

    public void StopMotion()
    {
        isMoving = false;
        spriteRenderer.enabled = false;
        callback?.Invoke(2);
    }

    public void SetCallback(Action<int> callback)
    {
        this.callback = callback;
    }
}
