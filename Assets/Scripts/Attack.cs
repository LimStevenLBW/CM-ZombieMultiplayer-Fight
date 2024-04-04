using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float forceAmount;
    private MeshRenderer mesh;
    private float attackPower;


    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void Activate(float attackPower)
    {
        this.attackPower = attackPower;
        //mesh.enabled = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        //mesh.enabled = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Attackable>() != null)
        {
            Attackable attackable = other.gameObject.GetComponent<Attackable>();

            attackable.Attacked(forceAmount, transform.forward, attackPower);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
