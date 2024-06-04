using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager
{
    [SerializeField] int totalEnemies;
    [SerializeField] int playerHealth;
    
    [SerializeField] float teleportX = 100;

    AudioSource audioSource;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip youWinMusic;
    [SerializeField] AudioClip gameOverMusic;

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
    GameObject enemyCounterSprite;
    GameObject enemyCounter;
    TextMeshProUGUI enemyCounterText;
    PlayerController playerController;
    private bool gameEnd;

    private Camera mainCamera;
    private Color startingBackground = new Color(1,1,0.8392158f,0);
    private Color playerHitColor = new Color(1,1,0.75f,0);

    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        audioSource = GetComponent<AudioSource>();

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
        enemyCounterSprite = canvas.transform.GetChild(8).gameObject;
        enemyCounter = canvas.transform.GetChild(9).gameObject;

        enemyCounterText = enemyCounter.GetComponent<TextMeshProUGUI>();

        playerController = player.GetComponent<PlayerController>();
        playerHealth = playerController.health;

        SetHearts();
        gameEnd = false;
    }

    override protected void Update()
    {
        base.Update();
        if(totalEnemies != GameObject.FindGameObjectsWithTag("Enemy").Length){
            totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemyCounterText.text = totalEnemies.ToString();
        }

        if(playerHealth != playerController.health){
            playerHealth = playerController.health;
            SetHearts();
        }
        if(playerHealth < 1){
            GameOverText();
        } else if (totalEnemies < 1){
            YouWinText();
        }

        Vector3 playerPos = player.transform.position;

        if(playerPos.x < 0){
            player.transform.position = new Vector3(teleportX, playerPos.y, playerPos.z);
        } else if (playerPos.x > teleportX){
            player.transform.position = new Vector3(0, playerPos.y, playerPos.z);
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

    public void YouWinText(){
        if(!gameEnd){
            NoUI();
            restartBox.SetActive(true);
            youWinText.SetActive(true);
            gameEnd = true;
            audioSource.Stop();
            audioSource.PlayOneShot(youWinMusic, 1);
        }
    }

    public void GameOverText(){
        if(!gameEnd){
            NoUI();
            restartBox.SetActive(true);
            gameOverText.SetActive(true);
            gameEnd = true;
            audioSource.Stop();
            audioSource.PlayOneShot(gameOverMusic, 1);
        }
    }

    public void HitBackgroundOn(bool on){
        if(on){
            mainCamera.backgroundColor = playerHitColor;
        } else {
            mainCamera.backgroundColor = startingBackground;
        }
    }
}
