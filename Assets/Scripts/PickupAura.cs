using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAura : MonoBehaviour
{
    public ImprovisedPlayerScript player;
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
        if (other.gameObject.GetComponent<Pickup>())
        {
            Pickup pickup = other.gameObject.GetComponent<Pickup>();
            pickup.Follow(player.transform);

        }
    }
}
