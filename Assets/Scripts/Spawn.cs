using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawn : NetworkBehaviour, Attackable
{
    public EnemyAI enemyPrefab;
    public float spawnRateTimer;
    public bool isConstant;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (isConstant) StartCoroutine(SpawnEnemy());
    }


    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRateTimer);

            EnemyAI enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isConstant)
        {
            //Instantiate(, transform.position, Quaternion.identity);
            //Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attacked(float playerForceAmount, Vector3 forceDirection, float attackPower)
    {
        Destroy(gameObject);
    }

}
