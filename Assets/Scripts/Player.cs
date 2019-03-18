
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameController gameController;
    //This is a reference to the GameController

    const float baseMoveSpeed = 4f;
    public float moveSpeed;
    public bool canDropBombs = true;
    //Can the player drop bombs?
    public bool canMove = true;
    //Can the player move?
    

    
    //Amount of bombs the player has left to drop, gets decreased as the player drops a bomb, increases as an owned bomb explodes

    private int mouseDirection; // 0 stop, 1 up, 2 down, 3 left, 4 right

    //Prefabs
    public GameObject bombPrefab;

    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        moveSpeed = baseMoveSpeed;
        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
        animator = myTransform.Find ("PlayerModel").GetComponent<Animator> ();
        mouseDirection = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        if(moveSpeed > 8)
        {
            moveSpeed = 8;
        }
        if(moveSpeed > 4)
        {
            moveSpeed -= 0.45f;
        }
        if(gameController.getBombsAmount() > 0) // 
        {
            canDropBombs = true;
        }
        else
        {
            canDropBombs = false;
        }
        UpdateMovement ();
    }

    private void UpdateMovement ()
    {
        animator.SetBool ("Walking", false);

        if (!canMove)
        { //Return if player can't move
            moveSpeed = baseMoveSpeed;
            return;
        }
        UpdatePlayerMovement();
    }
    
    
    // Updates Player 1's movement and facing rotation using the WASD keys and drops bombs using Space
    private void UpdatePlayerMovement ()
    {
        // Mouse Input
        if (Input.GetAxis("Mouse Y") > 0)
        { //Up movement
            mouseDirection = 1;
        }
        else if (Input.GetAxis("Mouse Y") < 0)
        { //Down movement
            mouseDirection = 2;
        }
        else if (Input.GetAxis("Mouse X") < 0)
        { //Left movement
            mouseDirection = 3;
        }
        else if (Input.GetAxis("Mouse X") > 0)
        { //Right movement
            mouseDirection = 4;
        }
        else 
        {
            mouseDirection = 0;
        }

        // Mouse and keyboard is flipped because of camera position
        if (Input.GetKey (KeyCode.S) || mouseDirection == 2)
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
            moveSpeed += 0.5f;
        }

        if (Input.GetKey (KeyCode.D) || mouseDirection == 4)
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
            moveSpeed += 0.5f;
        }

        if (Input.GetKey (KeyCode.W) || mouseDirection == 1)
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
            moveSpeed += 0.5f;
        }
        if (Input.GetKey (KeyCode.A) || mouseDirection == 3)
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
            moveSpeed += 0.5f;
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButton(0)))
        { //Drop bomb
            DropBomb ();
            mouseDirection = 0;
            //gameController.setDecreaseBombs();
        }
    }
    
    // Drops a bomb beneath the player
    private void DropBomb ()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            //Instantiate(bombPrefab, myTransform.position, bombPrefab.transform.rotation);
            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)), bombPrefab.transform.rotation);

        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Explosion"))
        {
            Debug.Log ("Player hit by explosion!");
            gameController.PlayerDied(); // Notifies the global state manager that the player died.
            Destroy(gameObject); // Destroys the player GameObject.
            //SceneManager.LoadScene("Over");
        }

        if (other.CompareTag("Ghost"))
        {
            Debug.Log("Player die by Ghost!");
            gameController.PlayerDied(); // Notifies the global state manager that the player died.
            Destroy(gameObject); // Destroys the player GameObject.
            //SceneManager.LoadScene("Over");
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        // Check if the collider we hit has a rigidbody
        // Then apply the force
        if (collision.rigidbody)
        {
            //collision.rigidbody.AddForce(Vector3.up * 15);
            //Debug.Log("Player is collide!!");
            moveSpeed = baseMoveSpeed;
        }
    }
}
