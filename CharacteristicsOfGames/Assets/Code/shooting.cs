using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D _rigidbody;
    private float timer;

    private float flashTimer;
    
    public float force = 8;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = player.transform.position - transform.position;
        _rigidbody.velocity = new Vector2(dir.x, dir.y).normalized * force;

        float rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 5) {
            Destroy(gameObject);
        }
    }

    // if bullet collides with player, destroy
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Platforms"))) {
            Destroy(gameObject);
        }
    }
}
