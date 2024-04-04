using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    public float hoverSpeed = 3;
    public float hoverDistance = 0.25f;
    private float startHeight;
    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * 45 * Time.deltaTime);
        //transform.position = new Vector3(transform.position.x, 
       // startHeight + Mathf.Cos(Time.time * hoverSpeed) * hoverDistance,
       // transform.position.z);
    }
}
