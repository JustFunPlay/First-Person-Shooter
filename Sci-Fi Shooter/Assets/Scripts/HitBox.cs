using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public HitBoxType type;

    public void HitDamage(int damageToDo)
    {
        if (type == HitBoxType.Reduced)
        {
            damageToDo -= (int)(damageToDo * 0.3f);
        }
        else if (type == HitBoxType.WeakPoint)
        {
            damageToDo += damageToDo;
        }
        GetComponentInParent<CharacterHealth>().OnTakeDamage(damageToDo);
    }

}

public enum HitBoxType
{
    Normal,
    WeakPoint,
    Reduced
}