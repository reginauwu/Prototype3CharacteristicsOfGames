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
    int jumpForce = 800;

    public LayerMask groundLayer;
    public Transform FeetTrans;
    float groundCheckDist = 1f; // bigger val = for wall stick/climb
    bool grounded = false;

    public bool invis = false;
    float stamina = 10;

    //private Color origColor;

    void Start()
    {
        p = this;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        origColor = GetComponent<Renderer>().material.color;
    }

    private void FixedUpdate() {
        grounded = Physics2D.OverlapCircle(FeetTrans.position, groundCheckDist, groundLayer);
        _animator.SetBool("Grounded", grounded);
    }

    void Update()
    {
        // left and right movement
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        _rigidbody.velocity = new Vector2(xSpeed, _rigidbody.velocity.y); // rb.velocity.y = grav
        _animator.SetFloat("Speed", Mathf.Abs(xSpeed));

        // flip character left or right direction
        if (xSpeed < 0) {
            //sprite.flipX = true;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (xSpeed > 0) {
            //sprite.flipX = false;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
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
}
