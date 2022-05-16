using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBase : MonoBehaviour
{
    public WeaponSlot weaponSlot;
    public AmmoType ammoType;
    public int damage;
    public int maxAmmo;
    public float equipTime;
    public float unEquipTime;
    public bool canFire;
    public PlayerControll player;
    public virtual void Fire(InputAction.CallbackContext callbackContext)
    {

    }
    public virtual void SecondaryFire(InputAction.CallbackContext callbackContext)
    {

    }
    public virtual void AltFire(InputAction.CallbackContext callbackContext)
    {

    }
    public virtual void Reload()
    {

    }
    public virtual void OnEquip(PlayerControll playerControll)
    {
        player = playerControll;
        player.UpdateAmmo(ammoType, weaponSlot);
        StartCoroutine(Equiping());
    }
    public virtual void OnUnEquip()
    {
        Destroy(gameObject);
    }
    public virtual IEnumerator Equiping()
    {
        yield return new WaitForSeconds(equipTime);
        canFire = true;
    }
}

[System.Serializable]
public enum AmmoType
{
    Light,
    Medium,
    Heavy,
    Shotgun,
    Grenade,
    RailGun,
    Other
}
[System.Serializable]
public enum WeaponSlot
{
    Primary,
    Secondary,
    Melee,
    Grenade,
    Ability
}
[System.Serializable]
public class FixedSprayPattern
{
    public Vector3 fixedSpray;
    public Vector3 fixedRecoil;
}
