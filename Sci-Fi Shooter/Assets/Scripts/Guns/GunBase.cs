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
    public float unEquipTime;
    public bool canFire;
    public PlayerControll player;
    public virtual void Fire(InputAction.CallbackContext callbackContext)
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
        player.UpdateAmmo(ammoType);
        StartCoroutine(Equiping());
    }
    public virtual void OnUnEquip()
    {

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
    Grenade
}
