using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreShots : Skill
{
    private Shoot shoot;
    public MoreShots() : base(10) { }
    private void Start()
    {
        this.shoot = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        Button.onClick.AddListener(() =>
        {
            OnSelected(EventArgs.Empty);
            this.shoot.AddProjectiles();
        });
    }
}