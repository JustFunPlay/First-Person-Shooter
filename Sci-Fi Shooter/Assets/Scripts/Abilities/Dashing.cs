using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    GameObject player;
    int ticks;
    public float cooldown;
    float distancePerTick;
    Vector3 dashDir;
    
    public void Dash(int ticks_, float cooldown_, float distancePerTick_, GameObject player_)
    {
        Dashing[] dashings = FindObjectsOfType<Dashing>();
        foreach (Dashing dash in dashings)
        {
            if (dash.cooldown > this.cooldown)
            {
                cooldown = dash.cooldown;
            }
        }
        ticks = ticks_;
        cooldown += cooldown_;
        distancePerTick = distancePerTick_;
        player = player_;
        dashDir = player.GetComponent<PlayerControll>().camRot.forward;
        StartCoroutine(CoolingDown());
        StartCoroutine(DashForward());
    }
    IEnumerator DashForward()
    {
        for (int i = 0; i < ticks; i++)
        {
            if (Physics.Raycast(player.transform.position, dashDir, 1.5f))
            {
                break;
            }
            player.GetComponent<Rigidbody>().MovePosition(player.transform.position + dashDir * distancePerTick);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator CoolingDown()
    {
        while (cooldown > 0)
        {
            yield return null;
            cooldown -= Time.deltaTime;
        }
        player.GetComponent<PlayerControll>().inventory.abilityAvailibility++;
        Destroy(gameObject);
    }
}
