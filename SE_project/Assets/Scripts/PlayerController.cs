using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Character
{
    public float playerBase{get; private set;}
    [SerializeField] float teleportX = 100;

    protected override void Awake()
    {
        base.Awake();
        playerBase = transform.localScale.y/2;
    }

    protected override void Update()
    {
        base.Update();
        if(health > 0){
            bool jumpDown = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            bool attackDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

            if(attackDown && !attacking){
                StartCoroutine(AttackingTime());
            }

            if(jumpDown && (onGround || Rigidbody.velocity.y == 0)){
                Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                onGround = false;
                if(!attacking){
                    SpritesOff();
                    jump.SetActive(true);
                }
            }

            if(Rigidbody.velocity.y == 0 && !onGround){
                onGround = true;
                Walk0On();
            }
        }
    }

    void FixedUpdate()
    {
        if(health > 0){
            float horizontalAxis = Input.GetAxis("Horizontal");
            transform.Translate(Vector2.right * Time.deltaTime * speed * horizontalAxis);

            if(Math.Abs(horizontalAxis) > 0){
                walking = true;
            } else {
                walking = false;
            }

            if((horizontalAxis > 0 && !facingRight) || (horizontalAxis < 0 && facingRight)){
                MirrorXAxis();
            }
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if(transform.position.x < 0){
            transform.position = new Vector3(teleportX, transform.position.y, transform.position.z);
        } else if (transform.position.x > teleportX){
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        base.OnCollisionEnter2D(collisionInfo);

        if(collisionInfo.gameObject.CompareTag("Enemy") && !invincible){
            loseHealth(1);
            StartCoroutine(InvincibleSeconds());
        }
    }

    protected override void IsDead()
    {
        base.IsDead();
    }

    IEnumerator AttackingTime(){
        attacking = true;
        invincible = true;
        bullet.SetActive(true);
        SpritesOff();
        attack.SetActive(true);
        yield return new WaitForSeconds(attackingTime/2);
        invincible = false;
        yield return new WaitForSeconds(attackingTime/2);
        attacking = false;
        bullet.SetActive(false);
        Walk0On();
    }
}
