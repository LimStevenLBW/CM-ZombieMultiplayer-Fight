using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class Pickup : NetworkBehaviour
{
    public enum pickupType
    {
        health,
        gold,
        weapon
    }

    public pickupType type;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        Vector3 direction = new Vector3(target.position.x, target.position.y+1, target.position.z) - transform.position;
        transform.position += direction.normalized * 6 * Time.deltaTime;
    }

    public void Follow(Transform target)
    {
        this.target = target;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ulong id = other.gameObject.GetComponent<NetworkObject>().OwnerClientId;

            ImprovisedPlayerScript player = other.gameObject.GetComponent<ImprovisedPlayerScript>();

            /*
            ClientRpcParams clientParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { id }
                }
            };
            */

            if (type == pickupType.health)
            {
                player.Heal(25);
                Destroy(gameObject);
            }
            else if (type == pickupType.gold)
            {
                player.AddGold(5);
                Destroy(gameObject);
            }
            

        }
    }
}
