using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalGunInformation : GunBase
{
    public Transform firePoint;
    public GameObject fakeHit;
    public GameObject trail;
    public Vector3 recoilValue;
    public float accuracy;
    protected bool isReloading;
    public float adsRecoilReduction;
    public Animator animator;

    protected void FireBullet(Vector3 direction)
    {
        if (Physics.Raycast(firePoint.position, firePoint.forward + direction, out RaycastHit hit, 500f))
        {
            if (hit.collider.GetComponent<HitBox>())
                hit.collider.GetComponent<HitBox>().HitDamage(damage);
            Instantiate(fakeHit, hit.point, Quaternion.identity);
            GameObject newTrail = Instantiate(trail, firePoint.position, Quaternion.identity);
            newTrail.GetComponent<LineRenderer>().SetPosition(0, firePoint.position);
            newTrail.GetComponent<LineRenderer>().SetPosition(1, hit.point);
        }
        else
        {
            GameObject newTrail = Instantiate(trail, firePoint.position, Quaternion.identity);
            newTrail.GetComponent<LineRenderer>().SetPosition(0, firePoint.position);
            newTrail.GetComponent<LineRenderer>().SetPosition(1, firePoint.position + (firePoint.forward + direction) * 500);
        }
    }
}
