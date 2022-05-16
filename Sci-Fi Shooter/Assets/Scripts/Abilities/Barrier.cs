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
        Barrier[] barriers = FindObjectsOfType<Barrier>();
        foreach (Barrier barrier in barriers)
        {
            if (barrier.cooldown > this.cooldown)
            {
                cooldown = barrier.cooldown;
            }
        }
        duration = dur_;
        cooldown += cd_ + dur_;
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
        player.inventory.abilityAvailibility++;
        Destroy(gameObject);
    }
}
