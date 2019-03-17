using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform[] waypoints;
    private Transform myTransform;
    private Animator animator;
    int cur = 0;

    private float ghostSpeed = 0.06f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position)
        {
            Vector3 p = Vector3.MoveTowards(transform.position, waypoints[cur].position, ghostSpeed);
            GetComponent<Rigidbody>().MovePosition(p);
            /*myTransform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walking", true);*/
        }
        // Waypoint reached, select next one
        else
        {
            cur = (cur + 1) % waypoints.Length;
        }

        // Animation
        /*Vector3 dir = waypoints[cur].position - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirZ", dir.z); */
    }
}
