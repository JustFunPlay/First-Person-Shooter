using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeployExplosiveSatchel : GunBase
{
    public GameObject satchel;
    public float blastRadius;
    public Transform throwPoint;
    
    bool detonater;
    ExplosiveSatchel liveSatchel;

    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (canFire && callbackContext.canceled)
        {
            canFire = false;
            GameObject nade = Instantiate(satchel, throwPoint.position, throwPoint.rotation);
            nade.GetComponent<ExplosiveSatchel>().Yeet(damage, blastRadius, player);
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

    public override IEnumerator Equiping()
    {
        ExplosiveSatchel[] satchels = FindObjectsOfType<ExplosiveSatchel>();
        foreach (ExplosiveSatchel satchel_ in satchels)
        {
            if (satchel_.player == player)
            {
                liveSatchel = satchel_;
                detonater = true;
                break;
            }
        }
        yield return base.Equiping();
        if (detonater)
        {
            liveSatchel.Detonate();
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
}
