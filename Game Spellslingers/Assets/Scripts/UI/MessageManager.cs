using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    private void Start()
    {
        Item.PickUp += Print;
        Barrier.Collide += Print;
        SummoningCircle.Summon += Print;
        Invoke("EndPrint", 10f);
    }

    private void OnDestroy()
    {
        Item.PickUp -= Print;
        Barrier.Collide -= Print;
        SummoningCircle.Summon -= Print;
    }

    private void Print(Item item, EventArgs e)
    {
        CancelInvoke();
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.ToString();
        PlaySound();
        Invoke("EndPrint", 20f);
    }
    private void Print(Barrier barrier, EventArgs e)
    {
        CancelInvoke();
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = barrier.ToString();
        PlaySound();
        Invoke("EndPrint", 10f);
    }

    private void Print(SummoningCircle summon, EventArgs e)
    {
        CancelInvoke();
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = summon.ToString();
        PlaySound();
        Invoke("EndPrint", 10f);
    }

    private void PlaySound()
    {
        AudioManager.instance.Play("UI_buttonclick");
    }

    private void EndPrint()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }
}
