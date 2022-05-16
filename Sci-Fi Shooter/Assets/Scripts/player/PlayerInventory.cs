using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "ScriptableObjects/Player inventory")]
public class PlayerInventory : ScriptableObject
{
    public GameObject primaryWeapon;
    public int primaryAmmo;
    public GameObject secondaryWeapon;
    public int secondaryAmmo;
    public GameObject meleeWeapon;
    public GameObject grenade;
    public int grenades;
    public GameObject ability;
    public int abilityAvailibility;

    //public GameObject currentWeapon;
    
    public int lightAmmo;
    public int mediumAmmo;
    public int heavyAmmo;
    public int shotgunAmmo;
    public int railAmmo;
}