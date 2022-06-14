using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControll : CharacterHealth
{
    Rigidbody rb;
    public Transform cam;
    public Transform camRot;
    public Transform leanPoint;
    Vector2 moveVector;
    Vector3 angles;

    public float moveSpeed;
    public float lookSpeed;

    public PlayerInventory inventory;

    public Transform gunpos;
    public bool syncGunAim;

    public GameObject weaponInHand;
    public WeaponSlot currentWeapon;
    public WeaponSlot previousWeapon;

    public Text hpText;
    public Text currentAmmoText;
    public Text maxAmmoText;
    public GameObject crossHair;

    public float baseFov;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        weaponInHand = Instantiate(inventory.meleeWeapon, cam.position, cam.rotation, cam);
        weaponInHand.GetComponent<GunBase>().OnEquip(this);
        baseFov = GetComponentInChildren<Camera>().fieldOfView;
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        Vector2 lookVector = callbackContext.ReadValue<Vector2>();
        transform.Rotate(0, lookVector.x * lookSpeed, 0);
        angles.x -= lookVector.y * lookSpeed;
        angles.x = Mathf.Clamp(angles.x, -90f, 90f);
        camRot.localRotation = Quaternion.Euler(angles);
    }
    public void OnFire(InputAction.CallbackContext callbackContext)
    {
        print("pressed Fire");
        weaponInHand.GetComponent<GunBase>().Fire(callbackContext);
        if (callbackContext.started)
        {
            print("should Fire");
        }
        if (callbackContext.canceled)
        {
            print("fire canceled");
        }
        if (callbackContext.performed)
        {
            print("fire performmed");
        }
    }
    public void SecondaryFire(InputAction.CallbackContext callbackContext)
    {
        weaponInHand.GetComponent<GunBase>().SecondaryFire(callbackContext);
    }
    public void OnAltFire(InputAction.CallbackContext callbackContext)
    {
        weaponInHand.GetComponent<GunBase>().AltFire(callbackContext);
    }
    public void Lean(InputAction.CallbackContext callbackContext)
    {
        Vector3 leanAngle = new Vector3(0, 0, 20 * callbackContext.ReadValue<float>());
        leanPoint.localRotation = Quaternion.Euler(leanAngle);
        print(callbackContext.ReadValue<float>());
    }
    public void EquipPrimary()
    {
        if (currentWeapon != WeaponSlot.Primary && inventory.primaryWeapon)
        {
            weaponInHand.GetComponent<GunBase>().OnUnEquip();
            weaponInHand = Instantiate(inventory.primaryWeapon, cam.position, cam.rotation, cam);
            previousWeapon = currentWeapon;
            currentWeapon = WeaponSlot.Primary;
            weaponInHand.GetComponent<GunBase>().OnEquip(this);
        }
    }
    public void EquipSecondary()
    {
        if (currentWeapon != WeaponSlot.Secondary && inventory.secondaryWeapon)
        {
            weaponInHand.GetComponent<GunBase>().OnUnEquip();
            weaponInHand = Instantiate(inventory.secondaryWeapon, cam.position, cam.rotation, cam);
            previousWeapon = currentWeapon;
            currentWeapon = WeaponSlot.Secondary;
            weaponInHand.GetComponent<GunBase>().OnEquip(this);
        }
    }
    public void EquipMelee()
    {
        if (currentWeapon != WeaponSlot.Melee && inventory.meleeWeapon)
        {
            weaponInHand.GetComponent<GunBase>().OnUnEquip();
            weaponInHand = Instantiate(inventory.meleeWeapon, cam.position, cam.rotation, cam);
            previousWeapon = currentWeapon;
            currentWeapon = WeaponSlot.Melee;
            weaponInHand.GetComponent<GunBase>().OnEquip(this);
        }
    }
    public void EquipGrenade()
    {
        if (currentWeapon != WeaponSlot.Grenade && inventory.grenades > 0 && inventory.grenade)
        {
            weaponInHand.GetComponent<GunBase>().OnUnEquip();
            weaponInHand = Instantiate(inventory.grenade, cam.position, cam.rotation, cam);
            previousWeapon = currentWeapon;
            currentWeapon = WeaponSlot.Grenade;
            weaponInHand.GetComponent<GunBase>().OnEquip(this);
        }
    }
    public void EquipAbility()
    {
        if (currentWeapon != WeaponSlot.Ability && inventory.abilityAvailibility > 0 && inventory.ability)
        {
            GameObject ability = Instantiate(inventory.ability, cam.position, cam.rotation, cam);
            ability.GetComponent<GunBase>().OnEquip(this);
        }
    }
    public void Reload()
    {
        weaponInHand.GetComponent<GunBase>().Reload();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1.1f))
        {
            rb.AddRelativeForce(moveVector.x * moveSpeed, 0, moveVector.y * moveSpeed, ForceMode.Acceleration);
            rb.drag = 6;
        }
        else
        {
            rb.AddRelativeForce(moveVector.x * moveSpeed * 0.01f, 0, moveVector.y * moveSpeed * 0.01f, ForceMode.Acceleration);
            rb.drag = 0.1f;
        }


    }
    public void UpdateAmmo(AmmoType ammo, WeaponSlot slot)
    {
        if (slot == WeaponSlot.Primary)
        {
            currentAmmoText.text = inventory.primaryAmmo.ToString();
        }
        else if (slot == WeaponSlot.Secondary)
        {
            currentAmmoText.text = inventory.secondaryAmmo.ToString();
        }
        else if (slot == WeaponSlot.Grenade)
        {
            currentAmmoText.text = inventory.grenades.ToString();
        }
        else
        {
            currentAmmoText.text = null;
        }

        if (ammo == AmmoType.Heavy)
        {
            maxAmmoText.text = inventory.heavyAmmo.ToString();
        }
        else if (ammo == AmmoType.Light)
        {
            maxAmmoText.text = inventory.lightAmmo.ToString();
        }
        else if (ammo == AmmoType.Medium)
        {
            maxAmmoText.text = inventory.mediumAmmo.ToString();
        }
        else if (ammo == AmmoType.Shotgun)
        {
            maxAmmoText.text = inventory.shotgunAmmo.ToString();
        }
        else if (ammo == AmmoType.RailGun)
        {
            maxAmmoText.text = inventory.railAmmo.ToString();
        }
        else
        {
            maxAmmoText.text = null;
        }
    }
}
