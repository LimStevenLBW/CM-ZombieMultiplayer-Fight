using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 direction2;

    public Rigidbody body;
    public float speed;
    public float jumpHeight;
    public float gravity;

    private bool isGrounded;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
        CheckForJumping();
    }

    void CheckForJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            if (isGrounded)
            {
                body.velocity += (new Vector3(0, jumpHeight, 0));
            }
        }
            
    }

    void FixedUpdate()
    {
        body.AddForce(Vector3.down * gravity);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = true;
        }
        
        if (other.tag == "Enemy")
        {
            
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = false;
        }

    }

    void CheckDirection()
    {
        if (Input.GetKeyDown("w"))
        {
            direction.z = 1;
        }
        if (Input.GetKeyDown("a"))
        {
            direction2.x = 1;
        }
        if (Input.GetKeyDown("s"))
        {
            direction2.z = 1;
        }
        if (Input.GetKeyDown("d"))
        {
            direction.x = 1;
        }


        if (Input.GetKeyUp("w"))
        {
            direction.z = 0;
        }
        if (Input.GetKeyUp("a"))
        {
            direction2.x = 0;
        }
        if (Input.GetKeyUp("s"))
        {
            direction2.z = 0;
        }
        if (Input.GetKeyUp("d"))
        {
            direction.x = 0;
        }

        transform.position += (direction.normalized * speed * Time.deltaTime);

        transform.position += (direction2.normalized * -speed * Time.deltaTime);
    }

}
