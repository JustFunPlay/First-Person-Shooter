using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBase : MonoBehaviour
{
    public AmmoType ammoType;
    public int damage;
    public int maxAmmo;
    public float equipTime;
    public bool canFire;
    public virtual void Fire(InputAction.CallbackContext callbackContext)
    {

    }
    public virtual void AltFire(InputAction.CallbackContext callbackContext)
    {

    }
    public virtual void Reload()
    {

    }
    public virtual void OnEquip()
    {

    }
    public virtual void OnUnEquip()
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
