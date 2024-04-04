using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SideToSide : MonoBehaviour
{
    public Transform leftSide;
    public Transform rightSide;

    private Vector3 direction;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (direction * speed * Time.deltaTime);

        if (transform.position.x <= leftSide.position.x) direction = Vector3.right;
        if (transform.position.x >= rightSide.position.x) direction = Vector3.left;
    }
}
