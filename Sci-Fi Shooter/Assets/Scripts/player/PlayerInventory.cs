using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "ScriptableObjects/Player inventory")]
public class PlayerInventory : ScriptableObject
{
    public GameObject primaryGun;
    public int currentPrimaryAmmo;
    public GameObject secondaryGun;
    public int currentSecondaryAmmo;
    public GameObject melee;
    
    public int lightAmmo;
    public int mediumAmmo;
    public int heavyAmmo;
    public int shotgunAmmo;

}
