using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;
    public float snapines;
    public float returnspeed;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnspeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapines * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void Recoil(Vector3 recoil)
    {
        targetRotation += new Vector3(-recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
    }
}
