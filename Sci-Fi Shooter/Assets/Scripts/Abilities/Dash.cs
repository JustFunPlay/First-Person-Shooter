using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : GunBase
{
    public float cooldown;
    public float distancePerTick;
    public int totalTicks;
    public GameObject yeet;
    public override IEnumerator Equiping()
    {
        player.weaponInHand.GetComponent<GunBase>().OnUnEquip();
        player.weaponInHand = gameObject;
        yield return base.Equiping();
        player.inventory.abilityAvailibility--;
        GameObject dashObject = Instantiate(yeet, transform.position, Quaternion.identity);
        dashObject.GetComponent<Dashing>().Dash(totalTicks, cooldown, distancePerTick, player.gameObject);
        if (player.currentWeapon == WeaponSlot.Primary)
        {
            player.previousWeapon = player.currentWeapon;
            player.currentWeapon = WeaponSlot.Ability;
            player.EquipPrimary();
        }
        else if (player.currentWeapon == WeaponSlot.Secondary)
        {
            player.previousWeapon = player.currentWeapon;
            player.currentWeapon = WeaponSlot.Ability;
            player.EquipSecondary();
        }
        else if (player.currentWeapon == WeaponSlot.Melee)
        {
            player.previousWeapon = player.currentWeapon;
            player.currentWeapon = WeaponSlot.Ability;
            player.EquipMelee();
        }
        else if (player.previousWeapon == WeaponSlot.Primary)
        {
            player.previousWeapon = player.currentWeapon;
            player.currentWeapon = WeaponSlot.Ability;
            player.EquipPrimary();
        }
        else if (player.previousWeapon == WeaponSlot.Secondary)
        {
            player.previousWeapon = player.currentWeapon;
            player.currentWeapon = WeaponSlot.Ability;
            player.EquipSecondary();
        }
        else
        {
            player.previousWeapon = player.currentWeapon;
            player.currentWeapon = WeaponSlot.Ability;
            player.EquipMelee();
        }
    }
}
