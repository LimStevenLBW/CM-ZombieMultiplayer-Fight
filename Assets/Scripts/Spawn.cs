using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawn : NetworkBehaviour, Attackable
{
    public EnemyAI enemyPrefab;
    public int spawnLimit;
    public float spawnRateTimer;
    public bool isConstant;
    public bool isActive;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (isConstant && isActive) StartCoroutine(SpawnEnemy());
    }

    public void Trigger()
    {
        isActive = true;
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        int spawnAmount = 0;
        while (spawnAmount < spawnLimit)
        {
            spawnAmount++;
            yield return new WaitForSeconds(spawnRateTimer);

            EnemyAI enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
        }
    }
    public void Attacked(float playerForceAmount, Vector3 forceDirection, float attackPower)
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
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

}
