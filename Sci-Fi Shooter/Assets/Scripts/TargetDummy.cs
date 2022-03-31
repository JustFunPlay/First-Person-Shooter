using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDummy : CharacterHealth
{
    public Text totalDamageText;
    public Text lastHitDamageText;
    float resetDelay;

    public override void OnTakeDamage(int damage)
    {
        base.OnTakeDamage(damage);
        lastHitDamageText.text = "(" + damage.ToString() + ")";
        totalDamageText.text = (maxHP - currentHP).ToString();
        resetDelay = 1.5f;
    }
    public override void OnDeath()
    {
        
    }
    private void FixedUpdate()
    {
        if (resetDelay > 0)
        {
            resetDelay -= Time.fixedDeltaTime;
        }
        else if (currentHP != maxHP)
        {
            currentHP = maxHP;
        }
    }
}
