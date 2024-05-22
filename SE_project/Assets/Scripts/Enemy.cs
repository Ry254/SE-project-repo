using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Character
{
    [SerializeField] float chasingSpeed;
    [SerializeField] float maxRadius = 5f;
    [SerializeField] float radius;
    Vector3 startPos;
    [SerializeField] float turningTime = .75f;
    [SerializeField] int direction;
    [SerializeField] bool chasingPlayer;

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
        facingRight = false;
        direction = -1;

        InvokeRepeating("RandomDirection", 0, Random.Range(0,3f));
    }

    void Update()
    {
        radius = transform.position.x - startPos.x;
        if(math.abs(radius) >= maxRadius){
            StartCoroutine(AtMaxRadius());
            transform.Translate(-(Vector2.right * radius).normalized * speed * Time.deltaTime);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime * direction);

        if((direction > 0 && !facingRight) || (direction < 0 && facingRight)){
            MirrorXAxis();
        }
    }

    void RandomDirection(){
        StartCoroutine(SwappingDirections());
    }

    IEnumerator SwappingDirections(){
        int newDirection = Random.Range(-1,2);
        if(newDirection == direction){
            newDirection = Random.Range(-1,2);
        }
        direction = 0;

        yield return new WaitForSeconds(turningTime);
        
        direction = newDirection;

        if(direction != 0){
            walking = true;
        } else {
            walking = false;
        }
    }

    IEnumerator AtMaxRadius(){
        int oldDirection = direction;
        direction = 0;
        yield return new WaitForSeconds(turningTime);
        direction = -oldDirection;
    }

    protected override void IsDead()
    {
        Destroy(gameObject);
    }   
}
