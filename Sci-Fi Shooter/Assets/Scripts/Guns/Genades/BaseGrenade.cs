using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrenade : MonoBehaviour
{
    LineRenderer trail;
    public LineRenderer trailToSpawn;

    protected virtual void Start()
    {
        trail = Instantiate(trailToSpawn);
        trail.positionCount = 30;
        for (int i = 0; i < trail.positionCount; i++)
        {
            trail.SetPosition(i, transform.position);
        }
    }
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        for (int i = 0; i < trail.positionCount; i++)
        {
            if (i != trail.positionCount - 1)
                trail.SetPosition(i, trail.GetPosition(i + 1));
            else
                trail.SetPosition(i, transform.position);
        }   
    }
    protected virtual void OnDestroy()
    {
        Destroy(trail.gameObject);
    }
}
