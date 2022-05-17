using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveRevealGreneda : MonoBehaviour
{
    float duration;
    float radius;
    float delay;
    public GameObject boom;
    
    public void YeetGrenade(float duration_, float delay_, float radius_)
    {
        duration = duration_;
        radius = radius_;
        delay = delay_;
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 35), ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DelayExplosion());
    }
    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(delay);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInParent<CharacterHealth>())
            {
                StopCoroutine(collider.GetComponentInParent<CharacterHealth>().Revealing(duration));
                collider.GetComponentInParent<CharacterHealth>().RevealThroughWalls(duration);
            }
        }
        GameObject bewm = Instantiate(boom, transform.position, transform.rotation);
        bewm.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        Destroy(gameObject);
    }
}
