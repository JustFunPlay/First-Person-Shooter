using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public void OnTakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <=0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
