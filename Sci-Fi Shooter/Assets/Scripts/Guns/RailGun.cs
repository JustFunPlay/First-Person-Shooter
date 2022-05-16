using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RailGun : GunBase
{
    public float attackDelay;
    public float reloadTime;
    public Transform bulletPoint;
    public GameObject projectileTrail;
    public Vector3 recoilValue;
    bool isReloading;
    public Animator animator;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (player.inventory.primaryAmmo == 0 && weaponSlot == WeaponSlot.Primary)
        {
            if (!isReloading)
            {
                StartCoroutine(Reloading());
            }
        }
        else if (player.inventory.secondaryAmmo == 0 && weaponSlot == WeaponSlot.Secondary)
        {
            if (!isReloading)
            {
                StartCoroutine(Reloading());
            }
        }
        else if (callbackContext.started && canFire)
        {
            StartCoroutine(Boom());
        }
    }

    public override void Reload()
    {
        if (!isReloading && player.inventory.primaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Primary)
        {
            StartCoroutine(Reloading());
        }
        else if (!isReloading && player.inventory.secondaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Secondary)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        if (weaponSlot == WeaponSlot.Primary)
        {
            for (int i = player.inventory.primaryAmmo; i < maxAmmo; i++)
            {
                player.inventory.railAmmo--;
                player.inventory.primaryAmmo++;
            }
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            for (int i = player.inventory.secondaryAmmo; i < maxAmmo; i++)
            {
                player.inventory.railAmmo--;
                player.inventory.secondaryAmmo++;
            }
        }
        player.UpdateAmmo(ammoType, weaponSlot);
        isReloading = false;
        canFire = true;
    }
    IEnumerator Boom()
    {
        canFire = false;
        Collider[] colliders = Physics.OverlapCapsule(bulletPoint.position, bulletPoint.position + bulletPoint.forward * 500, 0.05f);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<HitBox>())
            {
                collider.GetComponent<HitBox>().HitDamage(damage);
            }
        }
        Instantiate(projectileTrail, bulletPoint.position, bulletPoint.rotation);
        if (weaponSlot == WeaponSlot.Primary)
        {
            player.inventory.primaryAmmo--;
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            player.inventory.secondaryAmmo--;
        }
        player.UpdateAmmo(ammoType, weaponSlot);
        GetComponentInParent<RecoilScript>().Recoil(recoilValue, RecoilType.Procedural);
        yield return new WaitForSeconds(attackDelay);
        canFire = true;
        if (player.inventory.primaryAmmo == 0 && !isReloading && weaponSlot == WeaponSlot.Primary)
        {
            StartCoroutine(Reloading());
        }
        if (player.inventory.secondaryAmmo == 0 && !isReloading && weaponSlot == WeaponSlot.Secondary)
        {
            StartCoroutine(Reloading());
        }
    }
}
