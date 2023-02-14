using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer sprite;
    int speed = 8;
    int jumpForce = 800;

    public LayerMask groundLayer;
    public Transform FeetTrans;
    float groundCheckDist = 1f; // bigger val = for wall stick/climb
    bool grounded = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
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
    }
}
