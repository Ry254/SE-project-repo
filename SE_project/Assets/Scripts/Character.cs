using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    public Rigidbody2D Rigidbody{get; protected set;}
    public Collider2D Collider{get; protected set;}
    [SerializeField] protected bool onGround;
    [SerializeField] protected bool runOffGround;
    [SerializeField] public int health = 1;
    [SerializeField] protected bool invincible = false;
    [SerializeField] protected float invincibleSeconds = 1;
    [SerializeField] protected bool facingRight = true;
    protected GameObject bullet;
    protected GameObject walk0;
    protected GameObject walk1;
    protected GameObject walk2;
    protected GameObject walk3;
    protected GameObject attack;
    protected GameObject jump;
    protected GameObject death;
    protected GameObject hit;
    [SerializeField] protected bool attacking;
    [SerializeField] protected bool walking;
    [SerializeField] protected int walkCycle;
    [SerializeField] protected float walkCycleTime = .25f;
    [SerializeField] protected float attackingTime = .1f;
    [SerializeField] protected bool gotHit;
    [SerializeField] protected float hitTime = .25f;

    protected virtual void Awake()
    {
        Collider = GetComponent<Collider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
        onGround = true;
        runOffGround = false;

        bullet = transform.GetChild(0).gameObject;
        bullet.SetActive(false);
        attacking = false;
        walking = false;
        gotHit = false;

        walk0 = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        walk1 = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        walk2 = transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
        walk3 = transform.GetChild(1).gameObject.transform.GetChild(3).gameObject;
        attack = transform.GetChild(1).gameObject.transform.GetChild(4).gameObject;
        jump = transform.GetChild(1).gameObject.transform.GetChild(5).gameObject;
        death = transform.GetChild(1).gameObject.transform.GetChild(6).gameObject;
        hit = transform.GetChild(1).gameObject.transform.GetChild(7).gameObject;
        Walk0On();
    }

    protected virtual void Start(){
        InvokeRepeating("ChangeWalkSprite", 0, walkCycleTime);
    }
    
    protected virtual void LateUpdate()
    {
        if(health < 1){
            IsDead();
        }
    }

    protected virtual void Update(){
        if(health > 0 && Rigidbody.velocity.y == 0 && !onGround){
            onGround = true;
            Walk0On();
        }
        if(transform.position.y < -2){
            health = 0;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Ground") && Rigidbody.velocity.y <= 0 && transform.position.y > collisionInfo.gameObject.transform.position.y){
            onGround = true;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Ground")){
            float characterBottom = -gameObject.transform.localScale.y/2 + transform.position.y;
            float objectTop = collisionInfo.gameObject.transform.localScale.y/2 + transform.position.y;

            if(characterBottom < objectTop){
                StartCoroutine(OnGroundExit());
            } else {
                onGround = false;
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Bullet") && !invincible){
            loseHealth(other.gameObject.GetComponent<Bullet>().attackDamage);
            StartCoroutine(InvincibleSeconds());
        }
    }

    protected virtual void IsDead(){
        SpritesOff();
        death.SetActive(true);
        Collider.enabled = false;
        Rigidbody.isKinematic = true;
        Rigidbody.velocity = new Vector2(0,0);
        speed = 0;
    }

    protected virtual void loseHealth(int damage){
        health -= damage;
        StartCoroutine(GotHit());
    }

    protected virtual IEnumerator GotHit(){
        gotHit = true;
        SpritesOff();
        bullet.SetActive(false);
        hit.SetActive(true);
        yield return new WaitForSeconds(hitTime);
        gotHit = false;
        Walk0On();
    }

    protected virtual void MirrorXAxis(){
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
        facingRight = !facingRight;
    }

    protected virtual IEnumerator OnGroundExit(){
        runOffGround = true;
        yield return new WaitForSeconds(.05f);
        runOffGround = false;
        onGround = false;
    }

    protected virtual IEnumerator InvincibleSeconds(){
        invincible = true;
        yield return new WaitForSeconds(invincibleSeconds);
        invincible = false;
    }

    protected virtual void ChangeWalkSprite(){
        if(walking && onGround && Rigidbody.velocity.y == 0 && !attacking && health > 0 && !gotHit){
            walkCycle++;
            walkCycle %= 4;
            SpritesOff();
            switch(walkCycle){
                case 0:
                    walk0.SetActive(true);
                    break;
                case 1:
                    walk1.SetActive(true);
                    break;
                case 2:
                    walk2.SetActive(true);
                    break;
                case 3:
                    walk3.SetActive(true);
                    break;
            }
        } else if(onGround && !attacking && !gotHit){
            Walk0On();
        }
    }

    protected virtual void Walk0On(){
        SpritesOff();
        walk0.SetActive(true);
        walkCycle = 0;
    }

    protected virtual void SpritesOff(){
        walk0.SetActive(false);
        walk1.SetActive(false);
        walk2.SetActive(false);
        walk3.SetActive(false);
        attack.SetActive(false);
        jump.SetActive(false);
        death.SetActive(false);
        hit.SetActive(false);
    }
}
