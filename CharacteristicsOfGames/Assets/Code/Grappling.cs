using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    LineRenderer lineRenderer;
    SpringJoint2D springJoint;
    public Transform mouth;

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
        if(Input.GetButtonDown("Fire1")){
            lineRenderer.enabled = !lineRenderer.enabled;
            springJoint.enabled = !springJoint.enabled;
        }
        lineRenderer.SetPosition(0, mouth.position);
        // set the second position to the connected anchor which is attached to a rigidbody
        lineRenderer.SetPosition(1, springJoint.connectedAnchor + (Vector2)springJoint.connectedBody.transform.position);
    }
}
