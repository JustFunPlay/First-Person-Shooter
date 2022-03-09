using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public AmmoType ammoType;
    public virtual void Fire()
    {

    }
    public virtual void Reload()
    {

    }
}

[System.Serializable]
public enum AmmoType
{
    Light,
    Medium,
    Heavy,
    Shotgun
}
