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

        
    }

    public void BuyAmmo(PlayerController player)
    {

        if (player.score >= costAmmo)
        {
            player.score -= costAmmo;
            //PlayerWeaponSpawner.AddAmmo();
            Debug.Log("Purchased: Ammo");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    public void InteractWithMe(PlayerController player)
    {
        BuyAmmo(player);
    }
}
