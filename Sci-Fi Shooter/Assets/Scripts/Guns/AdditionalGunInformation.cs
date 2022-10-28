using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalGunInformation : GunBase
{
    public Transform firePoint;
    public GameObject fakeHit;
    public LineRenderer bulletTrail;
    public Vector3 recoilValue;
    public float accuracy;
    protected bool isReloading;
    public float adsRecoilReduction;
    public Animator animator;
    public LayerMask bangable;

    protected void FireBullet(Vector3 direction)
    {
        Physics.Raycast(firePoint.position, firePoint.forward + direction, out RaycastHit hit, 500f);
        Debug.Log(hit.collider.gameObject.layer);
        if (hit.collider.gameObject.tag == "BangableWall" && Physics.Raycast(firePoint.position, firePoint.forward + direction, out RaycastHit newHit, 500f, layerMask: bangable))
        {
            newHit.collider.GetComponent<HitBox>()?.HitDamage((int)(damage * 0.9f));
            Instantiate(fakeHit, newHit.point, Quaternion.identity);
            LineRenderer newTrail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            newTrail.SetPosition(0, firePoint.position);
            newTrail.SetPosition(1, hit.point);
        }
        else if (hit.collider)
        {
            hit.collider.GetComponent<HitBox>()?.HitDamage(damage);
            Instantiate(fakeHit, hit.point, Quaternion.identity);
            LineRenderer newTrail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            newTrail.SetPosition(0, firePoint.position);
            newTrail.SetPosition(1, hit.point);
        }
        else
        {
            LineRenderer newTrail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            newTrail.SetPosition(0, firePoint.position);
            newTrail.SetPosition(1, firePoint.position + (firePoint.forward + direction) * 500);
        }
    }
}
