using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon")]

public class Weapon : ScriptableObject
{
    public string weaponName;
    public int attackPower;
    public int cost;
    public GameObject model;
    public Sprite sprite;

}
