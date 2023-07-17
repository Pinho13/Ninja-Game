using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    const float groundCheckRadius = 0.2f;
    private bool isGrounded = false;
    public float JumpForce;
    [SerializeField]private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;


    [Header("Running")]
    [SerializeField] float maxSpeed;
    [SerializeField] float currentSpeed;
    [SerializeField] float speed;
    [SerializeField] float slowingDown;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Walk();
        Jump();
        GroundCheck();
        BetterJump();
        jumpBuffer();
    }

    void Walk()
    {
        float h = Input.GetAxis("Horizontal");
        currentSpeed = rb.velocity.x;

        if(Mathf.Abs(currentSpeed) < maxSpeed)
        {
            rb.AddForce(h * speed * Vector2.right, ForceMode2D.Force);
        }

        if(h == 0 && isGrounded)
        {
            if(currentSpeed > slowingDown)
            {
                rb.AddForce(slowingDown * Vector2.left, ForceMode2D.Force);
            }
            else if(currentSpeed < -slowingDown)
            {
                rb.AddForce(slowingDown * Vector2.right, ForceMode2D.Force);
            }

            if(Mathf.Abs(currentSpeed) < slowingDown - 0.5f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    void BetterJump()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * ( fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * ( lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    void GroundCheck()
    {
        isGrounded = false;
        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0)
        {
            isGrounded = true;
        }
    }

    void Jump()
    {
        if(jumpBufferCounter > 0f)
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            jumpBufferCounter = 0;
        }
    }

    void jumpBuffer()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        
        jumpBufferCounter -= Time.deltaTime;
    }


}
