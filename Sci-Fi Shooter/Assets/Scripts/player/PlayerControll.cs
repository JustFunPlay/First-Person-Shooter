using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
}
