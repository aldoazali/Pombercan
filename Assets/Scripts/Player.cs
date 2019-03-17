/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public GlobalStateManager globalManager;
    //This is a reference to the GlobalStateManager, a script that is notified of all player deaths and determines which player won.

    //Player parameters
    [Range (1, 2)] //Enables a nifty slider in the editor
    public int playerNumber = 1;
    //Indicates what player this is: P1 or P2
    const float baseMoveSpeed = 4f;
    public float moveSpeed;
    public bool canDropBombs = true;
    //Can the player drop bombs?
    public bool canMove = true;
    //Can the player move?
    

    public bool dead = false;
    //This variable is used to keep track if the player died to an explosion.

    private int MaxbombsAllowedPerPlayer = 4;
    private int bombsPerPlayer = 4;
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
        if(getBombsAmount() > 0) // 
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

        //Depending on the player number, use different input for moving
        if (playerNumber == 1)
        {
            UpdatePlayer1Movement ();
        } else
        {
            UpdatePlayer2Movement ();
        }
    }


    // Not implemented yet
    public void setIncreaseBombs() // to set amounts of bombs the player has left to drop + 1
    {
        if (bombsPerPlayer < MaxbombsAllowedPerPlayer)
            bombsPerPlayer++;
    }

    public void setDecreaseBombs() // to set amounts of bombs the player has left to drop - 1
    {
        if (bombsPerPlayer < MaxbombsAllowedPerPlayer)
            bombsPerPlayer--;
    }

    public int getBombsAmount()
    {
        return bombsPerPlayer;
    }

    /// <summary>
    /// Updates Player 1's movement and facing rotation using the WASD keys and drops bombs using Space
    /// </summary>
    private void UpdatePlayer1Movement ()
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
            //setDecreaseBombs();
        }

        

    }
    
    /// Updates Player 2's movement and facing rotation using the arrow keys and drops bombs using Enter or Return
    private void UpdatePlayer2Movement ()
    {
        if (Input.GetKey (KeyCode.DownArrow))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.UpArrow))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        { //Drop Bomb. For Player 2's bombs, allow both the numeric enter as the return key or players 
            //without a numpad will be unable to drop bombs
            DropBomb ();
        }
    }

    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
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
            Debug.Log ("Player" + playerNumber + " hit by explosion!");
            dead = true; //  Sets the dead variable so you can keep track of the player's death.
            globalManager.PlayerDied(playerNumber); // Notifies the global state manager that the player died.
            Destroy(gameObject); // Destroys the player GameObject.
        }

        if (other.CompareTag("Ghost"))
        {
            Debug.Log("Player die by Ghost!");
            dead = true; //  Sets the dead variable so you can keep track of the player's death.
            globalManager.PlayerDied(playerNumber); // Notifies the global state manager that the player died.
            Destroy(gameObject); // Destroys the player GameObject.
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        // Check if the collider we hit has a rigidbody
        // Then apply the force
        if (collision.rigidbody)
        {
            //collision.rigidbody.AddForce(Vector3.up * 15);
            Debug.Log("Player is collide!!");
            moveSpeed = baseMoveSpeed;
        }
    }
}
