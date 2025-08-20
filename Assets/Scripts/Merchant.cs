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

    public void BuyAmmo(PlayerController player)
    {

        if (player.score >= costAmmo)
        {
            player.score -= costAmmo;
            Debug.Log("Purchased: Ammo");
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

    public void InteractWithMe(PlayerController player)
    {
        BuyAmmo(player);
    }
}
