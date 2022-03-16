using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplacementGrenade : GunBase
{
    public GameObject liveGrenade;
    public Transform throwPoint;
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (canFire && callbackContext.canceled)
        {
            canFire = false;
            GameObject nade = Instantiate(liveGrenade, throwPoint.position, throwPoint.rotation);
            nade.GetComponent<LiveDisplacementGrenade>().YeetGrenade();
            player.EquipNext(player.previousWeapon);
        }
    }
    public override IEnumerator Equiping()
    {
        yield return base.Equiping();
        LiveDisplacementGrenade grenade = FindObjectOfType<LiveDisplacementGrenade>();
        if (grenade)
        {
            grenade.TeleportPlayer(player);
            yield return null;
            player.EquipNext(player.previousWeapon);
        }
    }
}
