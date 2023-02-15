using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public static playerController p;

    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer sprite;
    Color origColor;
    int speed = 8;
    public int jumpForce = 600;

    public LayerMask groundLayer;
    public Transform FeetTrans;
    float groundCheckDist = 0.3f; // bigger val = for wall stick/climb
    bool grounded = false;

    public bool invis = false;
    float stamina = 10;
    float scalex;

    // Consolidating code
    bool isWalled;
    bool isWallSliding;
    float wallSlidingSpeed = 1f;
    bool isWallJumping;
    float wallJumpingDirection;
    float wallJumpingTime = 0.2f;
    float wallJumpingCounter;
    float wallJumpingDuration = 0.4f;
    Vector2 wallJumpingPower = new Vector2(6f, 15f);
    private bool isWallGrabbing;
    public Transform checkWall;
    public float spikeLaunchForce = 7f;
    Color originColor;

    void Start()
    {
        p = this;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        origColor = GetComponent<Renderer>().material.color;
        scalex = transform.localScale.x;
    }

    private void FixedUpdate() {
        grounded = Physics2D.OverlapCircle(FeetTrans.position, groundCheckDist, groundLayer);
        _animator.SetBool("Grounded", grounded);
        if (!isWallJumping)
        {
            _rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, _rigidbody.velocity.y);
        }
        if(isWallGrabbing){
            _rigidbody.velocity = new Vector2(0, Input.GetAxis("Vertical") *speed);
        }
    }

    void Update()
    {
        // left and right movement
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        _rigidbody.velocity = new Vector2(xSpeed, _rigidbody.velocity.y); // rb.velocity.y = grav
        _animator.SetFloat("Speed", Mathf.Abs(xSpeed));

        // flip character left or right direction
        if (xSpeed < 0) {
            transform.localScale = new Vector3(-scalex, transform.localScale.y, transform.localScale.z);
        }
        else if (xSpeed > 0) {
            //sprite.flipX = false;
            transform.localScale = new Vector3(scalex, transform.localScale.y, transform.localScale.z);
        }

        // check if grounded and jumping
        if (grounded && Input.GetButtonDown("Jump")) {
            _rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.F) && !invis) {
            invis = true;
        } 

        if (Input.GetKeyDown(KeyCode.G) && invis) {
            invis = false;
        } 

        // wall grabbing
        isWalled = Physics2D.OverlapCircle(checkWall.position, 0.1f, groundLayer);
        if(isWalled  && Input.GetButton("Grab")){
            isWallGrabbing = true;
        }
        if(!isWalled ||!Input.GetButton("Grab")){
            isWallGrabbing = false;
            // print("grab false now");
        }else if(Input.GetButton("Grab")&&Input.GetButtonDown("Jump")){
            isWallGrabbing = false;
            _rigidbody.AddForce(new Vector2(jumpForce/2 * transform.localScale.x * -1, jumpForce));
        }
        if(isWallGrabbing){
            _rigidbody.gravityScale = 0;
        }else{
            _rigidbody.gravityScale = 2.5f; //same as previous setting 
        }

        if (invis) {
            gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            stamina -= Time.deltaTime * 2;
            //print(stamina);

            if (stamina < 0) {
                invis = false;
            }
        } else {
            gameObject.GetComponent<Renderer>().material.color = origColor;
            if (stamina < 10) {
                stamina += Time.deltaTime;
                //print(stamina);   
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 9){
            isWallGrabbing = false;
            print("spike");
            isWallJumping = false;
            Invoke (nameof(spikeShake),0.05f);
            Invoke("resetColor",0.1f);
            Invoke (nameof(spikeShake),0.15f);
            Invoke("resetColor",0.2f);

            // launch player away from spike
            _rigidbody.velocity = (transform.position - other.transform.position).normalized * spikeLaunchForce;
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
}
