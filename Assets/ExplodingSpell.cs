using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingSpell : MonoBehaviour
{
    float stopwatch = 0;
    float timer = 0.4f;

    [SerializeField] public GameObject small;
    [HideInInspector] public Vector2 direction;

    void Update()
    {
        stopwatch += Time.deltaTime;
        transform.position += (Vector3)(direction.normalized * 30) * Time.deltaTime;
        if(stopwatch > timer)
        {
            explode();
        }
    }

    void explode()
    {
        Instantiate(small, transform.position, Quaternion.identity, null).
            GetComponent<SmallExplodingSpell>().direction = new Vector2(direction.normalized.x, direction.normalized.y);
        Instantiate(small, transform.position, Quaternion.identity, null).
            GetComponent<SmallExplodingSpell>().direction = new Vector2(-direction.normalized.x, -direction.normalized.y);
        Instantiate(small, transform.position, Quaternion.identity, null).
            GetComponent<SmallExplodingSpell>().direction = new Vector2(-direction.normalized.y, direction.normalized.x);
        Instantiate(small, transform.position, Quaternion.identity, null).
            GetComponent<SmallExplodingSpell>().direction = new Vector2(direction.normalized.y, -direction.normalized.x);
        Destroy(gameObject);
    }
}
