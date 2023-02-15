using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCode : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    private float timer;
    private GameObject player;

    bool attack = false;

    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D _rigidbody;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // if player is close to enemy, shoot at player
        if (distance < 6 && !playerController.p.invis) {
            timer += Time.deltaTime;

            if (timer > 2) {
                timer = 0;
                shoot();
            }
            attack = true;
        } else {
            attack = false;
        } 


        // movement
        if (facingRight() && !attack) {
            _rigidbody.velocity = new Vector2(moveSpeed, 0f);
        } else if (!facingRight() && !attack) {
            _rigidbody.velocity = new Vector2(-moveSpeed, 0f);
        } else {
            _rigidbody.velocity = new Vector2(0f, 0f);
        }
    }

    void shoot() {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    private bool facingRight() {
        return transform.localScale.x > Mathf.Epsilon; // 0.0001f
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platforms")) {
            transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody.velocity.x)), transform.localScale.y);
        }
    }
}
