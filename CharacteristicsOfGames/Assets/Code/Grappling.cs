using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    LineRenderer lineRenderer;
    SpringJoint2D springJoint;
    public Transform mouth;
    Rigidbody2D currentTarget;
    bool canGrapple = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        if (mouth == null)
        {
            mouth = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire3")){
            if(canGrapple){
                lineRenderer.enabled = !lineRenderer.enabled;
                springJoint.enabled = !springJoint.enabled;
                springJoint.connectedBody = currentTarget;
            }
            else{
                lineRenderer.enabled = false;
                springJoint.enabled = false;
            }
        }
        if(springJoint.enabled){
            lineRenderer.SetPosition(0, mouth.position);
            // set the second position to the connected anchor which is attached to a rigidbody
            lineRenderer.SetPosition(1, springJoint.connectedAnchor + (Vector2)springJoint.connectedBody.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.tag == "GrapplePoint")
        {
            canGrapple = true;
            currentTarget = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GrapplePoint")
        {
            canGrapple = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("exited");
        if (collision.gameObject.tag == "GrapplePoint")
        {
            canGrapple = false;
        }
    }
}
