using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private Transform firePoint;
    private float speed;
    private int maxRange = 20;
    public event EventHandler Collided;

    public Projectile(float speed)
    {
        this.speed = speed; 
    }

    private void Update()
    {
        AtMaxRange();
    }

    public float Speed { get { return speed; } }

    public Transform FirePoint { set { this.firePoint = value; } }

    private void AtMaxRange()
    {
        if (this.firePoint != null && gameObject.activeSelf && 
                (Math.Abs(gameObject.transform.position.x - this.firePoint.position.x) > this.maxRange ||
                 Math.Abs(gameObject.transform.position.y - this.firePoint.position.y) > this.maxRange))
        {
            OnCollided(EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollided(EventArgs.Empty);
    }

    protected virtual void OnCollided(EventArgs e)
    {
        Collided?.Invoke(this, e);
    }
}
