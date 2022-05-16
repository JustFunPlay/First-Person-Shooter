using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PulseGrenade : GunBase
{
    public GameObject liveGrenade;
    public float blastRadius;
    public float blastDelay;
    public Transform throwpoint;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (canFire && callbackContext.canceled)
        {
            canFire = false;
            GameObject nade = Instantiate(liveGrenade, throwpoint.position, throwpoint.rotation);
            nade.GetComponent<LivePulseGrenade>().YeetGrenade(damage, blastDelay, blastRadius);
            player.inventory.grenades--;
            if (player.previousWeapon == WeaponSlot.Primary)
            {
                player.EquipPrimary();
            }
            else if (player.previousWeapon == WeaponSlot.Secondary)
            {
                player.EquipSecondary();
            }
            else
            {
                player.EquipMelee();
            }
        }
    }
    public override void AltFire(InputAction.CallbackContext callbackContext)
    {
        
    }
}
