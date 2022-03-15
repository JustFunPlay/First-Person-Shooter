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
    bool faToggle;

    public Vector3 recoilValue;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (firemode == ArFiremode.single && callbackContext.started && canFire)
        {
            StartCoroutine(SingleShot());
        }
        else if (firemode == ArFiremode.auto && callbackContext.started && canFire)
        {
            faToggle = true;
            StartCoroutine(FullAutoShot());
        }
        else if (firemode == ArFiremode.auto && callbackContext.canceled)
        {
            faToggle = false;
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {
        if (toggleFireMode && callbackContext.started)
        {
            if (firemode == ArFiremode.single)
            {
                firemode = ArFiremode.auto;
            }
            else
            {
                firemode = ArFiremode.single;
            }
        }
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
        ShootBullet();
        yield return new WaitForSeconds(1f / attackSpeed);
        canFire = true;
    }
    public IEnumerator FullAutoShot()
    {
        canFire = false;
        while (faToggle)
        {
            ShootBullet();
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        canFire = true;
    }
    public void ShootBullet()
    {
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, 500f))
        {
            if (hit.collider.GetComponentInParent<CharacterHealth>())
            {
                hit.collider.GetComponentInParent<CharacterHealth>().OnTakeDamage(damage);
            }
            Instantiate(fakeHit, hit.point, Quaternion.identity);
        }
        GetComponentInParent<RecoilScript>().Recoil(recoilValue);
    }
}
[System.Serializable]
public enum ArFiremode
{
    single,
    auto,
}
