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
        transform.position = new Vector3(playerX,transform.position.y,transform.position.z);
    }
}
