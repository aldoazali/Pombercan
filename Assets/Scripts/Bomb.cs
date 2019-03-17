using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    public LayerMask levelMask;
    private bool exploded = false;

    public GameObject Player ;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 3f);
    }   

    // Update is called once per frame
    void Update()
    {
        
    }


    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Spawns an explosion at the bomb’s position
        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));
        GetComponent<MeshRenderer>().enabled = false; // Disables the mesh renderer, making the bomb invisible.
        exploded = true;
        Destroy(gameObject, .3f); // Destroys the bomb after 0.3 seconds; this ensures all explosions will spawn before the GameObject is destroyed.
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        // Iterates a for loop for every unit of distance you want the explosions to cover.In this case, the explosion will reach two meters.
        for (int i = 1; i < 2; i++)
        {
            // A RaycastHit object holds all the information about what and at which position the Raycast hits -- or doesn't hit.
            RaycastHit hit;
            /* This important line of code sends out a raycast from the center of the bomb towards the direction you passed through the StartCoroutine call. 
             It then outputs the result to the RaycastHit object. The i parameter dictates the distance the ray should travel. 
             Finally, it uses a LayerMask named levelMask to make sure the ray only checks for blocks in the level and ignores the player and other colliders. */
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask);

            // If the raycast doesn't hit anything then it's a free tile.
            if (!hit.collider)
            {
                // Spawns an explosion at the position the raycast checked.
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            // The raycast hits a block.
            else
            { // Once the raycast hits a block, it breaks out of the for loop. This ensures the explosion can't jump over walls.
                break;
            }

            // Waits for 0.05 seconds before doing the next iteration of the for loop. This makes the explosion more convincing by making it look like it's expanding outwards.
            yield return new WaitForSeconds(.1f); //0.05
        }
    }

    public void OnTriggerEnter(Collider other)
    {   
        if (!exploded && other.CompareTag("Explosion"))
        { // Checks the the bomb hasn't exploded. Checks if the trigger collider has the Explosion tag assigned.
            CancelInvoke("Explode"); // Cancel the already called Explode invocation by dropping the bomb -- if you don't do this the bomb might explode twice.
            Explode(); // Explode!
        }
    }

}
