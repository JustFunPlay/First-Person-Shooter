using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyMachineGun : GunBase
{
    public float attackSpeed;
    public float reloadTime;
    public Transform firePoint;
    public GameObject fakeHit;
    bool faToggle;
    bool keepFiring;
    public float maxASBonus;
    public int shotsToMaxBonus;
    public float coolDownTime;
    
    public float currentAS;
    public int shotsFired;

    public FixedSprayPattern[] sprayPattern;
    int shotIndex;
    float delayToReset;

    public Vector3 recoilValue;
    public float accuracy;
    bool isReloading;
    public float adsRecoilReduction;
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
        if (callbackContext.started)
        {
            faToggle = true;
            keepFiring = true;
            if (canFire)
            {
                StartCoroutine(Dakka());
            }
        }
        else if (callbackContext.canceled)
        {
            keepFiring = false;
            faToggle = false;
        }
    }
    IEnumerator Dakka()
    {
        canFire = false;
        if (weaponSlot == WeaponSlot.Primary)
        {
            while (faToggle && player.inventory.primaryAmmo > 0)
            {
                ShootBullet();
                player.inventory.primaryAmmo--;
                player.UpdateAmmo(ammoType, weaponSlot);
                yield return new WaitForSeconds(1f / currentAS);
            }
            canFire = true;
            if (player.inventory.primaryAmmo == 0 && !isReloading)
            {
                StartCoroutine(Reloading());
            }
        }
        else if (weaponSlot == WeaponSlot.Secondary)
        {
            while (faToggle && player.inventory.secondaryAmmo > 0)
            {
                ShootBullet();
                player.inventory.secondaryAmmo--;
                player.UpdateAmmo(ammoType, weaponSlot);
                yield return new WaitForSeconds(1f / currentAS);
            }
            canFire = true;
            if (player.inventory.secondaryAmmo == 0 && !isReloading)
            {
                StartCoroutine(Reloading());
            }
        }
        StartCoroutine(CoolingDown());
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
    public void ShootBullet()
    {
        animator.SetTrigger("Shoot");
        float convertedAccuracy = (100 - accuracy) / 200;
        if (shotIndex >= sprayPattern.Length)
        {
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
                GetComponentInParent<RecoilScript>().Recoil(recoilValue * ((100 - adsRecoilReduction) / 100), RecoilType.Procedural);
            }
            else
            {
                GetComponentInParent<RecoilScript>().Recoil(recoilValue, RecoilType.Procedural);
            }
        }
        else
        {
            if (Physics.Raycast(firePoint.position, firePoint.forward + firePoint.TransformDirection(sprayPattern[shotIndex].fixedSpray), out RaycastHit hit, 500f))
            {
                if (hit.collider.GetComponent<HitBox>())
                {
                    hit.collider.GetComponent<HitBox>().HitDamage(damage);
                }
                Instantiate(fakeHit, hit.point, Quaternion.identity);
            }
            if (animator.GetBool("ADS") == true)
            {
                GetComponentInParent<RecoilScript>().Recoil(sprayPattern[shotIndex].fixedRecoil * ((100 - adsRecoilReduction) / 100), RecoilType.Fixed);
            }
            else
            {
                GetComponentInParent<RecoilScript>().Recoil(sprayPattern[shotIndex].fixedRecoil, RecoilType.Fixed);
            }
        }
        shotIndex++;
        delayToReset = 1;
        BoostFireRate();
    }
    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        delayToReset = 0;
        if (faToggle)
        {
            faToggle = false;
        }
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
            StartCoroutine(Dakka());
        }
        
    }
    void BoostFireRate()
    {
        CheckFireRate();
        if (shotsFired < shotsToMaxBonus)
        {
            shotsFired++;
        }
    }
    IEnumerator CoolingDown()
    {
        while ((!faToggle || isReloading) && shotsFired>0)
        {
            yield return new WaitForSeconds(coolDownTime/shotsToMaxBonus);
            if (shotsFired > 0)
            {
                shotsFired--;
            }
            CheckFireRate();
        }
    }
    void CheckFireRate()
    {
        currentAS = attackSpeed * (1f + (maxASBonus / shotsToMaxBonus) * shotsFired);
    }
    private void FixedUpdate()
    {
        if (delayToReset > 0)
        {
            delayToReset -= Time.fixedDeltaTime;
        }
        else if (shotIndex > 0)
        {
            shotIndex--;
        }
    }
}
