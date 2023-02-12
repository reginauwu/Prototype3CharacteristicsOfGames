using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }


    private float horizontal;
    private float speed = 6f;
    private float jumpingPower = 15f;
    private bool isFacingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 15f);

    // grab wall
    private bool isWallGrabbing;
    private float vertical;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private Transform checkWall;
    [SerializeField] private LayerMask layerWall;

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        vertical = Input.GetAxisRaw("Vertical");

        if(IsWalled()&& Input.GetButton("Grab")){
            isWallGrabbing = true;
        }
        if(!IsWalled()||!Input.GetButton("Grab")){
            isWallGrabbing = false;
            // print("grab false now");
        }else if(Input.GetButton("Grab")&&Input.GetButtonDown("Jump")){
            isWallGrabbing = false;
        }
        if(isWallGrabbing){
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x,0);
        }else{
            rb.gravityScale = 2.5f; //same as previous setting 
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        if(isWallGrabbing){
            rb.velocity = new Vector2(0, vertical *speed);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(checkGround.position, 0.2f, layerGround);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(checkWall.position, 0.2f, layerWall);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // rb.velocity = new Vector2(rb.velocity.x, 0);
            
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
