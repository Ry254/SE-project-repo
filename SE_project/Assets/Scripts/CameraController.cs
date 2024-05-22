using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    public Vector3 initialPosition;

    void Start()
    {
        player = GameObject.Find("Player");
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        float playerX = player.transform.position.x;
        float newX = 0;
        
        if(!(playerX < initialPosition.x)){
            newX = 1;
        }

        transform.position = new Vector3(playerX * newX ,transform.position.y,transform.position.z);
    }
}
