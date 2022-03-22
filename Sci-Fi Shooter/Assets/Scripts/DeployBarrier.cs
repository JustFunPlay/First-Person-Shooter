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
        yield return base.Equiping();
        if (player.inventory.weaponInventory[4].currentAmmo > 0)
        {
            player.inventory.weaponInventory[4].currentAmmo = 0;
            GameObject barrier = Instantiate(newBarrier, transform.position, transform.rotation);
            barrier.GetComponent<Barrier>().Yeet(duration, cooldown, cooldownPenalty, hp, player);
            player.EquipNext(player.previousWeapon);
        }
        else
        {
            player.EquipNext(player.previousWeapon);
        }
    }
}
