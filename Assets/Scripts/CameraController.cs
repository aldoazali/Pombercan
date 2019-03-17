using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GlobalStateManager globalManager;
    public GameObject player;       //Public variable to store a reference to the player game object
    
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()

    {
        if (globalManager.getDeadPlayers() == 0)
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            Vector3 posisi;
            posisi = this.transform.position;
            posisi.x = player.transform.position.x + offset.x;
            posisi.z = player.transform.position.z + offset.z;
            this.transform.position = posisi;
        }
    }
}