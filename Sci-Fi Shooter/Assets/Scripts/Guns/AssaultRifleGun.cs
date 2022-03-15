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
    public GameObject fakeHit;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        print("check To Fire");
        if (firemode == ArFiremode.single && callbackContext.started == true && canFire)
        {
            print("go Fire");
            StartCoroutine(SingleShot());
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {

    }
    public override void Reload()
    {
        
    }
    public override void OnEquip(PlayerControll playerControll)
    {
        base.OnEquip(playerControll);
    }
    public override void OnUnEquip()
    {
        
    }

    public IEnumerator SingleShot()
    {
        canFire = false;
        print("fire");
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, 500f))
        {
            if (hit.collider.GetComponentInParent<CharacterHealth>())
            {
                hit.collider.GetComponentInParent<CharacterHealth>().OnTakeDamage(damage);
            }
            Instantiate(fakeHit, hit.point, Quaternion.identity);
        }
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
