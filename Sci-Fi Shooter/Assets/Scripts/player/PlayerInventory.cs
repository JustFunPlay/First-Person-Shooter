using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "ScriptableObjects/Player inventory")]
public class PlayerInventory : ScriptableObject
{
    public WeaponInventory[] weaponInventory;

    //public GameObject currentWeapon;
    
    public int lightAmmo;
    public int mediumAmmo;
    public int heavyAmmo;
    public int shotgunAmmo;

}

[System.Serializable]
public class WeaponInventory
{
    public GameObject weapon;
    public int currentAmmo;
}