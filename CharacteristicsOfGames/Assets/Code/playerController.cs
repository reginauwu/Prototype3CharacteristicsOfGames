using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    Rigidbody2D _rigidbody;
    SpriteRenderer sprite;
    int speed = 10;
    int jumpForce = 800;

    public LayerMask groundLayer;
    public Transform FeetTrans;
    float groundCheckDist = 1f; // bigger val = for wall stick/climb
    bool grounded = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        grounded = Physics2D.OverlapCircle(FeetTrans.position, groundCheckDist, groundLayer);
    }

    void Update()
    {
        // left and right movement
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        _rigidbody.velocity = new Vector2(xSpeed, _rigidbody.velocity.y); // rb.velocity.y = grav
        if (xSpeed < 0) {
            sprite.flipX = true;
        }
        else if (xSpeed > 0) {
            sprite.flipX = false;
        }

        if (grounded && Input.GetButtonDown("Jump")) {
            _rigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }
}
