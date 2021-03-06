
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * A class that represents an arrow.
 * </summary>
 */
public class Arrow : Projectile
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite frostArrow;
    [SerializeField] private Sprite greatArrow;
    [SerializeField] private GameObject explosionPrefab;
    private SpriteRenderer spriteRenderer;
    private static int damage = 10;
    private static int pierceMax = 0;
    private int pierceCount = 0;
    private static bool explosionEnabled;

    public delegate void ArrowChangeEventHandler<T, U>(T sender, U eventArgs);
    public static event ArrowChangeEventHandler<Arrow, ArrowArgs> ArrowChange;

    /**
     * <summary>
     * A float to multiply damage.
     * </summary>
     */
    private static float damageMulti = 1;
    public static float DamageMulti { get { return damageMulti; } }

    /**
     * <summary>
     * A bool to let all arrows know they
     * can lifesteal.
     * </summary>
     */
    private static bool isLifestealActive = false;

    /**
     * <summary>
     * A bool to let the arrow know they 
     * have already activated lifesteal.
     * </summary>
     */
    private bool isLifestealActivated = false;

    /**
     * <summary>
     * A bool to let all arrows know they
     * can slow.
     * </summary>
     */
    private static bool isSlowActive = false;

    /**
     * <summary>
     * A bool to let all arrows know they
     * can stun.
     * </summary>
     */
    private static bool isStunActive = false;


    public Arrow() : base(15f, 250f) { }


    private void Awake()
    {
        gameObject.GetComponent<Lifesteal>().enabled = false;
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = this.arrowSprite;
    }

    private void Start()
    {
        this.Collided += ResetPierce;
    }

    private void OnEnable()
    {
        if (isSlowActive)
        {
            this.spriteRenderer.sprite = this.frostArrow;
        }

        if (isStunActive)
        {
            this.spriteRenderer.sprite = this.greatArrow;
        }
    }

    private void OnDestroy()
    {
        this.Collided -= ResetPierce;
    }

    public static int getPierceMax()
    {
        return pierceMax;
    }

    public static void setPierceMax(int value)
    {
        pierceMax = value;
        OnArrowChange();
    }

    public static void SetDamageMulti(float mult)
    {
        Arrow.damageMulti = mult;
    }

    public static void IncreaseDamage(int damage)
    {
        Arrow.damage += damage;
        OnArrowChange();
    }

    public override int GetDamage()
    {
        return Arrow.damage;
    }

    public static int Damage { get { return Arrow.damage; } }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.Play("ArrowHit");
            if (pierceCount >= pierceMax || explosionEnabled)
            {
                pierceCount = 0;
                base.OnTriggerEnter2D(collider);
            }
            else
            {
                pierceCount += 1;
            }

            if (collider.gameObject != null)
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (explosionEnabled)
                    {
                        //explosionPrefab
                        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        enemy.TakeDamage(GetDamage() * DamageMulti);
                    }
                    if (enemy.GetCurrentHealth() > 0)
                    {
                        SlowEnemy(collider);
                        StunEnemy(collider);
                    }
                }
            }
            StartLifesteal();
        }
    }

    public static void ActivateLifeSteal()
    {
        Arrow.isLifestealActive = true;
        OnArrowChange();
    }
    public static void DeactivateLifeSteal()
    {
        Arrow.isLifestealActive = false;
    }

    private void StartLifesteal()
    {
        if (!isLifestealActivated && isLifestealActive)
        {
            isLifestealActivated = true;
            gameObject.GetComponent<Lifesteal>().enabled = true;
        }
    }
    public static void ActivateFrostArrow()
    {
        Arrow.isSlowActive = true;
    }
    public static void DeactivateFrostArrow()
    {
        Arrow.isSlowActive = false;
    }

    public static void ActivateStun()
    {
        Arrow.isStunActive = true;
    }
    public static void DeactivateStun()
    {
        Arrow.isStunActive = false;
    }


    private void SlowEnemy(Collider2D collider)
    {
        if (isSlowActive)
        {
            PlayerSlow slow = Player.instance
                .gameObject.GetComponentInChildren<PlayerSlow>(true);
            slow.gameObject.SetActive(true);
            slow.Slow(collider);
        }
    }

    private void StunEnemy(Collider2D collision)
    {
        if (isStunActive)
        {
            PlayerStun stun = Player.instance
                    .gameObject.GetComponentInChildren<PlayerStun>(true);
            stun.gameObject.SetActive(true);
            stun.Stun(collision);
        }

    }

    public static void ResetDamage()
    {
        Arrow.damage = 10;
    }

    public static void ResetPierceMax()
    {
        Arrow.pierceMax = 0;
    }

    private void ResetPierce(object sender, EventArgs e)
    {
        this.pierceCount = 0;
    }

    private static void OnArrowChange()
    {
        ArrowArgs args = new ArrowArgs();
        args.Damage = Arrow.damage;
        args.Pierce = Arrow.pierceMax;
        args.LifeSteal = Lifesteal.healAmount;
        ArrowChange?.Invoke(null, args);
    }

    public static void ActivateExplosion()
    {
        Arrow.explosionEnabled = true;
    }

    public static void DeactivateExplosion()
    {
        Arrow.explosionEnabled = false;
    }

}

public class ArrowArgs : EventArgs
{
    public int Damage { get; set; }
    public int Pierce { get; set; }

    public float LifeSteal { get; set; }
}
