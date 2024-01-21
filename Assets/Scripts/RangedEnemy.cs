using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class RangedEnemy : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float nextWaypointDistance = .5f;
    [SerializeField] private float range = 10f;
    [SerializeField] private GameObject arrowGO;
    [SerializeField] private float arrowForce = 100;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float spread = 0.05f;


    float lastShootTime;
    Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;

    Seeker seeker;
    Rigidbody2D rb;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null) return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        }
        else
        {
            reachedEnd = false;
        }

        Vector2 waypointDirection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = waypointDirection * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        Vector2 playerDirection = ((Vector2)target.position - rb.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + playerDirection * 2, playerDirection, range-1);
        Debug.DrawLine(transform.position, hit.point);
        if(hit.transform != null && hit.transform.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if(Time.time - lastShootTime > waitTime)
            {
                Shoot(playerDirection);
                lastShootTime = Time.time;
            }
            
        }
        else
        {
            rb.AddForce(force);
        }


        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    void Shoot(Vector2 direction)
    {
        //object pooling would be better for optimization
        float xSpread = Random.RandomRange(-spread, spread);
        float ySpread = Random.RandomRange(-spread, spread);
        Vector2 spreadVector = new Vector2(xSpread, ySpread);
        GameObject arrow = (GameObject)Instantiate(arrowGO, (Vector2)transform.position + direction* 0.5f, Quaternion.Euler(direction));
        arrow.GetComponent<Rigidbody2D>().AddForce((direction + spreadVector) * arrowForce);
        Destroy(arrow, 1f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
}
