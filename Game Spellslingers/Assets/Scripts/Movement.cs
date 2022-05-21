using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 movement;
    private float moveSpeed = 10;

    public Movement(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void Awake()
    {
        this.rb = gameObject.AddComponent<Rigidbody2D>();
        this.rb.gravityScale = 0;
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        this.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        this.sr = GetComponent<SpriteRenderer>();
        this.anim = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        AnimateMovement();
    }

    public virtual void FixedUpdate()
    {
        UpdatePosition();
    }

    public float GetMoveSpeed() 
    {
        return this.moveSpeed;
    }

    public void SetMoveSpeed(float movespeed)
    {
        this.moveSpeed += movespeed;
    }

    public void SetX(float f) { this.movement.x = f; }

    public void SetY(float f) { this.movement.y = f; }

    public Rigidbody2D GetRb()
    {
        return this.rb;
    }

    public void AnimateMovement()
    {
        if (this.movement.x > 0) 
        {
            sr.flipX = false;
        }
        if (this.movement.x < 0) 
        {
            sr.flipX = true;
        }
        this.anim.SetFloat("Horizontal", this.movement.x);
        this.anim.SetFloat("Vertical", this.movement.y);
        this.anim.SetFloat("Speed", this.movement.sqrMagnitude);
    }


    public virtual void UpdatePosition()
    {
        rb.MovePosition(this.rb.position + 
            this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);
    }
}