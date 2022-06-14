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
    public GameObject trail;
    [Range(0, 100)] public float adsZoom;

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
        if (!isReloading && player.inventory.primaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Primary)
        {
            StartCoroutine(Reloading());
        }
        else if (!isReloading && player.inventory.secondaryAmmo < maxAmmo && weaponSlot == WeaponSlot.Secondary)
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
            if (weaponSlot == WeaponSlot.Primary)
            {
                player.inventory.primaryAmmo--;
            }
            else if (weaponSlot == WeaponSlot.Secondary)
            {
                player.inventory.secondaryAmmo--;
            }
            player.UpdateAmmo(ammoType, weaponSlot);
            yield return new WaitForSeconds(burstDuration / burstCount);
        }
        yield return new WaitForSeconds(burstLockout);
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
    public void ShootBullet()
    {
        float convertedAccuracy = (100 - accuracy) / 200;
        if (shotIndex >= sprayPattern.Length)
        {
            Vector3 bulletDirection = new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy));
            if (Physics.Raycast(bulletPoint.position, bulletPoint.forward + bulletDirection, out RaycastHit hit, 500f))
            {
                if (hit.collider.GetComponent<HitBox>())
                {
                    hit.collider.GetComponent<HitBox>().HitDamage(damage);
                }
                Instantiate(fakeHit, hit.point, Quaternion.identity);
                GameObject newTrail = Instantiate(trail, bulletPoint.position, Quaternion.identity);
                newTrail.GetComponent<LineRenderer>().SetPosition(0, bulletPoint.position);
                newTrail.GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }
            else
            {
                GameObject newTrail = Instantiate(trail, bulletPoint.position, Quaternion.identity);
                newTrail.GetComponent<LineRenderer>().SetPosition(0, bulletPoint.position);
                newTrail.GetComponent<LineRenderer>().SetPosition(1, bulletPoint.position + (bulletPoint.forward + bulletDirection) * 500);
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
                GameObject newTrail = Instantiate(trail, bulletPoint.position, Quaternion.identity);
                newTrail.GetComponent<LineRenderer>().SetPosition(0, bulletPoint.position);
                newTrail.GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }
            else
            {
                GameObject newTrail = Instantiate(trail, bulletPoint.position, Quaternion.identity);
                newTrail.GetComponent<LineRenderer>().SetPosition(0, bulletPoint.position);
                newTrail.GetComponent<LineRenderer>().SetPosition(1, bulletPoint.position + (bulletPoint.forward + sprayPattern[shotIndex].fixedSpray) * 500);
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
