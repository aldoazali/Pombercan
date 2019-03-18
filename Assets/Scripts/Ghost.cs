using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Transform myTransform;
    public Transform[] waypoints;
    int current = 0;

    private float ghostSpeed = 0.06f;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(waypoints.Length > 0)
        {
            //Debug.Log(current);
            if (transform.position != waypoints[current].position)
            {
                Vector3 p = Vector3.MoveTowards(transform.position, waypoints[current].position, ghostSpeed);
                GetComponent<Rigidbody>().MovePosition(p);
            }
            // Waypoint reached, select next one
            else
            {
                if (waypoints[current].position.z == waypoints[(current + 1) % waypoints.Length].position.z)
                {
                    if (waypoints[current].position.x > waypoints[(current + 1) % waypoints.Length].position.x)
                    {
                        myTransform.rotation = Quaternion.Euler(0, 270, 0);
                    }
                    else if (waypoints[current].position.x < waypoints[(current + 1) % waypoints.Length].position.x)
                    {
                        myTransform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                }
                else if (waypoints[current].position.x == waypoints[(current + 1) % waypoints.Length].position.x)
                {
                    if (waypoints[current].position.z > waypoints[(current + 1) % waypoints.Length].position.z)
                    {
                        myTransform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (waypoints[current].position.z < waypoints[(current + 1) % waypoints.Length].position.z)
                    {
                        myTransform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                current = (current + 1) % waypoints.Length;
            }
        // Waypoint not reached yet? then move closer
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            Destroy(gameObject); // Destroys the player GameObject.
        }
    }

    }
