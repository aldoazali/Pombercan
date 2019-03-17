using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    /*public SimpleObjectPool answerButtonObjectPool;
    public Text questionText;
    public Text scoreDisplay;
    public Text timeRemainingDisplay;
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive = false;
    private float timeRemaining;
    
    private int questionIndex;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>(); */

    public Text highScoreDisplay;
    private int playerScore;

    void Start()
    {
        /*dataController = FindObjectOfType<DataController>();                              // Store a reference to the DataController so we can request the data we need for this round

        currentRoundData = dataController.GetCurrentRoundData();                            // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
        questionPool = currentRoundData.questions;                                          // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object

        timeRemaining = currentRoundData.timeLimitInSeconds;                                // Set the time limit for this round based on the RoundData object
        UpdateTimeRemainingDisplay();
        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true; */
    }

    void Update()
    {
        /*if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;                                                // If the round is active, subtract the time since Update() was last called from timeRemaining
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)                                                     // If timeRemaining is 0 or less, the round ends
            {
                EndRound();
            }
        } */
    }
    

    private void UpdateTimeRemainingDisplay()
    {
        //timeRemainingDisplay.text = Mathf.Round(timeRemaining).ToString();
    }

    public void EndRound()
    {
        /*isRoundActive = false;

        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true); */
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}