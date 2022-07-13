using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApexForm : Skill
{
    public ApexForm() : base(1) { }

    public override void Start()
    {
        base.Start();
        Button.onClick.AddListener(() =>
        {
            OnSelected(EventArgs.Empty);
            Mage mage = (Mage)Player.instance;
            mage.ActivateApexForm();
        });
    }

    public override void Reset() { }

    public override bool IsSignatureSkill()
    {
        return true;
    }
}