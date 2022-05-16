using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knife : GunBase
{
    public override void Fire(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && canFire)
        {
            print("stab");
        }
    }
}
