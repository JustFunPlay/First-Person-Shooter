using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployBarrier : GunBase
{
    public GameObject newBarrier;
    public int hp;
    public float duration;
    public float cooldown;
    public float cooldownPenalty;
    public override void OnEquip(PlayerControll playerControll)
    {
        base.OnEquip(playerControll);
    }
    public override IEnumerator Equiping()
    {
        player.weaponInHand.GetComponent<GunBase>().OnUnEquip();
        player.weaponInHand = gameObject;
        yield return base.Equiping();
        player.inventory.abilityAvailibility--;
        GameObject barrier = Instantiate(newBarrier, transform.position, transform.rotation);
        barrier.GetComponent<Barrier>().Yeet(duration, cooldown, cooldownPenalty, hp, player);
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
