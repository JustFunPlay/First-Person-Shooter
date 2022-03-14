using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssaultRifleGun : GunBase
{
    public float attackSpeed;
    public float reloadTime;
    public ArFiremode firemode;
    public bool toggleFireMode;
    public Transform firePoint;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (firemode == ArFiremode.single && callbackContext.started && canFire)
        {
            StartCoroutine(SingleShot());
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {

    }
    public override void Reload()
    {
        
    }
    public override void OnEquip()
    {
        
    }
    public override void OnUnEquip()
    {
        
    }

    public IEnumerator SingleShot()
    {
        canFire = false;
        print("fire");
        yield return new WaitForSeconds(1f / attackSpeed);
        canFire = true;
    }
}
[System.Serializable]
public enum ArFiremode
{
    single,
    auto,
}
