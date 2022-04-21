using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : CharacterHealth
{
    public float duration;
    public float cooldown;
    public float cooldownPenalty;
    public GameObject physicalBarrier;
    public PlayerControll player;

    public void Yeet(float dur_, float cd_, float cdp_, int hp_, PlayerControll player_)
    {
        duration = dur_;
        cooldown = cd_;
        cooldownPenalty = cdp_;
        maxHP = hp_;
        currentHP = maxHP;
        player = player_;
        StartCoroutine(CoolingDown());
        StartCoroutine(TimeOut());
    }
    public override void OnDeath()
    {
        physicalBarrier.SetActive(false);
        cooldown += cooldownPenalty;
    }
    IEnumerator TimeOut()
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    transform.Translate(0, 0, 0.2f);
        //    yield return new WaitForFixedUpdate();
        //}
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, 400, ForceMode.Force);
        yield return new WaitForSeconds(0.25f);
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(duration);
        physicalBarrier.SetActive(false);
    }
    IEnumerator CoolingDown()
    {
        while (cooldown > 0)
        {
            yield return null;
            cooldown -= Time.deltaTime;
        }
        player.inventory.weaponInventory[4].currentAmmo = 1;
    }
}
