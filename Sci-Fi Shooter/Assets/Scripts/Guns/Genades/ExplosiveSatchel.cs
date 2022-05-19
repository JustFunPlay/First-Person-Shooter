using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveSatchel : CharacterHealth
{
    public GameObject boom;
    float radius;
    float damage;
    public PlayerControll player;
    bool die;

    public void Yeet(float damage_, float radius_, PlayerControll player_)
    {
        radius = radius_;
        damage = damage_;
        player = player_;
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 15), ForceMode.Impulse);
    }

    public void Detonate()
    {
        die = true;
        List<CharacterHealth> characters = new List<CharacterHealth>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInParent<CharacterHealth>())
            {
                bool b = false;
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].gameObject.GetInstanceID() == collider.GetComponentInParent<CharacterHealth>().gameObject.GetInstanceID())
                    {
                        b = true;
                    }
                }
                if (b == false)
                {
                    characters.Add(collider.GetComponentInParent<CharacterHealth>());
                }
            }
        }
        print(colliders.Length.ToString() + "/" + characters.Count.ToString());
        for (int i = 0; i < characters.Count; i++)
        {
            float dst = Vector3.Distance(characters[i].transform.position, transform.position);
            float damageToDo = damage * (1 - (dst / (radius * 2)));
            characters[i].OnTakeDamage((int)damageToDo);
        }
        GameObject bewm = Instantiate(boom, transform.position, transform.rotation);
        bewm.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //transform.parent = collision.transform;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    public override void OnDeath()
    {
        if (die == false)
        {
            die = true;
            player.inventory.grenades--;
            List<CharacterHealth> characters = new List<CharacterHealth>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius * 0.5f);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponentInParent<CharacterHealth>())
                {
                    bool b = false;
                    for (int i = 0; i < characters.Count; i++)
                    {
                        if (characters[i] = collider.GetComponentInParent<CharacterHealth>())
                        {
                            b = true;
                        }
                    }
                    if (b == false)
                    {
                        characters.Add(collider.GetComponentInParent<CharacterHealth>());
                    }
                }
            }
            for (int i = 0; i < characters.Count; i++)
            {
                float dst = Vector3.Distance(characters[i].transform.position, transform.position);
                float damageToDo = damage * (1 - (dst / (radius)));
                characters[i].OnTakeDamage((int)damageToDo);
            }
            GameObject bewm = Instantiate(boom, transform.position, transform.rotation);
            bewm.transform.localScale = new Vector3(radius, radius, radius);
            base.OnDeath();
        }
    }
}
