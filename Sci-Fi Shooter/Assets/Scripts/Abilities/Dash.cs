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
        yield return base.Equiping();
        if (player.inventory.weaponInventory[4].currentAmmo > 0)
        {
            player.inventory.weaponInventory[4].currentAmmo = 0;
            GameObject dashObject = Instantiate(yeet, transform.position, Quaternion.identity);
            dashObject.GetComponent<Dashing>().Dash(totalTicks, cooldown, distancePerTick, player.gameObject);
            player.EquipNext(player.previousWeapon);
        }
        else
        {
            player.EquipNext(player.previousWeapon);
        }
    }
}
