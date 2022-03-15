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
    public Vector3 recoilValue;
    public float accuracy;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (canFire && callbackContext.started)
        {
            StartCoroutine(GoBurst());
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {
        
    }
    public override void Reload()
    {
        
    }
    IEnumerator GoBurst()
    {
        canFire = false;
        for (int i = 0; i < burstCount; i++)
        {
            ShootBullet();
            yield return new WaitForSeconds(burstDuration / burstCount);
        }
        yield return new WaitForSeconds(burstLockout);
        canFire = true;
    }
    public void ShootBullet()
    {
        float convertedAccuracy = 2.5f * (100 - accuracy) / 100;
        if (Physics.Raycast(bulletPoint.position, bulletPoint.forward + new Vector3(Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy), Random.Range(-convertedAccuracy, convertedAccuracy)), out RaycastHit hit, 500f))
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
