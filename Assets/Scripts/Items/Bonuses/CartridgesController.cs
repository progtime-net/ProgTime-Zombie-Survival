using System;
using UnityEngine;

public class CartridgesController : Bonus
{
    
    [SerializeField] private GameObject aKAmmoPack;
    [SerializeField] private GameObject shotgunAmmoPack;
    [SerializeField] private GameObject sniperAmmoPack;
    [SerializeField] private int ammo;
    protected enum TypeState {Sniper,AK,Shotgun }
    private TypeState typeState;

    public override void StopBoost() { }
    private void Start()
    {
        startPosition = transform.position;
        int type=UnityEngine.Random.Range(1,3);
        if (type==1)
        {
            typeState = TypeState.Sniper;
            ammo = UnityEngine.Random.Range(5, 25);
            
        }
        else if(type==2)
        {
            typeState = TypeState.AK;
            ammo = UnityEngine.Random.Range(30, 150);
        }
        else
        {
            typeState = TypeState.Shotgun;
            ammo = UnityEngine.Random.Range(10, 50);
        }
        
    }

    protected override bool Apply(GameObject interactor)
    {
        return false; // DELETE
        /// TODO: Do this when Gun will complete
    }
}
