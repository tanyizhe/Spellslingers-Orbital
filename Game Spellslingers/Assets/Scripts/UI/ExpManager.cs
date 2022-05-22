using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public static event EventHandler LevelUp;
    public event EventHandler GainMaxExp;
    public event EventHandler GainExp;

    private int maxExp = 1;
    private int exp = 0;

    private void Start()
    {
        ExpManager.LevelUp += IncreaseMaxExp;
        Enemy.DropExp += AddExp;
    }

    private void Update()
    {
        IsLevelUp();
    }

    private void OnDisable()
    {
        ExpManager.LevelUp -= IncreaseMaxExp;
        Enemy.DropExp -= AddExp;
    }

    private void IsLevelUp()
    {
        if (this.exp >= this.maxExp)
        {
            OnLevelUp(EventArgs.Empty);
        }
    }

    public int MaxExp { get { return this.maxExp; } }
    public int Exp { get { return this.exp; } }

    protected virtual void OnLevelUp(EventArgs e)
    {
        LevelUp?.Invoke(this, e);
    }

    protected virtual void OnGainMaxExp(EventArgs e)
    {
        GainMaxExp?.Invoke(this, e);
    }

    protected virtual void OnGainExp(EventArgs e)
    {
        GainExp?.Invoke(this, e);
    }

    public void IncreaseMaxExp(object sender, EventArgs e)
    {
        this.exp -= this.maxExp;
        this.maxExp += 1;
        OnGainMaxExp(EventArgs.Empty);
    }

    public void AddExp(object sender, DropExpEventArgs e)
    {
        this.exp += e.Exp;
        OnGainExp(EventArgs.Empty); 
    }
}
