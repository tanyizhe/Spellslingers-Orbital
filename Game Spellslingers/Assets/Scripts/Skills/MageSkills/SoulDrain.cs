using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDrain : Skill
{
    public SoulDrain() : base(5) { }
    
    public override void Start()
    {
        base.Start();
        Button.onClick.AddListener(() =>
        {
            OnSelected(EventArgs.Empty);
            HealOnEnemyDeath heal = Player.instance.GetComponent<HealOnEnemyDeath>();
            heal.enabled = true;
            heal.IncreaseHeal(2f);
        });
    }

    public override void Reset() { }

    public override bool IsSignatureSkill()
    {
        return false;
    }
}
