using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplodingSpell : MonoBehaviour
{
    float stopwatch = 0;
    float timer = 0.3f;

    [HideInInspector] public Vector2 direction;

    void Update()
    {
        stopwatch += Time.deltaTime;
        transform.position += (Vector3)(direction.normalized * 30) * Time.deltaTime;
        if (stopwatch > timer)
        {
            explode();
        }
    }

    void explode()
    {
        Destroy(gameObject);
    }
}
