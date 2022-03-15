using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivePulseGrenade : MonoBehaviour
{
    public int damage;
    public float blastDelay;
    public float blastRadius;
    public GameObject boom;
    public void YeetGrenade(int damage_, float delay_, float radius_)
    {
        damage = damage_;
        blastDelay = delay_;
        blastRadius = radius_;
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 50), ForceMode.Impulse);
        //GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(100, 100, 100), ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DelayExplosion());
    }
    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(blastDelay);
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInParent<CharacterHealth>())
            {
                float dst = Vector3.Distance(collider.GetComponentInParent<CharacterHealth>().transform.position, transform.position);
                float damageToDo = damage * (1 - (dst / (blastRadius * 2)));
                collider.GetComponentInParent<CharacterHealth>().OnTakeDamage((int)damageToDo);
            }
        }
        GameObject bewm = Instantiate(boom, transform.position, transform.rotation);
        bewm.transform.localScale = new Vector3(blastRadius * 2, blastRadius * 2, blastRadius * 2);
        Destroy(gameObject);
    }
}
