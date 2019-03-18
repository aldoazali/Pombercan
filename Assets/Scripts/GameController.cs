using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private int deadPlayers = 0; // hold the amount of players that died.
    //private bool gameOver;
   // private bool restart;
    private int score;

    //Not Implemented yet
    public int MaxbombsAllowedPerPlayer = 4;
    public int bombsPerPlayer = 4;

    public Text scoreText;

    void UpdateScore()
    {
        //scoreText.text = "Score: " + score;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
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

    public void Start()
    {
        //gameOver = false;
        //restart = false;
        score = 0;
        deadPlayers = 0;
        UpdateScore();
    }

    public int getDeadPlayers()
    {
        return deadPlayers;
    }


    public void PlayerDied()
    {
        deadPlayers++; // Adds one dead player.

        if (deadPlayers >= 1)
        {
            //gameOver = true;
            Debug.Log("Player has died");
            SceneManager.LoadScene("Over");
        }
    }

    

    
    
}
