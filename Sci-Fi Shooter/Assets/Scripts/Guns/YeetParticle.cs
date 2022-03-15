using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetParticle : MonoBehaviour
{
    public float lifeTime;
    void Start()
    {
        StartCoroutine(TimeToDeath());
    }
    IEnumerator TimeToDeath()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
