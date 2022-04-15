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
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0)
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
        if (!isReloading && player.inventory.weaponInventory[player.currentWeapon].currentAmmo < maxAmmo)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        for (int i = player.inventory.weaponInventory[player.currentWeapon].currentAmmo; i < maxAmmo; i++)
        {

        }
        player.UpdateAmmo(ammoType);
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
        player.inventory.weaponInventory[player.currentWeapon].currentAmmo--;
        player.UpdateAmmo(ammoType);
        GetComponentInParent<RecoilScript>().Recoil(recoilValue, RecoilType.Procedural);
        yield return new WaitForSeconds(attackDelay);
        canFire = true;
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reloading());
        }
    }
}
