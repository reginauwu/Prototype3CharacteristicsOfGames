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

    // spike touching
    private bool isSpikeTouching;

    private Color originColor;

    private float lastDirection;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private Transform checkWall;
    [SerializeField] private LayerMask layerWall;
    [SerializeField] private Transform checkSpike;
    // [SerializeField] private Transform checkSpikeBL;
    // [SerializeField] private Transform checkSpikeTL;
    // [SerializeField] private Transform checkSpikeTR;
    
    [SerializeField] private LayerMask layerSpike;

    private void Update()
    {
        

        horizontal = Input.GetAxisRaw("Horizontal");


        lastDirection = horizontal==0?lastDirection:horizontal;
        
        vertical = Input.GetAxisRaw("Vertical");

        // wall grabbing
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

        // jump 

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // touch spike
        if(IsSpiked()){
            isSpikeTouching = true;
            print("danger");

        }else{
            isSpikeTouching = false;
        }

        // wall slide and wall jump
        WallSlide();
        WallJump();
        // WallJumpSpike();

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


    private bool IsSpiked(){
        // return (Physics2D.OverlapCircle(checkSpike.position, 0.1f,layerSpike)||Physics2D.OverlapCircle(checkSpikeDown.position, 0.1f,layerSpike));
        return Physics2D.OverlapArea(checkSpike.position +new Vector3(-0.6f,-0.6f,0f),checkSpike.position + new Vector3(0.6f,0.6f,0f),layerSpike);
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
        if (isWallSliding||isSpikeTouching)
        // if (isWallSliding)        
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
        //  (rb.velocity.x!=0||(rb.velocity.x==0&&rb.velocity.y!=0))&&
        if (isSpikeTouching && wallJumpingCounter > 0f)
        // if (isSpikeTouching && rb.velocity.x!=0&& wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            if(rb.velocity.y==0){
                // rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x * 0.5f, rb.velocity.y+6f);

                rb.velocity = new Vector2(lastDirection *-3f, rb.velocity.y+6f);
            }else{
                rb.velocity = new Vector2(-3f*lastDirection,6f);
                print("here");
            }
            

            


            wallJumpingCounter = 0f;
            
            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
            isSpikeTouching = false;
            Invoke (nameof(spikeShake),0.05f);
            print("111111");
            Invoke("resetColor",0.1f);
            print("222");
            Invoke (nameof(spikeShake),0.15f);
            print("333");
            
            Invoke("resetColor",0.2f);
            print("444");
        }





    }
    private void spikeShake(){
        originColor = this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        // Invoke("resetColor",0.2f);
    }
    private void resetColor(){
        // this.GetComponent<SpriteRenderer>().color = originColor;
        this.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);
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






    // private void WallJumpSpike(){
    //     // // if(isSpikeTouching){
    //     // //     if (Input.GetButtonDown("Horizontal")){
    //     // //     rb.transform.Translate(-1.0f*Input.GetAxisRaw("Horizontal"),0,0);
    //     // // }
    //     // // isSpikeTouching = false;
        

    //     // }

    //     // // if (isWallSliding)        
    //     // // {
    //     // //     isSpikeTouching = false;
    //     // //     wallJumpingDirection = -transform.localScale.x;
    //     // //     wallJumpingCounter = wallJumpingTime;

    //     // //     CancelInvoke(nameof(StopWallJumping));
    //     // // }
    //     // // else
    //     // // {
    //     // //     wallJumpingCounter -= Time.deltaTime;
    //     // // }

    //     // // if (Input.GetButtonDown("Horizontal")&& wallJumpingCounter > 0f)
    //     // // {
    //     // //     isSpikeTouching = true;
    //     // //     rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x * 0.2f, rb.velocity.y);
    //     // //     wallJumpingCounter = 0f;

    //     // //     if (transform.localScale.x != wallJumpingDirection)
    //     // //     {
    //     // //         isFacingRight = !isFacingRight;
    //     // //         Vector3 localScale = transform.localScale;
    //     // //         localScale.x *= -1f;
    //     // //         transform.localScale = localScale;
    //     // //     }

    //     // //     Invoke(nameof(StopWallJumping), wallJumpingDuration);
    //     // // }
    // }
}
