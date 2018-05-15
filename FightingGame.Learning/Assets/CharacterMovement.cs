using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speedX;
    public float jumpSpeedY;
    public float delayBeforeDoubleJump;

    bool facingRight, jumping, canJump, canDoubleJump;
    float speed;

    Animator anim;
    Rigidbody2D rb2d;
    //Transform transform;
    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        facingRight = true;
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(speed);
        HandleJumpAndFall();
        Flip();

        // Move Left 
        if (Input.GetKeyDown(KeyCode.A)) {
            speed = -speedX;
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            speed = 0;
        }

        // Move Right 
        if (Input.GetKeyDown(KeyCode.D)) {
            speed = speedX;
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

    }

    void MovePlayer(float playerSpeed)
    {
        if (playerSpeed < 0 && !jumping || playerSpeed > 0 && !jumping) {
            anim.SetInteger("State", 1);
        }
        if (speed == 0 && !jumping) {
            anim.SetInteger("State", 0);
        }

        rb2d.velocity = new Vector3(speed, rb2d.velocity.y, 0);
    }

    void HandleJumpAndFall()
    {
        if (jumping) {
            if (rb2d.velocity.y > 0) {
                anim.SetInteger("State", 2);
            }
            else {
                anim.SetInteger("State", 3);
            }
        }
    }

    void Flip()
    {
        if (speed > 0 && !facingRight || speed < 0 && facingRight) {
            facingRight = !facingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    void Jump()
    {
        if (canJump) {
            jumping = true;
            canJump = false;
            rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpSpeedY), ForceMode2D.Impulse);
            anim.SetInteger("State", 2);
            Invoke("EnableDoubleJump", delayBeforeDoubleJump);
        }
        if (canDoubleJump) {
            canDoubleJump = false;
            rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpSpeedY), ForceMode2D.Impulse);
            anim.SetInteger("State", 2);
        }
    }

    void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            jumping = false;
            canJump = true;
            canDoubleJump = false;
            anim.SetInteger("State", 0);
        }
    }
}

