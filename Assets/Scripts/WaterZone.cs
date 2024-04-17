using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ImprovisedPlayerScript player = other.gameObject.GetComponent<ImprovisedPlayerScript>();
            ulong playerID = player.GetComponent<NetworkObject>().OwnerClientId;

            if (playerID == NetworkManager.Singleton.LocalClientId)
            {
                Cursor.lockState = CursorLockMode.None;
                player.Respawn();
            }
            else
            {

            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
