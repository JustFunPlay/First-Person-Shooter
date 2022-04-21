using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public GameObject[] meshes;

    public  virtual void OnTakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <=0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public void RevealThroughWalls(float duration)
    {
        StartCoroutine(Revealing(duration));
    }
    IEnumerator Revealing(float duration)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].layer = 6;
        }
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].layer = 0;
        }
    }
}
