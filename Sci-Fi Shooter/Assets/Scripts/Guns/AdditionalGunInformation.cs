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
}
