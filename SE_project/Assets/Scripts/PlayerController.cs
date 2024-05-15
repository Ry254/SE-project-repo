using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    public Rigidbody2D playerRigidbody{get; private set;}
    public CapsuleCollider2D playerCollider{get; private set;}
    public float playerBase{get; private set;}
    [SerializeField] bool onGround;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        onGround = true;

        playerBase = transform.localScale.y/2;
    }

    void Update()
    {
        bool jumpDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        if(jumpDown && (onGround || playerRigidbody.velocity.y == 0)){
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            onGround = false;
        }

        if(playerRigidbody.velocity.y == 0){
            onGround = true;
        }
    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * Time.deltaTime * speed * horizontalAxis);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Ground") && playerRigidbody.velocity.y <= 0f){
            onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Ground")){
            StartCoroutine(OnGroundExit());
        }
    }

    IEnumerator OnGroundExit(){
        yield return new WaitForSeconds(.05f);
        onGround = false;
    }
}
