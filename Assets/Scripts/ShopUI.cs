using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private ImprovisedPlayerScript player;
    public void ShowShop()
    {
        gameObject.SetActive(true);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void HideShop()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayer(ImprovisedPlayerScript player)
    {
        this.player = player;

        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out ShopItem item))
            {
                item.SetPlayer(player);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
