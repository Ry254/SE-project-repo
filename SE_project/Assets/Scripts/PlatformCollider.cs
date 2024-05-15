using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private BoxCollider2D platformCollider;
    private GameObject player;
    private PlayerController playerController;
    private float platformBase;
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        platformCollider = GetComponent<BoxCollider2D>();
        platformCollider.enabled = false;

        platformBase = transform.localScale.y/2;
    }

    void Update()
    {
        if(BelowPlayer()){
            platformCollider.enabled = true;
        } else {
            platformCollider.enabled = false;
        }
    }

    public bool BelowPlayer(){
        float playerBase = player.transform.position.y - playerController.playerBase;
        float swapY = transform.position.y + platformBase;
        return(playerBase >= swapY);
    }
}
