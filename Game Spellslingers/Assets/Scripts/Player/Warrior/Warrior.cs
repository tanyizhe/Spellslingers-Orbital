using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    /**
     * <summary>
     * A class that represents Warrior. 
     * The warrior has a basic slam attack.
     * </summary> 
     */
    [SerializeField] private LayerMask enLayer;
    [SerializeField] private float aoe;
    [SerializeField] private Transform slamPos;
    [SerializeField] private GameObject slamGroundEffect;

    [SerializeField] private float attack;
    private float armour;
    private float regen;
    private float finalRegen;

    // skills
    private bool sanctuaryEnabled;


    private bool frenzyEnabled;
    private bool demonEnabled;
    private bool cullEnabled;
    private bool earthquakeEnabled;
    private Vector2 aftershockLocation;
    [SerializeField] private GameObject aftershockPrefab;

    // final damage
    private float finalDamage;

    // audio control
    private float timeBtwAudio;

    public event ChangeEventHandler<Warrior, EventArgs> SlamChange;
    public event ChangeEventHandler<Warrior, EventArgs> WarriorChange;

    void OnEnable()
    {
        HammerSlamEvent.slamEventInfo += ExecuteAttack;
        regen = 0;
        InvokeRepeating("Regen", 0, 3.0f);
        earthquakeEnabled = false;
        cullEnabled = false;
        demonEnabled = false;
        frenzyEnabled = false;
        sanctuaryEnabled = false;

        timeBtwAudio = 0f;
    }

    void OnDisable()
    {
        HammerSlamEvent.slamEventInfo -= ExecuteAttack;
    }
    /**
     * <summary>
     * A method that damages enemies in a circle.
     * </summary> 
     */
    void Update()
    {
        timeBtwAudio -= Time.deltaTime;
    }

    private void ExecuteAttack()
    {
        AudioManager.instance.Play("slam_sfx");

        finalDamage = this.attack;
        if (earthquakeEnabled)
        {
            finalDamage -= 20;
            // final damage cannot be < 0.
            if (finalDamage < 0)
            {
                finalDamage = 0;
            }
            aftershockLocation = new Vector2(slamPos.position.x, slamPos.position.y);
            Invoke("CreateAftershock", 2f);
        }

        Collider2D[] areaSlam = Physics2D.OverlapCircleAll(slamPos.position, aoe, enLayer);
        for (int i = 0; i < areaSlam.Length; i++)
        {
            if (cullEnabled)
            {
                if (areaSlam[i].GetComponent<Health>().CurrentHealth / areaSlam[i].GetComponent<Health>().MaxHealth < 0.1f)
                {
                    areaSlam[i].GetComponent<Health>().TakeDamage(99999);
                }
                else
                {
                    areaSlam[i].GetComponent<Health>().TakeDamage(finalDamage);
                }
            }
            else
            {
                areaSlam[i].GetComponent<Health>().TakeDamage(finalDamage);
            }
            if (demonEnabled)
            {
                health.CurrentHealth += regen * 1.5f;
                if (health.CurrentHealth > health.MaxHealth)
                {
                    health.CurrentHealth = health.MaxHealth;
                }
            }
        }
        Instantiate(slamGroundEffect, slamPos.position, Quaternion.identity);

        
    }

    public void IncreaseSlamArea(float increment)
    {
        aoe += increment;
        OnSlamChange();
    }
    public float GetSlamArea()
    {
        return aoe;
    }

    public void IncreaseArmour(float increment)
    {
        armour += increment;
    }

    public float GetArmour()
    {
        return armour;
    }

    public void IncreaseRegen(float increment)
    {
        regen += increment;
        OnWarriorChange();
    }

    public float GetRegen()
    {
        return regen;
    }

    public void SetAttack(float newAtk)
    {
        attack = newAtk;
        OnSlamChange();
    }

    public float GetAttack()
    {
        return attack;
    }
    #region Earthquake methods
    /**
     * <summary>
     * Signature Earthquake skill
     * </summary>
     */
    public void ActivateEarthquake()
    {
        earthquakeEnabled = true;
    }

    private void CreateAftershock()
    {
        Instantiate(aftershockPrefab, aftershockLocation, Quaternion.identity);
    }
    #endregion

    #region Executioner methods
    /**
     * <summary>
     * Signature Executioner skill
     * </summary>
     */
    public void ActivateCull()
    {
        cullEnabled = true;
    }
    #endregion

    #region Frenzy methods
    /**
     * <summary>
     * Signature Frenzy skill
     * </summary>
     */
    public bool IsFrenzy()
    {
        return frenzyEnabled;
    }
    public void ActivateFrenzy()
    {
        frenzyEnabled = true;
        health.CurrentHealth = health.CurrentHealth / 2;
        health.MaxHealth = health.MaxHealth / 2;
        this.attack = this.attack + 100f;
        GameObject healthUI = transform.GetChild(2).gameObject;
        healthUI.SetActive(false);
    }
    public void DeactivateFrenzy()
    {
        frenzyEnabled = false;
        //GameObject healthUI = transform.GetChild(2).gameObject;
        //healthUI.SetActive(true);
    }

    #endregion

    #region Demon methods

    public void ActivateDemon()
    {
        demonEnabled = true;
    }

    #endregion

    #region Sanctuary methods

    public void ActivateSanctuary()
    {
        sanctuaryEnabled = true;
    }

    #endregion

    #region Regeneration methods
    /**
     * <summary>
     * Passively regenerates health
     * </summary>
     */

    private void Regen()
    {
        if (frenzyEnabled || demonEnabled)
        {
            return;
        }
        // regenerates
        if (health.CurrentHealth < health.MaxHealth)
        {
            finalRegen = regen;
            if (sanctuaryEnabled)
            {
                finalRegen = health.MaxHealth * regen / 100;
            }
            health.TakeDamage(-finalRegen);
            if (health.CurrentHealth > health.MaxHealth)
            {
                health.CurrentHealth = health.MaxHealth;
            }
        }
    }
    #endregion



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(slamPos.position, aoe);
    }

    #region Defences and damage taking
    /**
     * <summary>
     * Check the warrior's defences and deal damage accordingly.
     * </summary>
     */
    public override void TakeDamage(float damage)
    {
        // aim for this formula is to make armour more effective vs smaller hits
        // and less effective against larger hits
        if (timeBtwAudio <= 0 && GetCurrentHealth() > 0)
        {
            AudioManager.instance.Play("warrior_hit");
            timeBtwAudio = 1f;
        }
        if (damage != 0)
        {
            base.TakeDamage(damage * damage / (armour + damage));
        }
    }
    #endregion

    protected virtual void OnSlamChange()
    {
        SlamChange?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnWarriorChange()
    {
        WarriorChange?.Invoke(this, EventArgs.Empty);
    }

    private void DestroyHammerOnDeath()
    {
        transform.Find("Hammer").gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return "Warrior";
    }
}