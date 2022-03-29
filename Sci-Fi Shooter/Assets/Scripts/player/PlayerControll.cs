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
    Vector2 moveVector;
    Vector3 angles;

    public float moveSpeed;
    public float lookSpeed;

    public PlayerInventory inventory;

    public Transform gunpos;
    public bool syncGunAim;

    public int currentWeapon;
    public int previousWeapon;

    public Text hpText;
    public Text currentAmmoText;
    public Text maxAmmoText;
    public GameObject crossHair;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Instantiate(inventory.weaponInventory[0].weapon, gunpos.position, gunpos.rotation, gunpos);
        GetComponentInChildren<GunBase>().OnEquip(this);
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
        GetComponentInChildren<GunBase>().Fire(callbackContext);
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
    public void AimDownSights(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            GetComponentInChildren<Animator>().SetBool("ADS", true);
            syncGunAim = false;
            crossHair.SetActive(false);
        }
        else if (callbackContext.canceled)
        {
            GetComponentInChildren<Animator>().SetBool("ADS", false);
            syncGunAim = true;
            crossHair.SetActive(true);
        }
    }
    public void OnAltFire(InputAction.CallbackContext callbackContext)
    {
        GetComponentInChildren<GunBase>().AltFire(callbackContext);
    }
    public void EquipNext(int weaponIndex)
    {
        if (weaponIndex != currentWeapon)
        {
            previousWeapon = currentWeapon;
            currentWeapon = weaponIndex;
            GetComponentInChildren<GunBase>().OnUnEquip();
            Destroy(GetComponentInChildren<GunBase>().gameObject);
            GameObject newGun = Instantiate(inventory.weaponInventory[weaponIndex].weapon, gunpos.position, gunpos.rotation, gunpos);
            newGun.GetComponent<GunBase>().OnEquip(this);
        }
        //StartCoroutine(EquipingNext(weaponIndex));
    }
    public void Reload()
    {
        GetComponentInChildren<GunBase>().Reload();
    }
    IEnumerator EquipingNext(int weaponIndex)
    {
        
        yield return new WaitForSeconds(GetComponentInChildren<GunBase>().unEquipTime);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * moveVector.y * moveSpeed * Time.fixedDeltaTime + transform.right * moveVector.x * moveSpeed * Time.fixedDeltaTime);
        if (syncGunAim && Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f))
        {
            gunpos.LookAt(hit.point, cam.up);
        }
        else
        {
            gunpos.localRotation = new Quaternion();
        }
    }
    public void UpdateAmmo(AmmoType ammo)
    {
        currentAmmoText.text = inventory.weaponInventory[currentWeapon].currentAmmo.ToString();
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
        else
        {
            maxAmmoText.text = null;
        }
    }
}
