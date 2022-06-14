using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knife : GunBase
{
    public float timeToHit;
    public float timeAfterHit;
    public GameObject fakeHit;
    public Animator animator;

    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && canFire)
        {
            StartCoroutine(Stabbing());
        }
    }
    IEnumerator Stabbing()
    {
        canFire = false;
        animator.SetTrigger("Stab");
        yield return new WaitForSeconds(timeToHit);
        if (Physics.Raycast(player.cam.position, player.cam.forward, out RaycastHit hit, 1.5f))
        {

        }
        yield return new WaitForSeconds(timeAfterHit);
        canFire = true;
    }
}
