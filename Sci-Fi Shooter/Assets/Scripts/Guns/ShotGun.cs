using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotGun : AdditionalGunInformation
{
    public Vector3[] fixedPellets;
    public int extraPellets;
    public float attackSpeed;
    public float reloadTime;
    [Range(0, 100)] public float adsZoom;
    bool faToggle;
    bool keepFiring;
    public SgFireMode fireMode;

    public bool canChoke;
    bool choking;
    public int chokeDamageBoost;
    public int chokePelletReduction;
    public float chockeAccuracyBoost;

    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (player.inventory.primaryAmmo == 0 && weaponSlot == WeaponSlot.Primary)
        {
            if (!isReloading)
                StartCoroutine(Reloading());
        }
        else if (player.inventory.secondaryAmmo == 0 && weaponSlot == WeaponSlot.Secondary)
        {
            if (!isReloading)
                StartCoroutine(Reloading());
        }
        else if (fireMode == SgFireMode.Double && (callbackContext.started || callbackContext.canceled))
        {
            ShootBullet();
            if (player.inventory.primaryAmmo == 0 && weaponSlot == WeaponSlot.Primary)
            {
                if (!isReloading)
                    StartCoroutine(Reloading());
            }
            else if (player.inventory.secondaryAmmo == 0 && weaponSlot == WeaponSlot.Secondary)
            {
                if (!isReloading)
                    StartCoroutine(Reloading());
            }
        }
        else if (fireMode == SgFireMode.Single && callbackContext.started && canFire)
            StartCoroutine(SingleFire());
        if (fireMode == SgFireMode.Auto && callbackContext.started)
        {
            faToggle = true;
            keepFiring = true;
            if (canFire)
                StartCoroutine(AutoFire());
        }
        else if (fireMode == SgFireMode.Auto && callbackContext.canceled)
        {
            faToggle = false;
            keepFiring = false;
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && canChoke)
        {
            if (choking)
            {
                damage -= chokeDamageBoost;
                extraPellets += chokePelletReduction;
                accuracy -= chockeAccuracyBoost;
                choking = false;
            }
            else
            {
                damage += chokeDamageBoost;
                extraPellets -= chokePelletReduction;
                accuracy += chockeAccuracyBoost;
                choking = true;
            }
        }
    }
    public override void Reload()
    {
        if (!isReloading && player.inventory.primaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Primary)
            StartCoroutine(Reloading());
        else if (!isReloading && player.inventory.secondaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Secondary)
            StartCoroutine(Reloading());
    }

    IEnumerator SingleFire()
    {
        canFire = false;
        ShootBullet();
        yield return new WaitForSeconds(1f / attackSpeed);
        canFire = true;
        if (!isReloading && player.inventory.primaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Primary)
            StartCoroutine(Reloading());
        else if (!isReloading && player.inventory.secondaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Secondary)
            StartCoroutine(Reloading());
    }
    IEnumerator AutoFire()
    {
        canFire = false;
        while (faToggle && player.inventory.primaryAmmo > 0 && weaponSlot == WeaponSlot.Primary)
        {
            ShootBullet();
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        while (faToggle && player.inventory.secondaryAmmo > 0 && weaponSlot == WeaponSlot.Secondary)
        {
            ShootBullet();
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        canFire = true;
        if (!isReloading && player.inventory.primaryAmmo == 0 && weaponSlot == WeaponSlot.Primary)
            StartCoroutine(Reloading());
        else if (!isReloading && player.inventory.secondaryAmmo == 0 && weaponSlot == WeaponSlot.Secondary)
            StartCoroutine(Reloading());
    }
    public void ShootBullet()
    {
        float convertedAccuracy = (100 - accuracy) / 200;
        animator.SetTrigger("Shoot");
        for (int i = 0; i < fixedPellets.Length + extraPellets; i++)
        {
            Vector3 bulletDirection = new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy));

            if (i < fixedPellets.Length)
                FireBullet(fixedPellets[i]);
            else
                FireBullet(bulletDirection);
        }
        if (weaponSlot == WeaponSlot.Primary)
            player.inventory.primaryAmmo--;
        else if (weaponSlot == WeaponSlot.Secondary)
            player.inventory.secondaryAmmo--;
        player.UpdateAmmo(ammoType, weaponSlot);
        if (animator.GetBool("ADS") == true)
            GetComponentInParent<RecoilScript>().Recoil(recoilValue * ((100 - adsRecoilReduction)/100), RecoilType.Procedural);
        else
            GetComponentInParent<RecoilScript>().Recoil(recoilValue, RecoilType.Procedural);
    }
    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        if (faToggle)
            faToggle = false;
        animator.speed = 1f / reloadTime;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        if (weaponSlot == WeaponSlot.Primary)
        {
            for (int i = player.inventory.primaryAmmo; i < maxAmmo; i++)
            {
                player.inventory.shotgunAmmo--;
                player.inventory.primaryAmmo++;
            }
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            for (int i = player.inventory.secondaryAmmo; i < maxAmmo; i++)
            {
                player.inventory.shotgunAmmo--;
                player.inventory.secondaryAmmo++;
            }
        }
        player.UpdateAmmo(ammoType, weaponSlot);
        isReloading = false;
        canFire = true;
        animator.speed = 1;
        if (keepFiring)
        {
            faToggle = true;
            StartCoroutine(AutoFire());
        }
    }
    public override void SecondaryFire(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            animator.SetBool("ADS", true);
            player.cam.GetComponent<Camera>().fieldOfView = player.baseFov * (1 - (adsZoom / 100));
        }
        else if (callbackContext.canceled)
        {
            animator.SetBool("ADS", false);
            player.cam.GetComponent<Camera>().fieldOfView = player.baseFov;
        }
    }
    public override void OnUnEquip()
    {
        player.cam.GetComponent<Camera>().fieldOfView = player.baseFov;
        base.OnUnEquip();
    }
}

[System.Serializable]
public enum SgFireMode
{
    Double,
    Single,
    Auto
}
