using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Weapon weapon;
    private ImprovisedPlayerScript player;

    public void Buy()
    {
        int playerMoney = player.GetCoins();
        int cost = weapon.cost;

        if (playerMoney >= cost)
        {
            player.weaponController.Equip(weapon);
            playerMoney -= cost;
            player.SetCoins(playerMoney);
        }

    }
    public void SetInfo(Weapon weapon)
    {
        nameText.SetText(weapon.weaponName);

        string c = weapon.cost + " Gold";
        costText.SetText(c);

        Image image = GetComponent<Image>();
        image.sprite = weapon.sprite;

    }

    // Start is called before the first frame update
    void Start()
    {
        SetInfo(weapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
