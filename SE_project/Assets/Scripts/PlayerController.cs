using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Character
{
    public float playerBase{get; private set;}
    private GameManager gameManager;

    AudioSource audioSource;
    [SerializeField] AudioClip playerHit;
    [SerializeField] AudioClip playerJump;
    [SerializeField] AudioClip playerAttack;


    protected override void Awake()
    {
        base.Awake();
        playerBase = transform.localScale.y/2;

        audioSource = GetComponent<AudioSource>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if(health > 0){
            bool jumpDown = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            bool attackDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

            if(attackDown && !attacking){
                audioSource.PlayOneShot(playerAttack, .5f);
                StartCoroutine(AttackingTime());
            }

            if(jumpDown && ((onGround && Rigidbody.velocity.y == 0) || (runOffGround && Rigidbody.velocity.y < 0))){
                Debug.Log(runOffGround);
                Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);

                audioSource.PlayOneShot(playerJump);

                onGround = false;
                if(!attacking && !gotHit){
                    SpritesOff();
                    jump.SetActive(true);
                }
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
    protected override void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        base.OnCollisionEnter2D(collisionInfo);

        if(collisionInfo.gameObject.CompareTag("Enemy") && !invincible){
            audioSource.PlayOneShot(playerHit);
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

    protected override IEnumerator GotHit()
    {
        gotHit = true;
        SpritesOff();
        bullet.SetActive(false);
        hit.SetActive(true);
        gameManager.HitBackgroundOn(true);
        yield return new WaitForSeconds(hitTime);
        gotHit = false;
        Walk0On();
        gameManager.HitBackgroundOn(false);
    }
}
