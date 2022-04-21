using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RevealGrenade : GunBase
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
            nade.GetComponent<LiveRevealGreneda>().YeetGrenade(damage, blastDelay, blastRadius);
            player.EquipNext(player.previousWeapon);
        }
    }
}
