using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public  virtual void OnTakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <=0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
