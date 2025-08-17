using System;
using UnityEngine;

public class CartridgesController : Bonus
{

    protected override bool Apply(GameObject interactor)
    {
        var cartridges = interactor.GetComponent<Cartridges>();
        if (!cartridges) return false;

        if (cartridges.IsFull) return false;

        cartridges.Take();
        return true;
    }
    // void Update()
    // {
    //     UpdateYPosBySin();
    //     UpdateRotation();
    // }
}
