using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int totalEnemies;
    [SerializeField] int playerHealth;

    GameObject player;
    GameObject canvas;
    GameObject heart0;
    GameObject heart1;
    GameObject heart2;
    GameObject heart3;
    GameObject heart4;
    GameObject restartBox;
    GameObject youWinText;
    GameObject gameOverText;
    PlayerController playerController;
    private bool gameEnd;

    void Awake()
    {
        player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        heart0 = canvas.transform.GetChild(0).gameObject;
        heart1 = canvas.transform.GetChild(1).gameObject;
        heart2 = canvas.transform.GetChild(2).gameObject;
        heart3 = canvas.transform.GetChild(3).gameObject;
        heart4 = canvas.transform.GetChild(4).gameObject;
        restartBox = canvas.transform.GetChild(5).gameObject;
        youWinText = canvas.transform.GetChild(6).gameObject;
        gameOverText = canvas.transform.GetChild(7).gameObject;

        playerController = player.GetComponent<PlayerController>();
        playerHealth = playerController.health;

        SetHearts();
        gameEnd = false;
    }

    void Update()
    {
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if(playerHealth != playerController.health){
            playerHealth = playerController.health;
            SetHearts();
        }
        if(playerHealth < 1){
            GameOverText();
        } else if (totalEnemies < 1){
            YouWinText();
        }
    }

    void SetHearts(){
        if(!gameEnd){
            NoUI();
            switch(playerHealth){
                case 5:
                    heart0.SetActive(true);
                    heart1.SetActive(true);
                    heart2.SetActive(true);
                    heart3.SetActive(true);
                    heart4.SetActive(true);
                    break;
                case 4:
                    heart0.SetActive(true);
                    heart1.SetActive(true);
                    heart2.SetActive(true);
                    heart3.SetActive(true);
                    break;
                case 3:
                    heart0.SetActive(true);
                    heart1.SetActive(true);
                    heart2.SetActive(true);
                    break;
                case 2:
                    heart0.SetActive(true);
                    heart1.SetActive(true);
                    break;
                case 1:
                    heart0.SetActive(true);
                    break;
            }
        }    
    }

    void NoUI(){
        heart0.SetActive(false);
        heart1.SetActive(false);
        heart2.SetActive(false);
        heart3.SetActive(false);
        heart4.SetActive(false);
        restartBox.SetActive(false);
        youWinText.SetActive(false);
        gameOverText.SetActive(false);
    }

    public void ResetGame(){
        SceneManager.LoadScene(0);
        Debug.Log("clicked");
    }

    public void YouWinText(){
        if(!gameEnd){
            NoUI();
            restartBox.SetActive(true);
            youWinText.SetActive(true);
            gameEnd = true;
        }
    }

    public void GameOverText(){
        if(!gameEnd){
            NoUI();
            restartBox.SetActive(true);
            gameOverText.SetActive(true);
            gameEnd = true;
        }
    }
}
