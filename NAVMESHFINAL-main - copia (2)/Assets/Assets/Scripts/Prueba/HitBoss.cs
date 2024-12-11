using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoss : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("P3"))
        {
            coll.GetComponent<Personaje3D>().HP_Min -= damage;
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
