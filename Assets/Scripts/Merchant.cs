using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class Merchant : MonoBehaviour, IInteractableE
{
    [Header("Merchant Settings")]
    [SerializeField] string merchantName = "Shopkeeper";
    [SerializeField] float interactionRange = 3f;
    [SerializeField] int costAmmo = 50;


    private bool canInteract = false;
    public bool CanInteract => canInteract;

    void Start()
    {

    }

    void FixedUpdate()
    {
        //CheckPlayerDistance();

        
    }

    //void CheckPlayerDistance()
    //{
    //    if (PlayerController.LocalPlayer == null) return;

    //    float distance = Vector3.Distance(transform.position, PlayerController.LocalPlayer.transform.position);
    //    canInteract = distance <= interactionRange;

    //    // Optional: Show interaction prompt
    //    if (canInteract)
    //    {
    //        // You could show a UI prompt here
    //        Debug.Log("Press E to buy ammo");
    //    }
    //}

    public void InteractWithMe(PlayerController player)
    {
        BuyAmmo(player);
    }
    public void BuyAmmo(PlayerController player)
    {

        if (player.score >= costAmmo)
        {
            player.AddScore(-costAmmo);
            Debug.Log("Purchased: Ammo");
            Gun gun = player.Inventory.CurrentWeapon as Gun;
            gun.totalAmmo += 10;
            gun.AmmoChangedNotify(gun.CurrentAmmo, gun.totalAmmo);
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }
    /*void OnInteract()
    {
        if (canInteract)
        {
            BuyAmmo();
        }
    }*/

    
}
