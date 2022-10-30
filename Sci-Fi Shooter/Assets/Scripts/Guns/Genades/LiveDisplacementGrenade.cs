using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveDisplacementGrenade : BaseGrenade
{
    public void YeetGrenade()
    {
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 35), ForceMode.Impulse);
    }

    public void TeleportPlayer(PlayerControll player)
    {
        player.transform.position = transform.position + Vector3.up * 1.1f;
        Destroy(gameObject);
    }
}
