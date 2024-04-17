using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShopKeeper : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ImprovisedPlayerScript player = other.gameObject.GetComponent<ImprovisedPlayerScript>();
            ulong playerID = player.GetComponent<NetworkObject>().OwnerClientId;

            if(playerID == NetworkManager.Singleton.LocalClientId)
            {
                Cursor.lockState = CursorLockMode.None;
                player.ShowShop();
            }
            else
            {
               
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ImprovisedPlayerScript player = other.gameObject.GetComponent<ImprovisedPlayerScript>();
            ulong playerID = player.GetComponent<NetworkObject>().OwnerClientId;

            if (playerID == NetworkManager.Singleton.LocalClientId)
            {
                Cursor.lockState = CursorLockMode.Locked;
                player.HideShop();
            }
            else
            {

            }
        }
    }
}
