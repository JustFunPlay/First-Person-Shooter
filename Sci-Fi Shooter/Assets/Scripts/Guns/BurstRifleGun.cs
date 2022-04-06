using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BurstRifleGun : GunBase
{
    public int burstCount;
    public float burstDuration;
    public float burstLockout;
    public Transform bulletPoint;
    public float reloadTime;
    public GameObject fakeHit;

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
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0)
        {
            if (!isReloading)
            {
                StartCoroutine(Reloading());
            }
        }
        else if (canFire && callbackContext.started)
        {
            StartCoroutine(GoBurst());
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {
        
    }
    public override void Reload()
    {
        if (!isReloading && player.inventory.weaponInventory[player.currentWeapon].currentAmmo < maxAmmo)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator GoBurst()
    {
        canFire = false;
        for (int i = 0; i < burstCount; i++)
        {
            ShootBullet();
            player.inventory.weaponInventory[player.currentWeapon].currentAmmo--;
            player.UpdateAmmo(ammoType);
            yield return new WaitForSeconds(burstDuration / burstCount);
        }
        yield return new WaitForSeconds(burstLockout);
        canFire = true;
        if (player.inventory.weaponInventory[player.currentWeapon].currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reloading());
        }
    }
    public void ShootBullet()
    {
        float convertedAccuracy = (100 - accuracy) / 200;
        if (shotIndex >= sprayPattern.Length)
        {
            if (Physics.Raycast(bulletPoint.position, bulletPoint.forward + new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy)), out RaycastHit hit, 500f))
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
            if (Physics.Raycast(bulletPoint.position, bulletPoint.forward + bulletPoint.TransformDirection(sprayPattern[shotIndex].fixedSpray), out RaycastHit hit, 500f))
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
    }
    IEnumerator Reloading()
    {
        canFire = false;
        isReloading = true;
        delayToReset = 0;
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
