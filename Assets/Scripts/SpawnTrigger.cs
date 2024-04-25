using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnTrigger : NetworkBehaviour
{
    public Spawn spawner;

    public void OnTriggerEnter(Collider other)
    {
        if (IsServer && other.gameObject.TryGetComponent(out ImprovisedPlayerScript player))
        {
            spawner.Trigger();
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }

    }

}
