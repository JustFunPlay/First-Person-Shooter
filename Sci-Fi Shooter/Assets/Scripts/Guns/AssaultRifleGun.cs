using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssaultRifleGun : AdditionalGunInformation
{
    public float attackSpeed;
    public float reloadTime;
    public ArFiremode firemode;
    public bool toggleFireMode;
    [Range(0, 100)]public float adsZoom;
    bool faToggle;
    bool keepFiring;

    public FixedSprayPattern[] sprayPattern;
    int shotIndex;
    float delayToReset;

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
        if (!isReloading && player.inventory.primaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Primary)
        {
            StartCoroutine(Reloading());
        }
        else if (!isReloading && player.inventory.secondaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Secondary)
        {
            StartCoroutine(Reloading());
        }
    }
    public override void OnEquip(PlayerControll playerControll)
    {
        base.OnEquip(playerControll);
        
    }
    public IEnumerator SingleShot()
    {
        canFire = false;
        print("fire");
        ShootBullet();
        if (weaponSlot == WeaponSlot.Primary)
        {
            player.inventory.primaryAmmo--;
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            player.inventory.secondaryAmmo--;
        }
        player.UpdateAmmo(ammoType, weaponSlot);
        yield return new WaitForSeconds(1f / attackSpeed);
        canFire = true;
        if (player.inventory.primaryAmmo == 0 && !isReloading && weaponSlot == WeaponSlot.Primary)
        {
            StartCoroutine(Reloading());
        }
        else if (player.inventory.secondaryAmmo == 0 && !isReloading && weaponSlot == WeaponSlot.Secondary)
        {
            StartCoroutine(Reloading());
        }
    }
    public IEnumerator FullAutoShot()
    {
        canFire = false;
        if (weaponSlot == WeaponSlot.Primary)
        {
            while (faToggle && player.inventory.primaryAmmo > 0)
            {
                ShootBullet();
                player.inventory.primaryAmmo--;
                player.UpdateAmmo(ammoType, weaponSlot);
                yield return new WaitForSeconds(1f / attackSpeed);
            }
            canFire = true;
            if (player.inventory.primaryAmmo == 0 && !isReloading)
                StartCoroutine(Reloading());
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            while (faToggle && player.inventory.secondaryAmmo > 0)
            {
                ShootBullet();
                player.inventory.secondaryAmmo--;
                player.UpdateAmmo(ammoType, weaponSlot);
                yield return new WaitForSeconds(1f / attackSpeed);
            }
            canFire = true;
            if (player.inventory.secondaryAmmo == 0 && !isReloading)
                StartCoroutine(Reloading());
        }
    }
    public void ShootBullet()
    {
        animator.SetTrigger("Shoot");
        float convertedAccuracy = (100 - accuracy) / 200;
        if (shotIndex >= sprayPattern.Length)
        {
            Vector3 bulletDirection = new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy));
            FireBullet(bulletDirection);
            if (animator.GetBool("ADS") == true)
                GetComponentInParent<RecoilScript>().Recoil(recoilValue * ((100 - adsRecoilReduction) / 100), RecoilType.Procedural);
            else
                GetComponentInParent<RecoilScript>().Recoil(recoilValue, RecoilType.Procedural);
        }
        else
        {
            FireBullet(firePoint.TransformDirection(sprayPattern[shotIndex].fixedSpray));
            if (animator.GetBool("ADS") == true)
                GetComponentInParent<RecoilScript>().Recoil(sprayPattern[shotIndex].fixedRecoil * ((100 - adsRecoilReduction) / 100), RecoilType.Fixed);
            else
                GetComponentInParent<RecoilScript>().Recoil(sprayPattern[shotIndex].fixedRecoil, RecoilType.Fixed);
        }
        shotIndex++;
        delayToReset = 1;
    }

    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        delayToReset = 0;
        if (faToggle)
            faToggle = false;
        animator.speed = 1f / reloadTime;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        if (weaponSlot == WeaponSlot.Primary)
        {
            for (int i = player.inventory.primaryAmmo; i < maxAmmo; i++)
            {
                if (ammoType == AmmoType.Heavy && player.inventory.heavyAmmo > 0)
                {
                    player.inventory.heavyAmmo--;
                    player.inventory.primaryAmmo++;
                }
                else if (ammoType == AmmoType.Light && player.inventory.lightAmmo > 0)
                {
                    player.inventory.lightAmmo--;
                    player.inventory.primaryAmmo++;
                }
                else if (ammoType == AmmoType.Medium && player.inventory.mediumAmmo > 0)
                {
                    player.inventory.mediumAmmo--;
                    player.inventory.primaryAmmo++;
                }
            }
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            for (int i = player.inventory.secondaryAmmo; i < maxAmmo; i++)
            {
                if (ammoType == AmmoType.Heavy && player.inventory.heavyAmmo > 0)
                {
                    player.inventory.heavyAmmo--;
                    player.inventory.secondaryAmmo++;
                }
                else if (ammoType == AmmoType.Light && player.inventory.lightAmmo > 0)
                {
                    player.inventory.lightAmmo--;
                    player.inventory.secondaryAmmo++;
                }
                else if (ammoType == AmmoType.Medium && player.inventory.mediumAmmo > 0)
                {
                    player.inventory.mediumAmmo--;
                    player.inventory.secondaryAmmo++;
                }
            }
        }
        player.UpdateAmmo(ammoType, weaponSlot);
        isReloading = false;
        canFire = true;
        animator.speed = 1;
        if (keepFiring)
        {
            faToggle = true;
            StartCoroutine(FullAutoShot());
        }
    }
    private void FixedUpdate()
    {
        if (delayToReset > 0)
            delayToReset -= Time.fixedDeltaTime;
        else if (shotIndex > 0)
            shotIndex--;
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
public enum ArFiremode
{
    single,
    auto,
}
