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
    bool keepFiring;

    public Vector3 recoilValue;
    public float accuracy;
    bool isReloading;
    public float adsRecoilReduction;
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
        else if (firemode == ArFiremode.single && callbackContext.started && canFire)
        {
            StartCoroutine(SingleShot());
        }
        if (firemode == ArFiremode.auto && callbackContext.started)
        {
            faToggle = true;
            keepFiring = true;
            if (canFire)
            {
                StartCoroutine(FullAutoShot());
            }
        }
        else if (firemode == ArFiremode.auto && callbackContext.canceled)
        {
            keepFiring = false;
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
        if (!isReloading && player.inventory.weaponInventory[player.currentWeapon].currentAmmo < maxAmmo)
        {
            StartCoroutine(Reloading());
        }
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
        player.inventory.weaponInventory[player.currentWeapon].currentAmmo--;
        player.UpdateAmmo(ammoType);
        yield return new WaitForSeconds(1f / attackSpeed);
        canFire = true;
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reloading());
        }
    }
    public IEnumerator FullAutoShot()
    {
        canFire = false;
        while (faToggle && player.inventory.weaponInventory[player.currentWeapon].currentAmmo > 0)
        {
            ShootBullet();
            player.inventory.weaponInventory[player.currentWeapon].currentAmmo--;
            player.UpdateAmmo(ammoType);
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        canFire = true;
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reloading());
        }
    }
    public void ShootBullet()
    {
        float convertedAccuracy = (100 - accuracy) / 75;
        if (Physics.Raycast(firePoint.position, firePoint.forward + new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy)), out RaycastHit hit, 500f))
        {
            if (hit.collider.GetComponent<HitBox>())
            {
                hit.collider.GetComponent<HitBox>().HitDamage(damage);
            }
            Instantiate(fakeHit, hit.point, Quaternion.identity);
        }
        if (animator.GetBool("ADS") == true)
        {
            GetComponentInParent<RecoilScript>().Recoil(recoilValue * ((100 - adsRecoilReduction) / 100));
        }
        else
        {
            GetComponentInParent<RecoilScript>().Recoil(recoilValue);
        }
    }

    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        if (faToggle)
        {
            faToggle = false;
        }
        animator.speed = 1f / reloadTime;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        for (int i = player.inventory.weaponInventory[player.currentWeapon].currentAmmo; i < maxAmmo; i++)
        {
            if (ammoType == AmmoType.Heavy && player.inventory.heavyAmmo > 0)
            {
                player.inventory.heavyAmmo--;
                player.inventory.weaponInventory[player.currentWeapon].currentAmmo++;
            }
            else if (ammoType == AmmoType.Light && player.inventory.lightAmmo > 0)
            {
                player.inventory.lightAmmo--;
                player.inventory.weaponInventory[player.currentWeapon].currentAmmo++;
            }
            else if (ammoType == AmmoType.Medium && player.inventory.mediumAmmo > 0)
            {
                player.inventory.mediumAmmo--;
                player.inventory.weaponInventory[player.currentWeapon].currentAmmo++;
            }
            else if (ammoType == AmmoType.Shotgun && player.inventory.shotgunAmmo > 0)
            {
                player.inventory.shotgunAmmo--;
                player.inventory.weaponInventory[player.currentWeapon].currentAmmo++;
            }
        }
        player.UpdateAmmo(ammoType);
        isReloading = false;
        canFire = true;
        animator.speed = 1;
        if (keepFiring)
        {
            faToggle = true;
            StartCoroutine(FullAutoShot());
        }
    }
}
[System.Serializable]
public enum ArFiremode
{
    single,
    auto,
}
