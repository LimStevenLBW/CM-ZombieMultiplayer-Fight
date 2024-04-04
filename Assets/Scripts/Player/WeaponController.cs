using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    public GameObject heldWeaponModel;

    public void Equip(Weapon weapon)
    {
        this.weapon = weapon;
        Destroy(heldWeaponModel);
        heldWeaponModel = Instantiate(weapon.model, transform.position, Quaternion.identity);
        heldWeaponModel.gameObject.transform.SetParent(transform);
        heldWeaponModel.transform.localRotation = Quaternion.Euler(0,0,0);
        heldWeaponModel.transform.localPosition = Vector3.zero;
    }
}
