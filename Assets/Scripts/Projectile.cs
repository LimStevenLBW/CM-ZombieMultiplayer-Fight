using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour, Attackable
{
    public float forceAmount;
    public float throwPower;
    private MeshRenderer mesh;
    public  float attackPower;
    public float despawnTime;
    private Rigidbody body;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        body = GetComponent<Rigidbody>();   
    }

    public void Fire(Vector3 direction)
    {
        body.AddForce(direction * throwPower);
        StartCoroutine(Despawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Despawn()
    {
        if (IsServer)
        {
            yield return new WaitForSeconds(despawnTime);
            GetComponent<NetworkObject>().Despawn();
        }
      
    }

    public float GetForce() { return throwPower; }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        if(collision.gameObject.GetComponent<Attackable>() != null)
        {
            Debug.Log(OwnerClientId + "Collision");
            Attackable attackable = collision.gameObject.GetComponent<Attackable>();
            Vector3 enemyPos = collision.gameObject.transform.position;
            
            Vector3 forceDirection = enemyPos - transform.position;

            attackable.Attacked(forceAmount, forceDirection.normalized, attackPower);
        }
    }

    public void Attacked(float forceAmount, Vector3 forceDirection, float attackPower)
    {
        StopAllCoroutines();
        StartCoroutine(Despawn());
        body.AddForce(forceDirection * forceAmount);
    }
}
