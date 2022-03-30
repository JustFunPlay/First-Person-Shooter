using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotGun : GunBase
{
    [Range(1, 50)]
    public int pellets;
    public float attackSpeed;
    public float reloadTime;
    public Transform bulletPoint;
    public GameObject fakeHit;
    public Vector3 recoilValue;
    [Range(0f, 100f)]
    public float accuracy;
    bool faToggle;
    bool keepFiring;
    public SgFireMode fireMode;
    bool isReloading;

    public bool canChoke;
    bool choking;
    public int chokeDamageBoost;
    [Range(1, 50)]
    public int chokePelletReduction;
    public float chockeAccuracyBoost;
    [Range(0, 100)]
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
        else if (fireMode == SgFireMode.Double && (callbackContext.started || callbackContext.canceled))
        {
            ShootBullet();
            if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0 && !isReloading)
            {
                StartCoroutine(Reloading());
            }
        }
        else if (fireMode == SgFireMode.Single && callbackContext.started && canFire)
        {
            StartCoroutine(SingleFire());
        }
        if (fireMode == SgFireMode.Auto && callbackContext.started)
        {
            faToggle = true;
            keepFiring = true;
            if (canFire)
            {
                StartCoroutine(AutoFire());
            }
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
                pellets += chokePelletReduction;
                accuracy -= chockeAccuracyBoost;
                choking = false;
            }
            else
            {
                damage += chokeDamageBoost;
                pellets -= chokePelletReduction;
                accuracy += chockeAccuracyBoost;
                choking = true;
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

    IEnumerator SingleFire()
    {
        canFire = false;
        ShootBullet();
        yield return new WaitForSeconds(1f / attackSpeed);
        canFire = true;
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator AutoFire()
    {
        canFire = false;
        while (faToggle && player.inventory.weaponInventory[player.currentWeapon].currentAmmo > 0)
        {
            ShootBullet();
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
        animator.SetTrigger("Shoot");
        for (int i = 0; i < pellets; i++)
        {
            if (Physics.Raycast(bulletPoint.position, bulletPoint.forward + new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy)), out RaycastHit hit, 500f))
            {
                if (hit.collider.GetComponent<HitBox>())
                {
                    hit.collider.GetComponent<HitBox>().HitDamage(damage);
                }
                Instantiate(fakeHit, hit.point, Quaternion.identity);
            }
        }
        player.inventory.weaponInventory[player.currentWeapon].currentAmmo--;
        player.UpdateAmmo(ammoType);
        if (animator.GetBool("ADS") == true)
        {
            GetComponentInParent<RecoilScript>().Recoil(recoilValue * ((100 - adsRecoilReduction)/100), RecoilType.Procedural);
        }
        else
        {
            GetComponentInParent<RecoilScript>().Recoil(recoilValue, RecoilType.Procedural);
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
            StartCoroutine(AutoFire());
        }
    }
}

[System.Serializable]
public enum SgFireMode
{
    Double,
    Single,
    Auto
}
